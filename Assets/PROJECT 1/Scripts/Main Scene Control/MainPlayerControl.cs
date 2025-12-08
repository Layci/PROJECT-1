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

        [Header("카메라 참조")]
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

            // 카메라 기준 방향
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;

            Vector3 moveDir = (camForward * input.z + camRight * input.x).normalized;

            // 캐릭터 달리는 방식 전환
            if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                holdRun = !holdRun;
            }

            if (holdRun)
            {
                // 토글형
                if (Input.GetKeyDown(KeyCode.LeftShift))
                    run = !run;
            }
            else
            {
                // 유지형
                run = Input.GetKey(KeyCode.LeftShift);
            }
            
            float speed = run ? runSpeed : moveSpeed;

            Vector3 move = moveDir * speed;

            // === 중력 처리 ===
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

            // === 회전 ===
            if (moveDir.magnitude > 0.1f)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
            }

            // === 애니메이션 ===
            float animSpeed = move.magnitude / runSpeed;   // 0 ~ 1
            anim.SetFloat("Speed", animSpeed);

            // 플레이어가 움직이지 않으면
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
            //isAttacking = true;
            anim.SetTrigger("Attack");

            // 적의 Transform
            Transform target = enemy.transform;

            float dashSpeed = 12f;
            float stopDistance = 1.2f;

            // 적 방향 바라보기
            Vector3 targetDir = (target.position - transform.position).normalized;
            targetDir.y = 0;
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = targetRot;
            cr.enabled = false; // 이동충돌 방지 (돌진 시 필수)

            while (true)
            {
                if (target == null) break;

                Vector3 dir = (target.position - transform.position).normalized;
                float dist = Vector3.Distance(transform.position, target.position);

                // 도착하면 공격 판정
                if (dist <= stopDistance)
                    break;

                // XZ 평면 이동
                Vector3 move = new Vector3(dir.x, 0, dir.z);
                transform.position += move * dashSpeed * Time.deltaTime;

                yield return null;
            }

            // TODO: 적에게 실제 데미지 적용
            Debug.Log("Hit enemy: " + enemy.name);

            //isAttacking = false;
            cr.enabled = true;
        }

        public void RestorePlayerPosition()
        {
            transform.position = PartyFormationManager.Instance.lastFieldPosition;
        }
    }
}
