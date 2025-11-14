using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class MainPlayerControl : MonoBehaviour
    {
        [Header("이동 설정")]
        public float moveSpeed = 3f;
        public float runSpeed = 6f;
        public float rotateSpeed = 10f;
        public float gravity = -10f;

        [Header("캐릭터 상태")]
        public bool run = false;
        public bool holdRun = true;
        public bool isAttack = false;

        [Header("카메라 참조")]
        public Transform cameraTransform; // 3인칭 카메라의 Transform
        public Animator anim;

        CharacterController cr;
        private float verticalVelocity;

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
            cr = GetComponentInChildren<CharacterController>();
        }

        void Update()
        {
            MovePlayer();
            PlayerAttack();
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
        }

        void PlayerAttack()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttack)
            {
                Debug.Log("rrr");
            }
        }
    }
}
