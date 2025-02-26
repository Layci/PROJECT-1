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
