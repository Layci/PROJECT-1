using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Project1
{
    public class EnemySelectorUI : MonoBehaviour
    {
        public static EnemySelectorUI instance;

        public RectTransform selectorUI; // 선택 UI 오브젝트
        public Camera mainCamera;        // 메인 카메라 참조
        public Canvas canvas; // EnemySelectorUI에 연결
        public float yOffset = 50f;    // Y값을 올릴 오프셋 값

        public bool isTurn = false;

        //public Transform selectedEnemy; // 현재 선택된 적의 Transform
        private List<Transform> currentAOETargets = new();
        public List<RectTransform> multiSelectorUIs = new();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        private void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main; // 메인 카메라 참조
            }
        }

        private void Update()
        {
            if (!isTurn) return;
            if (currentAOETargets == null || currentAOETargets.Count == 0) return;

            for (int i = 0; i < currentAOETargets.Count; i++)
            {
                if (i >= multiSelectorUIs.Count) continue;

                Transform target = currentAOETargets[i];
                if (target == null) continue;

                Vector2 screenPos =
                    RectTransformUtility.WorldToScreenPoint(mainCamera, target.position);

                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    screenPos + Vector2.up * yOffset,
                    canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
                    out Vector2 localPos
                );

                multiSelectorUIs[i].localPosition = localPos;
            }
        }

        /*private void Update()
        {
            if (selectedEnemy != null && isTurn)
            {
                UpdateAOEUI();
                *//*// 선택된 적의 위치를 UI로 변환
                Vector3 screenPosition = mainCamera.WorldToScreenPoint(selectedEnemy.transform.position);
                screenPosition.y += yOffset;

                // UI의 위치 갱신
                selectorUI.position = screenPosition;*//*
            }
            else if (!isTurn)
            {
                // 선택된 적이 없으면 UI를 숨김
                selectorUI.gameObject.SetActive(false);
            }
        }*/

        void UpdateAOEUI()
        {
            /*for (int i = 0; i < allEnemies.Count; i++)
            {
                if (i >= multiSelectorUIs.Count) continue;

                Vector3 screenPos = mainCamera.WorldToScreenPoint(allEnemies[i].transform.position);
                screenPos.y += yOffset;
                multiSelectorUIs[i].position = screenPos;
            }*/
        }

        /*public void SetSelectedEnemy(Transform enemyTransform)
        {
            selectedEnemy = enemyTransform;
            if (selectedEnemy != null)
            {
                ShowSingleTargetUI();
            }
        }*/

        public void ShowSingleTargetUI()
        {
            HideAOEUI();
            selectorUI.gameObject.SetActive(true);
        }

        public void HideSingleTargetUI()
        {
            selectorUI.gameObject.SetActive(false);
        }

        public void ShowAOETargets(List<Transform> aoeTargets)
        {
            currentAOETargets = aoeTargets;

            // UI 개수 확보
            while (multiSelectorUIs.Count < aoeTargets.Count)
            {
                RectTransform newUI = Instantiate(selectorUI, selectorUI.transform.parent);
                newUI.anchorMin = selectorUI.anchorMin;
                newUI.anchorMax = selectorUI.anchorMax;
                newUI.pivot = selectorUI.pivot;
                newUI.localScale = Vector3.one;
                multiSelectorUIs.Add(newUI);
            }

            // 활성화만 처리
            for (int i = 0; i < multiSelectorUIs.Count; i++)
            {
                multiSelectorUIs[i].gameObject.SetActive(i < aoeTargets.Count);
            }
        }

        /*public void ShowAOETargets(List<Transform> aoeTargets)
        {
            HideSingleTargetUI();
            while (multiSelectorUIs.Count < aoeTargets.Count)
            {
                RectTransform newUI = Instantiate(selectorUI, selectorUI.transform.parent);
                newUI.anchorMin = selectorUI.anchorMin;
                newUI.anchorMax = selectorUI.anchorMax;
                newUI.pivot = selectorUI.pivot;
                newUI.localScale = Vector3.one;
                multiSelectorUIs.Add(newUI);
            }

            for (int i = 0; i < multiSelectorUIs.Count; i++)
            {
                if (i < aoeTargets.Count)
                {
                    Transform enemy = aoeTargets[i];

                    Vector2 screenPos =
                        RectTransformUtility.WorldToScreenPoint(mainCamera, enemy.position);

                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        canvas.transform as RectTransform,
                        screenPos + Vector2.up * yOffset,
                        canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
                        out Vector2 localPos
                    );

                    multiSelectorUIs[i].localPosition = localPos;
                    multiSelectorUIs[i].gameObject.SetActive(true);
                }
                else
                {
                    multiSelectorUIs[i].gameObject.SetActive(false);
                }
            }
        }*/

        /*public void ShowAOETargets(List<Transform> aoeTargets)
        {
            //selectorUI.gameObject.SetActive(false);

            while (multiSelectorUIs.Count < aoeTargets.Count)
            {
                RectTransform newUI = Instantiate(selectorUI, selectorUI.transform.parent);
                multiSelectorUIs.Add(newUI);
            }

            for (int i = 0; i < multiSelectorUIs.Count; i++)
            {
                if (i < aoeTargets.Count)
                {
                    Transform enemy = aoeTargets[i];
                    Vector3 screenPos = mainCamera.WorldToScreenPoint(enemy.position);
                    screenPos.y += yOffset;

                    multiSelectorUIs[i].position = screenPos;
                    multiSelectorUIs[i].gameObject.SetActive(true);
                }
                else
                {
                    multiSelectorUIs[i].gameObject.SetActive(false);
                }
            }
        }*/

        public void HideAOEUI()
        {
            foreach (var ui in multiSelectorUIs)
            {
                ui.gameObject.SetActive(false);
            }
        }
    }
}
