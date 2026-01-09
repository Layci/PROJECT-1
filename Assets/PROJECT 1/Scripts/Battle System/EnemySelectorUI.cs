using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Project1
{
    public class EnemySelectorUI : MonoBehaviour
    {
        public RectTransform selectorUI; // 선택 UI 오브젝트
        public Camera mainCamera;        // 메인 카메라 참조
        public bool isTurn = false;
        public static EnemySelectorUI instance;
        public float yOffset = 50f;    // Y값을 올릴 오프셋 값
        public List<BaseEnemyControl> allEnemies = new List<BaseEnemyControl>(); // 전투 중 등장한 모든 적
        //public List<Transform> allEnemies = new List<Transform>(); // 전투 중 등장한 모든 적
        public Transform selectedEnemy; // 현재 선택된 적의 Transform
        public List<RectTransform> multiSelectorUIs = new List<RectTransform>();

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
            if (selectedEnemy != null && isTurn)
            {
                // 선택된 적의 위치를 UI로 변환
                Vector3 screenPosition = mainCamera.WorldToScreenPoint(selectedEnemy.transform.position);
                screenPosition.y += yOffset;

                // UI의 위치 갱신
                selectorUI.position = screenPosition;
            }
            else if (!isTurn)
            {
                // 선택된 적이 없으면 UI를 숨김
                selectorUI.gameObject.SetActive(false);
            }
        }

        public void SetSelectedEnemy(Transform enemyTransform)
        {
            selectedEnemy = enemyTransform;
            if (selectedEnemy != null)
            {
                ShowSingleTargetUI();
            }
        }

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
            selectorUI.gameObject.SetActive(false);

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
        }

        public void HideAOEUI()
        {
            foreach (var ui in multiSelectorUIs)
            {
                ui.gameObject.SetActive(false);
            }
        }
    }
}
