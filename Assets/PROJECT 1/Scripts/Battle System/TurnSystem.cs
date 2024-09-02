using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Project1
{
    public class TurnSystem : MonoBehaviour
    {
        public List<BaseCharacterControl> characters; // 모든 캐릭터를 저장하는 리스트
        private int currentCharacterIndex = 0; // 현재 턴을 받는 캐릭터의 인덱스

        private void Start()
        {
            // 캐릭터들을 속도 순으로 정렬
            characters.Sort((a, b) => b.unitSpeed.CompareTo(a.unitSpeed));

            // 첫 캐릭터의 턴 시작
            StartTurn();
        }

        private void StartTurn()
        {
            if (characters.Count == 0) return;

            // 현재 턴을 받을 캐릭터
            BaseCharacterControl currentCharacter = characters[currentCharacterIndex];

            // 캐릭터가 이동 및 공격을 시작하도록 함
            currentCharacter.StartMoveToAttack();
        }

        public void EndTurn()
        {
            // 현재 턴을 마치고 다음 캐릭터로 넘어감
            currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
            StartTurn();
        }
    }
}
