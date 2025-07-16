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

            if(isTurn)
            {
                // 플레이어 턴이면 UI를 보이게 함
                selectorUI.gameObject.SetActive(true);
            }
        }

        public void ShowSingleTargetUI()
        {
            selectorUI.gameObject.SetActive(true);
        }

        public void HideSingleTargetUI()
        {
            selectorUI.gameObject.SetActive(false);
        }

        public void ShowAOETargets(List<Transform> aoeTargets)
        {
            // 기존 UI 숨김
            selectorUI.gameObject.SetActive(false);

            // 필요 수만큼 UI 오브젝트 생성 또는 재사용
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

        public List<Transform> selectedEnemies = new List<Transform>();

        public void HighlightEnemies(List<Transform> targets)
        {
            selectedEnemies = targets;

            // 예시: 각 적 위에 하이라이트 표시 오브젝트 활성화
            foreach (var enemy in selectedEnemies)
            {
                // 하이라이트 표시 오브젝트를 적 위에 띄우는 로직 추가
                // 예: enemy.GetComponent<EnemyUI>()?.ShowHighlight();
            }

            selectorUI.gameObject.SetActive(false); // 단일 선택 UI는 숨김
        }

        // 선택된 적 설정
        public void SetSelectedEnemy(Transform enemyTransform)
        {
            selectedEnemy = enemyTransform;

            if (selectedEnemy != null)
            {
                selectorUI.gameObject.SetActive(true); // UI 활성화
            }
        }

        // 선택 취소
        public void DeselectEnemy()
        {
            selectedEnemy = null;
            selectorUI.gameObject.SetActive(false);
        }
    }
}
