using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class AllySelectorUI : MonoBehaviour
    {
        public static AllySelectorUI instance;

        public RectTransform singleSelector;
        public Camera mainCamera;
        public float yOffset = 50f;

        private readonly List<RectTransform> aoeSelectors = new();
        private List<Transform> currentTargets = new();

        private void Awake()
        {
            instance = this;
            if (mainCamera == null)
                mainCamera = Camera.main;
        }

        private void Update()
        {
            if (currentTargets == null || currentTargets.Count == 0)
                return;

            for (int i = 0; i < currentTargets.Count; i++)
            {
                var target = currentTargets[i];
                if (target == null) continue;

                Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);
                screenPos.y += yOffset;

                if (i == 0)
                {
                    singleSelector.position = screenPos;
                }
                else if (i - 1 < aoeSelectors.Count)
                {
                    aoeSelectors[i - 1].position = screenPos;
                }
            }
        }

        public void ShowTargets(List<Transform> targets)
        {
            HideAll();

            if (targets == null || targets.Count == 0)
                return;

            currentTargets = targets;

            // 첫 번째는 단일 UI
            singleSelector.gameObject.SetActive(true);

            // 나머지는 AOE UI
            for (int i = 1; i < targets.Count; i++)
            {
                while (aoeSelectors.Count < i)
                {
                    var ui = Instantiate(singleSelector, singleSelector.parent);
                    aoeSelectors.Add(ui);
                }

                aoeSelectors[i - 1].gameObject.SetActive(true);
            }
        }

        public void HideAll()
        {
            //singleSelector.gameObject.SetActive(false);
            foreach (var ui in aoeSelectors)
                ui.gameObject.SetActive(false);

            currentTargets.Clear();
        }
    }
}
