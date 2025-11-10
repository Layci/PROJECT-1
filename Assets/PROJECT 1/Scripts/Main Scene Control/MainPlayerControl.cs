using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class MainPlayerControl : MonoBehaviour
    {
        [Header("이동 설정")]
        public float moveSpeed = 5f;
        public float rotateSpeed = 10f;

        [Header("카메라 참조")]
        public Transform cameraTransform; // 주로 3인칭 카메라의 Transform
        public Animator anim;

        private Vector3 moveDirection;

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            MovePlayer();
        }

        void MovePlayer()
        {
            float h = Input.GetAxisRaw("Horizontal"); // A,D
            float v = Input.GetAxisRaw("Vertical");   // W,S
            Vector3 input = new Vector3(h, 0, v).normalized;

            // 카메라 기준 방향
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            moveDirection = (camForward * input.z + camRight * input.x).normalized;

            if (moveDirection.magnitude > 0.1f)
            {
                transform.position += moveDirection * moveSpeed * Time.deltaTime;

                Quaternion targetRot = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);

                anim.SetFloat("Speed", 1);
            }
            else
            {
                anim.SetFloat("Speed", 0);
            }
        }
    }
}
