using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("타겟 설정")]
        public Transform target;
        public Vector3 offset = new Vector3(0, 6, -6);

        [Header("카메라 회전 설정")]
        public float mouseSensitivity = 120f;
        public float minYAngle = -30f;
        public float maxYAngle = 70f;
        public float smoothSpeed = 10f;

        [Header("카메라 줌 설정")]
        public float distance = 6f;            // 기본 거리
        public float minDistance = 3f;         // 최대 줌인
        public float maxDistance = 10f;        // 최대 줌아웃
        public float zoomSensitivity = 2f;     // 마우스 휠 민감도
        public float zoomSmoothness = 10f;     // 거리 보간 속도

        private float yaw;   // 마우스 X
        private float pitch; // 마우스 Y
        private float targetDistance;          // 보간용 거리
        private Vector3 currentVelocity;

        void Start()
        {
            // 커서 잠금 및 숨김
            LockCursor(true);

            // 초기 각도
            Vector3 angles = transform.eulerAngles;
            yaw = angles.y;
            pitch = angles.x;

            // 줌 초기화
            targetDistance = distance;
        }

        void LateUpdate()
        {
            if (target == null) return;

            HandleCursorLock();
            HandleCameraRotation();
            HandleCameraZoom();
            FollowTarget();
        }

        void HandleCursorLock()
        {
            // Alt를 누르면 잠금 해제
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                LockCursor(false);
            }
            else if (Input.GetKeyUp(KeyCode.LeftAlt))
            {
                LockCursor(true);
            }
        }

        void LockCursor(bool locked)
        {
            Cursor.visible = !locked;
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        }

        void HandleCameraRotation()
        {
            // Alt 키를 누르고 있을 땐 카메라 회전 금지
            if (Input.GetKey(KeyCode.LeftAlt))
                return;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, minYAngle, maxYAngle);
        }

        void HandleCameraZoom()
        {
            // 마우스 휠 입력 (-값은 줌인, +값은 줌아웃)
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (Mathf.Abs(scroll) > 0.01f)
            {
                targetDistance -= scroll * zoomSensitivity;
                targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
            }

            // 부드럽게 거리 보간
            distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * zoomSmoothness);
        }

        void FollowTarget()
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

            // 오프셋 + 거리 반영
            Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance + Vector3.up * offset.y;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.rotation = rotation;
        }
    }
}
