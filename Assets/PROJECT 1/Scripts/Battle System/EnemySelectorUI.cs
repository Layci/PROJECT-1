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

            if(isTurn)
            {
                // 플레이어 턴이면 UI를 보이게 함
                selectorUI.gameObject.SetActive(true);
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

        // 현재 선택된 적 기준 범위 대상 계산+표시
        public void ShowCurrentAOERange()
        {
            HideSingleTargetUI();
            List<Transform> aoeTargets = GetAOETargetsFromSelection();
            ShowAOETargets(aoeTargets);
        }

        // EnemySelection에서 가져와서 transform 리스트로 만든 범위 대상
        private List<Transform> GetAOETargetsFromSelection()
        {
            List<Transform> results = new List<Transform>();
            if (EnemySelection.instance == null) return results;

            // 현재 턴 주체의 skillAttackRange 사용 (BaseUnit에 aoeRange가 있다고 가정)
            int range = 0;
            var cur = TurnSystem.instance != null ? TurnSystem.instance.CurrentCharacter : null;
            if (cur != null) range = Mathf.Max(0, cur.skillAttackRange);

            var targets = EnemySelection.instance.GetAOETargets(range);
            foreach (var e in targets)
            {
                if (e != null) results.Add(e.transform);
            }
            return results;

            /*List<Transform> results = new List<Transform>();
            if (EnemySelection.instance == null) return results;

            // EnemySelection의 인덱스 기반 메서드 호출
            // 현재 턴 플레이어의 skillAttackRange 사용
            int range = TurnSystem.instance.currentCharacter.skillAttackRange;
            var targets = EnemySelection.instance.GetAOETargets(range);
            foreach (var e in targets)
            {
                if (e != null)
                    results.Add(e.transform);
            }
            return results;*/
        }
        /*private List<Transform> GetAOETargetsFromSelection()
        {
            List<Transform> results = new List<Transform>();
            if (EnemySelection.instance == null) return results;

            var enemySelection = EnemySelection.instance;
            var targets = enemySelection.GetAOETargets(skillAttackRange);
            foreach (var e in targets)
            {
                if (e != null)
                    results.Add(e.transform);
            }
            return results;
        }*/

        public List<BaseEnemyControl> GetAOETargets()
        {
            var allEnemies = TurnSystem.instance.enemyCharacters; // 턴시스템의 적 리스트
            int targetIndex = EnemySelection.instance.selectedEnemyIndex; // 현재 선택 인덱스
            List<BaseEnemyControl> targets = new List<BaseEnemyControl>();

            if (targetIndex < 0 || targetIndex >= allEnemies.Count)
                return targets;

            // 중심 포함
            targets.Add(allEnemies[targetIndex]);

            // 좌우 범위
            for (int offset = 1; offset <= (TurnSystem.instance.CurrentCharacter as BaseCharacterControl).skillAttackRange; offset++)
            {
                int left = targetIndex - offset;
                int right = targetIndex + offset;

                if (left >= 0) targets.Add(allEnemies[left]);
                if (right < allEnemies.Count) targets.Add(allEnemies[right]);
            }

            return targets;
        }

        /*public List<BaseEnemyControl> GetAOETargets()
        {
            var allEnemies = EnemySelectorUI.instance.allEnemies;
            int targetIndex = EnemySelection.instance.GetSelectedEnemyIndex();
            List<BaseEnemyControl> targets = new List<BaseEnemyControl>();

            if (targetIndex < 0 || targetIndex >= allEnemies.Count)
                return targets;

            // 중심 포함
            targets.Add(allEnemies[targetIndex]);

            // 좌우 범위 포함
            for (int offset = 1; offset <= EnemySelectorUI.instance.skillAttackRange; offset++)
            {
                int left = targetIndex - offset;
                int right = targetIndex + offset;

                if (left >= 0) targets.Add(allEnemies[left]);
                if (right < allEnemies.Count) targets.Add(allEnemies[right]);
            }

            return targets;
        }*/

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

        /*public void ShowAOETargets(List<Transform> aoeTargets)
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
        }*/

        /*public List<Transform> GetAOETargets()
        {
            List<Transform> aoeTargets = new List<Transform>();

            if (selectedEnemy == null || allEnemies.Count == 0)
                return aoeTargets;

            int selectedIndex = allEnemies.IndexOf(selectedEnemy);
            if (selectedIndex == -1)
                return aoeTargets;

            for (int i = -skillAttackRange; i <= skillAttackRange; i++)
            {
                int targetIndex = selectedIndex + i;
                if (targetIndex >= 0 && targetIndex < allEnemies.Count)
                {
                    aoeTargets.Add(allEnemies[targetIndex]);
                }
            }

            return aoeTargets;
        }*/

        public void HideAOEUI()
        {
            foreach (var ui in multiSelectorUIs)
            {
                ui.gameObject.SetActive(false);
            }
        }

        /*public void DeselectEnemy()
        {
            selectedEnemy = null;
            HideSingleTargetUI();
        }*/

        /*public List<Transform> selectedEnemies = new List<Transform>();

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
        }*/
    }
}
