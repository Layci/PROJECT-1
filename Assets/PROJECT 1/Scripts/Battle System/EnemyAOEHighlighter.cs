using DG.Tweening;
using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class EnemyAOEHighlighter : MonoBehaviour
    {
        public static EnemyAOEHighlighter Instance;

        private List<GameObject> highlights = new List<GameObject>();

        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// 범위 내 대상에게 아웃라인 적용
        /// </summary>
        public void ShowAOETargets(List<GameObject> targets)
        {
            // 기존 하이라이트 모두 제거하고 새로 적용
            ClearAllHighlights();

            if (targets == null || targets.Count == 0) return;

            foreach (var t in targets)
            {
                if (t == null) continue;

                // OutlineHighlighter 컴포넌트 찾아서 Apply
                var outline = t.GetComponentInChildren<OutlineHighlighter>();
                if (outline != null)
                {
                    outline.ApplyOutline();
                    Debug.Log("메테리얼 바뀜!");
                }
                else
                {
                    // 만약 OutlineHighlighter가 없다면(테스트용) Log를 찍어두자
                    Debug.LogWarning($"[EnemyAOEHighlighter] OutlineHighlighter 없음: {t.name}");
                }

                highlights.Add(t);
            }
        }

        // 하이라이트 제거
        public void ClearAllHighlights()
        {
            foreach (var t in highlights)
            {
                if (t == null) continue;

                var outline = t.GetComponentInChildren<OutlineHighlighter>();
                if (outline != null)
                {
                    outline.ClearOutline();
                }
            }
            highlights.Clear();
        }

        /*public void ClearAllHighlights()
        {
            foreach (var h in activeHighlights)
            {
                if (h != null)
                    h.ClearOutline();
            }

            activeHighlights.Clear();
        }*/
    }
}
