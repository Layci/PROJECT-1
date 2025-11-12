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

        [Header("줌 보정")]
        public float minPitch = 10f;           // 줌인 시 카메라 각도 (낮음)
        public float maxPitch = 45f;           // 줌아웃 시 카메라 각도 (높음)
        public float minHeight = 3.5f;         // 줌인 시 카메라 높이
        public float maxHeight = 7f;           // 줌아웃 시 카메라 높이

        [Header("카메라 충돌 보정")]
        public LayerMask collisionMask;        // 충돌 감지용 레이어 (예: Default, Environment)
        public float cameraRadius = 0.3f;      // 충돌 감지 구체 반경
        public float collisionOffset = 0.2f;   // 벽에서 살짝 띄워주는 거리
        public float collisionSmooth = 10f;    // 충돌 시 부드러운 이동 정도

        private float yaw;   // 마우스 X
        private float pitch; // 마우스 Y
        private float targetDistance; // 보간용 거리
        private Vector3 currentCameraPos;

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
            //FollowTarget();
            FollowTargetWithCollision();
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

            // 카메라 줌 보정
            float t = Mathf.InverseLerp(minDistance, maxDistance, distance);

            // 거리 비율 기반으로 각도/높이 보간
            float targetPitch = Mathf.Lerp(minPitch, maxPitch, t);
            float targetHeight = Mathf.Lerp(minHeight, maxHeight, t);

            // 부드럽게 적용
            pitch = Mathf.Lerp(pitch, targetPitch, Time.deltaTime * 2f);
            offset.y = Mathf.Lerp(offset.y, targetHeight, Time.deltaTime * 5f);
        }
        void FollowTargetWithCollision()
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

            // 원래 카메라 목표 위치 계산
            Vector3 desiredPos = target.position - rotation * Vector3.forward * distance + Vector3.up * offset.y;
            Vector3 dir = desiredPos - target.position;
            float finalDistance = distance;

            // 카메라 충돌 감지 (SphereCast)
            if (Physics.SphereCast(target.position, cameraRadius, dir.normalized, out RaycastHit hit, distance, collisionMask))
            {
                finalDistance = Mathf.Clamp(hit.distance - collisionOffset, minDistance, distance);
            }

            // 최종 카메라 위치 계산
            Vector3 correctedPos = target.position - rotation * Vector3.forward * finalDistance + Vector3.up * offset.y;

            // 부드럽게 이동
            currentCameraPos = Vector3.Lerp(currentCameraPos, correctedPos, Time.deltaTime * collisionSmooth);

            transform.position = currentCameraPos;
            transform.rotation = rotation;
        }
        /*void FollowTarget()
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

            // 오프셋 + 거리 반영
            Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance + Vector3.up * offset.y;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.rotation = rotation;
        }*/
    }
}
