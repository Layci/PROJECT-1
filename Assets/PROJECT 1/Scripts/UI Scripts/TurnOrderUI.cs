using ProJect1;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project1
{
    public class TurnOrderUI : MonoBehaviour
    {
        public GameObject turnIconPrefab;     // �� ������ ������
        public Transform turnListParent;     // �� ����Ʈ �θ� ������Ʈ

        private List<BaseUnit> allCharacters;
        //private List<object> allCharacters;  // ��� ĳ���� ����Ʈ
        private int currentTurnIndex;        // ���� �� �ε���

        // �ʱ�ȭ �Լ�
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

        // �� ���� UI ����
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

            // ���� UI �ʱ�ȭ
            foreach (Transform child in turnListParent)
            {
                Destroy(child.gameObject);
            }

            // �� ���� ����
            int displayCount = 5; // ǥ���� �� ����
            for (int i = 0; i < displayCount; i++)
            {
                if (allCharacters == null || allCharacters.Count == 0)
                {
                    Debug.LogError("AllCharacters list is null or empty in UpdateTurnOrderUI.");
                    return;
                }

                int turnIndex = (currentTurnIndex + i) % allCharacters.Count;

                // ������ �ν��Ͻ� ����
                GameObject turnIconInstance = Instantiate(turnIconPrefab, turnListParent);

                // ������ ����
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
