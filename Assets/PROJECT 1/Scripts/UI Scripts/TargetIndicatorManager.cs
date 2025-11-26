using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class TargetIndicatorManager : MonoBehaviour
    {
        public static TargetIndicatorManager Instance;
        private readonly List<GameObject> activeIndicators = new();

        void Awake()
        {
            Instance = this;
        }

        /// UI에 이미 존재하는 TargetIcon 활성화
        public void ShowTargetIndicators(List<GameObject> targets)
        {
            ClearIndicators();
            Debug.Log("ddddddddddddddddddddddddddddddddddddddddddddd");
            foreach (var target in targets)
            {
                if (target == null) continue;

                // 플레이어 내부의 TargetIcon UI 찾기
                var targetIcon = target.transform.Find("UI_Canvas/TargetIcon");
                if (targetIcon != null)
                {
                    targetIcon.gameObject.SetActive(true);
                    activeIndicators.Add(targetIcon.gameObject);
                }
                else
                {
                    Debug.LogWarning($"[TargetIndicator] {target.name}에 TargetIcon이 없습니다.");
                }
            }

            Debug.Log($"[TargetIndicator] {activeIndicators.Count}명의 타겟 표시됨");
        }

        /// 모든 타겟 표시 제거
        public void ClearIndicators()
        {
            foreach (var icon in activeIndicators)
            {
                if (icon != null)
                    icon.SetActive(false);
            }
            activeIndicators.Clear();
        }
    }
}
