using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project1
{
    public class TurnOrderUI : MonoBehaviour
    {
        public static TurnOrderUI instance; // 싱글톤
        public GameObject turnIconPrefab;  // 턴 아이콘 프리팹
        public Transform turnListParent;  // 아이콘 배치 부모 오브젝트

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        // UI를 갱신하는 함수
        public void UpdateTurnOrderUI(List<object> currentTurnOrder)
        {
            // 기존 UI 초기화
            foreach (Transform child in turnListParent)
            {
                Destroy(child.gameObject);
            }

            // 새로운 턴 순서 UI 생성
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
