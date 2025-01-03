using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project1
{
    public class TurnOrderUI : MonoBehaviour
    {
        public static TurnOrderUI instance; // �̱���
        public GameObject turnIconPrefab;  // �� ������ ������
        public Transform turnListParent;  // ������ ��ġ �θ� ������Ʈ

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        // UI�� �����ϴ� �Լ�
        public void UpdateTurnOrderUI(List<object> currentTurnOrder)
        {
            // ���� UI �ʱ�ȭ
            foreach (Transform child in turnListParent)
            {
                Destroy(child.gameObject);
            }

            // ���ο� �� ���� UI ����
            foreach (var character in currentTurnOrder)
            {
                GameObject icon = Instantiate(turnIconPrefab, turnListParent);
                TurnIcon iconScript = icon.GetComponent<TurnIcon>();

                if (character is BaseCharacterControl playerCharacter)
                {
                    iconScript.Setup(playerCharacter.unitIcon, playerCharacter.unitName);
                }
                else if (character is BaseEnemyControl enemyCharacter)
                {
                    iconScript.Setup(enemyCharacter.unitIcon, enemyCharacter.unitName);
                }
            }
        }
    }
}
