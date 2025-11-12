using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("타겟 설정")]
        public Transform target;
        public List<Renderer> playerRenderers = new List<Renderer>(); // 여러 렌더러 지원
        public Vector3 offset = new Vector3(0, 6, -6);
        public float zoomYOffset = 2f; // 줌인할 때 Y를 얼마나 낮출지

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

        [Header("캐릭터 투명화 설정")]
        public float fadeStartDistance = 2.5f;  // 이 거리 이하로 가까워지면 서서히 투명
        public float fadeEndDistance = 1.2f;    // 완전히 투명해지는 거리
        public float fadeSpeed = 8f;            // 페이드 속도

        private float currentDistance;
        private float targetDistance; // 보간용 거리
        private float yaw;   // 마우스 X
        private float pitch; // 마우스 Y
        //private Vector3 currentCameraPos;
        private float currentAlpha = 1f;

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

            // 자동으로 자식 오브젝트에서 모든 Renderer를 찾기
            if (target != null && playerRenderers.Count == 0)
            {
                playerRenderers.AddRange(target.GetComponentsInChildren<Renderer>());
            }
        }

        void LateUpdate()
        {
            if (target == null) return;

            HandleCursorLock();
            HandleCameraRotation();
            HandleCameraZoom();
            FollowTargetWithCollision();
            HandleTransparency();
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
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (Mathf.Abs(scroll) > 0.01f)
            {
                targetDistance -= scroll * zoomSensitivity;
                targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
            }

            distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * zoomSmoothness);

            // 줌 중일 때만 offset.y 변경
            if (Mathf.Abs(scroll) > 0.01f)
            {
                float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
                float targetHeight = Mathf.Lerp(minHeight, maxHeight, t);
                offset.y = Mathf.Lerp(offset.y, targetHeight, Time.deltaTime * 5f);
            }
        }
        void FollowTargetWithCollision()
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 castStart = target.position + Vector3.up * 1.5f;
            Vector3 desiredPos = target.position - rotation * Vector3.forward * distance + Vector3.up * offset.y;
            Vector3 dir = desiredPos - castStart;
            float finalDistance = distance;

            bool isHit = Physics.SphereCast(castStart, cameraRadius, dir.normalized, out RaycastHit hit, distance, collisionMask);

            if (isHit)
            {
                finalDistance = Mathf.Clamp(hit.distance - collisionOffset, 0.5f, distance);
            }

            currentDistance = Mathf.Lerp(currentDistance, finalDistance, Time.deltaTime * collisionSmooth);

            // 충돌 시 벽 위로 튀지 않게 자연스럽게 캐릭터 쪽으로 이동
            Vector3 correctedPos = target.position - rotation * Vector3.forward * currentDistance + Vector3.up * offset.y;
            transform.position = correctedPos;
            transform.rotation = rotation;

            /*Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 castStart = target.position + Vector3.up * 1.5f;
            Vector3 desiredPos = target.position - rotation * Vector3.forward * distance + Vector3.up * offset.y;
            Vector3 dir = desiredPos - castStart;
            float finalDistance = distance;

            // ===== 충돌 감지 =====
            bool isHit = Physics.SphereCast(castStart, cameraRadius, dir.normalized, out RaycastHit hit, distance, collisionMask);

            if (isHit)
            {
                finalDistance = Mathf.Min(hit.distance - collisionOffset, distance);
            }
            else if (Physics.CheckSphere(desiredPos, cameraRadius, collisionMask))
            {
                finalDistance = Mathf.Max(0.2f, distance - 0.5f);
            }
            else
            {
                finalDistance = distance;
            }

            // ===== 거리 보간 =====
            if (isHit)
                currentDistance = Mathf.MoveTowards(currentDistance, finalDistance, Time.deltaTime * collisionSmooth * 10f);
            else
                currentDistance = Mathf.Lerp(currentDistance, finalDistance, Time.deltaTime * collisionSmooth);

            // ===== 카메라 최종 위치 =====
            Vector3 correctedPos = target.position - rotation * Vector3.forward * currentDistance + Vector3.up * offset.y;
            transform.position = correctedPos;
            transform.rotation = rotation;*/
        }

        void HandleTransparency()
        {
            if (playerRenderers == null || playerRenderers.Count == 0) return;

            float targetAlpha = 1f;

            // 카메라가 너무 가까워지면 투명도 적용
            if (currentDistance < fadeStartDistance)
            {
                targetAlpha = Mathf.InverseLerp(fadeEndDistance, fadeStartDistance, currentDistance);
            }

            currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * fadeSpeed);

            foreach (var renderer in playerRenderers)
            {
                foreach (var mat in renderer.materials)
                {
                    if (mat.HasProperty("_Color"))
                    {
                        Color color = mat.color;
                        color.a = currentAlpha;
                        mat.color = color;

                        // 만약 투명도가 적용 안 된다면 아래 코드로 RenderMode 변경 필요
                        if (currentAlpha < 0.99f)
                        {
                            SetMaterialTransparent(mat);
                        }
                        else
                        {
                            SetMaterialOpaque(mat);
                        }
                    }
                }
            }
        }

        // 머티리얼 모드 전환 함수 추가
        void SetMaterialTransparent(Material mat)
        {
            mat.SetFloat("_Mode", 3);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
        }

        void SetMaterialOpaque(Material mat)
        {
            mat.SetFloat("_Mode", 0);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            mat.SetInt("_ZWrite", 1);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = -1;
        }
    }
}
