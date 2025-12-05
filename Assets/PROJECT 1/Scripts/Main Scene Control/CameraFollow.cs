using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("타겟 설정")]
        public Transform target;
        public Transform characterRoot; // 캐릭터 MeshRoot (투명 처리용)
        public Vector3 offset = new Vector3(0, 6, -6);
        public float pivotHeight = 1.0f; // 회전 중심 위치 (허리 정도 높이)

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
        public float zoomYOffset = 2f; // 줌 시 카메라 높이 변화량

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
        public float fadeDistance = 3.5f; // 카메라가 이 거리보다 가까워지면 투명 처리
        public float fadeSpeed = 5f;
        public float fadedAlpha = 0.4f;

        private float yaw;   // 마우스 X
        private float pitch; // 마우스 Y
        private float targetDistance; // 보간용 거리
        private float currentDistance;

        private List<Renderer> characterRenderers = new List<Renderer>();
        private Dictionary<Renderer, Color[]> originalColors = new Dictionary<Renderer, Color[]>();
        private bool isFaded = false;

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
            currentDistance = distance;

            if (characterRoot)
                CacheCharacterRenderers();
        }

        void LateUpdate()
        {
            if (target == null) return;

            HandleCursorLock();
            HandleCameraRotation();
            HandleCameraZoom();
            FollowTargetWithCollision();
            HandleCharacterFade();
        }

        void HandleCursorLock()
        {
            // Alt를 누르면 잠금 해제
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                LockCursor(false);
            }
            else if (Input.GetKeyUp(KeyCode.LeftAlt) && !MainPlayerControl.instance.inputBlocked)
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
            if (Input.GetKey(KeyCode.LeftAlt) || MainPlayerControl.instance.inputBlocked)
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
        }

        /*void HandleCameraZoom()
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
        }*/

        void FollowTargetWithCollision()
        {
            // 줌 비율에 따라 카메라 높이 자동 보정
            float zoomT = Mathf.InverseLerp(minDistance, maxDistance, distance);
            float adjustedYOffset = Mathf.Lerp(offset.y - zoomYOffset, offset.y, zoomT);

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 pivot = target.position + Vector3.up * pivotHeight;
            Vector3 desiredPos = pivot - rotation * Vector3.forward * distance + Vector3.up * adjustedYOffset;

            Vector3 dir = desiredPos - pivot;
            float finalDistance = distance;

            bool isHit = Physics.SphereCast(pivot, cameraRadius, dir.normalized, out RaycastHit hit, distance, collisionMask);
            if (isHit)
                finalDistance = Mathf.Min(hit.distance - collisionOffset, distance);

            currentDistance = Mathf.Lerp(currentDistance, finalDistance, Time.deltaTime * collisionSmooth);

            Vector3 correctedPos = pivot - rotation * Vector3.forward * currentDistance + Vector3.up * adjustedYOffset;
            transform.position = correctedPos;
            transform.rotation = rotation;
        }

        /*void FollowTargetWithCollision()
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
        }*/

        // ===== 캐릭터 투명 처리 =====
        void HandleCharacterFade()
        {
            if (!characterRoot) return;

            float targetAlpha = (currentDistance < fadeDistance) ? fadedAlpha : 1f;

            foreach (var rend in characterRenderers)
            {
                if (rend == null) continue;

                var mats = rend.materials;
                for (int i = 0; i < mats.Length; i++)
                {
                    if (!originalColors.ContainsKey(rend)) continue;

                    Color baseColor = originalColors[rend][i];
                    float currentAlpha = mats[i].color.a;

                    // 부드럽게 알파 보간 (양방향)
                    float newAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * fadeSpeed);

                    baseColor.a = newAlpha;
                    mats[i].color = baseColor;

                    // 렌더링 모드 자동 전환
                    if (newAlpha < 0.99f)
                    {
                        mats[i].SetFloat("_Mode", 3); // Transparent
                        mats[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        mats[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        mats[i].SetInt("_ZWrite", 0);
                        mats[i].DisableKeyword("_ALPHATEST_ON");
                        mats[i].EnableKeyword("_ALPHABLEND_ON");
                        mats[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        mats[i].renderQueue = 3000;
                    }
                    else
                    {
                        mats[i].SetFloat("_Mode", 0); // Opaque
                        mats[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        mats[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                        mats[i].SetInt("_ZWrite", 1);
                        mats[i].DisableKeyword("_ALPHABLEND_ON");
                        mats[i].renderQueue = -1;
                    }
                }
            }

            isFaded = (targetAlpha < 0.99f);
        }

        void CacheCharacterRenderers()
        {
            var renderers = characterRoot.GetComponentsInChildren<Renderer>();
            foreach (var rend in renderers)
            {
                characterRenderers.Add(rend);
                var mats = rend.materials;
                Color[] colors = new Color[mats.Length];
                for (int i = 0; i < mats.Length; i++)
                    colors[i] = mats[i].color;
                originalColors.Add(rend, colors);
            }
        }
    }
}
