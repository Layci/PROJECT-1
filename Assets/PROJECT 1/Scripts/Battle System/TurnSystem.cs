using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Project1
{
    public class TurnSystem : MonoBehaviour
    {
        public List<BaseCharacterControl> characters; // ��� ĳ���͸� �����ϴ� ����Ʈ
        private int currentCharacterIndex = 0; // ���� ���� �޴� ĳ������ �ε���

        private void Start()
        {
            // ĳ���͵��� �ӵ� ������ ����
            characters.Sort((a, b) => b.unitSpeed.CompareTo(a.unitSpeed));

            // ù ĳ������ �� ����
            StartTurn();
        }

        private void StartTurn()
        {
            if (characters.Count == 0) return;

            // ���� ���� ���� ĳ����
            BaseCharacterControl currentCharacter = characters[currentCharacterIndex];

            // ĳ���Ͱ� �̵� �� ������ �����ϵ��� ��
            currentCharacter.StartMoveToAttack();
        }

        public void EndTurn()
        {
            // ���� ���� ��ġ�� ���� ĳ���ͷ� �Ѿ
            currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
            StartTurn();
        }
    }
}
