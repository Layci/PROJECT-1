using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProJect1
{
    public class MainPlayerControl : MonoBehaviour
    {
        [Header("이동 설정")]
        public float moveSpeed = 3f;
        public float runSpeed = 6f;
        public float rotateSpeed = 10f;
        public float gravity = -10f;
        public float stopMoveTime = 0;

        [Header("캐릭터 상태")]
        public bool run = false;
        public bool holdRun = true;
        public bool isAttack = false;
        public bool isAttacking = false;
        public bool inputBlocked = false;

        [Header("카메라 설정")]
        public Transform cameraTransform; // 3인칭 카메라의 Transform
        public Animator anim;

        CharacterController cr;
        public static MainPlayerControl instance;
        private float verticalVelocity;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
            cr = GetComponentInChildren<CharacterController>();

            // 안전장치: 전투 후 메인씬 복귀 시 공격/입력 제한 상태 강제 초기화
            isAttacking = false;
            inputBlocked = false;
            if (cr != null) cr.enabled = true;

            //transform.position = PartyFormationManager.Instance.lastFieldPosition;
            Debug.Log(PartyFormationManager.Instance.lastFieldPosition);
        }

        void Update()
        {
            if (!isAttacking && !inputBlocked)
                MovePlayer();

            if (Input.GetMouseButtonDown(0) && !inputBlocked && !EventSystem.current.IsPointerOverGameObject())
                TryAttack();
        }

        void MovePlayer()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector3 input = new Vector3(h, 0, v).normalized;

            
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;

            Vector3 moveDir = (camForward * input.z + camRight * input.x).normalized;

            // 달리기 방식 변환
            if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                holdRun = !holdRun;
            }

            if (holdRun)
            {
                // 누르는 동안
                if (Input.GetKeyDown(KeyCode.LeftShift))
                    run = !run;
            }
            else
            {
                // 토글 방식
                run = Input.GetKey(KeyCode.LeftShift);
            }
            
            float speed = run ? runSpeed : moveSpeed;

            Vector3 move = moveDir * speed;

            if (cr.isGrounded)
            {
                if (verticalVelocity < -2f)
                    verticalVelocity = -2f;
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            Vector3 finalMove = move + new Vector3(0, verticalVelocity, 0);
            cr.Move(finalMove * Time.deltaTime);

            if (moveDir.magnitude > 0.1f)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
            }

            float animSpeed = move.magnitude / runSpeed;   // 0 ~ 1
            anim.SetFloat("Speed", animSpeed);

            if (moveDir.magnitude < 0.1f)
            {
                stopMoveTime += Time.deltaTime;
            }
            else
            {
                stopMoveTime = 0;
            }

            if (stopMoveTime >= 10)
            {
                anim.SetTrigger("Rest");
                stopMoveTime = 0;
            }
        }

        void TryAttack()
        {
            if (isAttacking) return;

            MainSenceEnemy enemy = PlayerCombat.instance.currentTarget;
            isAttacking = true;
            if (enemy != null)
                StartCoroutine(DashAndAttack(enemy));
            else
                anim.SetTrigger("Attack");
        }

        IEnumerator DashAndAttack(MainSenceEnemy enemy)
        {
            PartyFormationManager.Instance.lastFieldPosition = transform.position;
            anim.SetTrigger("Attack");

            // 타겟 설정
            Transform target = enemy.transform;

            float dashSpeed = 12f;
            float stopDistance = 1.2f;

            // 적 방향 바라보기
            Vector3 targetDir = (target.position - transform.position).normalized;
            targetDir.y = 0;
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = targetRot;
            cr.enabled = false; // 이동 충돌 일시 정지 (대쉬 중 벽 뚫기 방지 및 부드러운 이동)

            while (true)
            {
                if (target == null) break;

                Vector3 dir = (target.position - transform.position).normalized;
                float dist = Vector3.Distance(transform.position, target.position);

                // 목표 거리에 도착하면 중단
                if (dist <= stopDistance)
                    break;

                // XZ 평면 이동
                Vector3 move = new Vector3(dir.x, 0, dir.z);
                transform.position += move * dashSpeed * Time.deltaTime;

                yield return null;
            }

            // 데미지 전달 등의 로직 처리 지점
            Debug.Log("Hit enemy: " + enemy.name);

            cr.enabled = true;
        }

        public void RestorePlayerPosition()
        {
            transform.position = PartyFormationManager.Instance.lastFieldPosition;
        }
    }
}
