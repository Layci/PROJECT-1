using ProJect1;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project1
{
    public class TurnOrderUI : MonoBehaviour
    {
        public GameObject turnIconPrefab;     // 턴 아이콘 프리팹
        public Transform turnListParent;     // 턴 리스트 부모 오브젝트

        private List<BaseUnit> allCharacters;
        //private List<object> allCharacters;  // 모든 캐릭터 리스트
        private int currentTurnIndex;        // 현재 턴 인덱스

        // 초기화 함수
        public void Initialize(List<BaseUnit> characters, int currentIndex)
        {
            if (characters == null || characters.Count == 0)
            {
                Debug.LogError("Characters list is null or empty in TurnOrderUI.Initialize.");
                return;
            }

            allCharacters = characters;
            currentTurnIndex = currentIndex;

            UpdateTurnOrderUI();
        }
        /*public void Initialize(List<object> characters, int currentIndex)
        {
            if (characters == null || characters.Count == 0)
            {
                Debug.LogError("Characters list is null or empty in TurnOrderUI.Initialize.");
                return;
            }

            allCharacters = characters;
            currentTurnIndex = currentIndex;

            UpdateTurnOrderUI();
        }*/

        // 턴 순서 UI 갱신
        public void UpdateTurnOrderUI()
        {
            if (turnIconPrefab == null)
            {
                Debug.LogError("Turn Icon Prefab is not assigned in TurnOrderUI.");
                return;
            }

            if (turnListParent == null)
            {
                Debug.LogError("Turn List Parent is not assigned in TurnOrderUI.");
                return;
            }

            // 기존 UI 초기화
            foreach (Transform child in turnListParent)
            {
                Destroy(child.gameObject);
            }

            // 턴 순서 생성
            int displayCount = 5; // 표시할 턴 개수
            for (int i = 0; i < displayCount; i++)
            {
                if (allCharacters == null || allCharacters.Count == 0)
                {
                    Debug.LogError("AllCharacters list is null or empty in UpdateTurnOrderUI.");
                    return;
                }

                int turnIndex = (currentTurnIndex + i) % allCharacters.Count;

                // 프리팹 인스턴스 생성
                GameObject turnIconInstance = Instantiate(turnIconPrefab, turnListParent);

                // 데이터 설정
                TurnIcon iconScript = turnIconInstance.GetComponent<TurnIcon>();
                if (iconScript == null)
                {
                    Debug.LogError("TurnIcon component is missing on the prefab.");
                    continue;
                }

                if (allCharacters[turnIndex] is BaseCharacterControl playerCharacter)
                {
                    iconScript.Setup(playerCharacter.unitIcon, playerCharacter.unitName, true);
                }
                else if (allCharacters[turnIndex] is BaseEnemyControl enemyCharacter)
                {
                    iconScript.Setup(enemyCharacter.unitIcon, enemyCharacter.unitName, false);
                }
            }
        }
    }
}
