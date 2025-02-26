using Project1;
using ProJect1;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Project1
{
    public class TurnSystem : MonoBehaviour
    {
        public static TurnSystem instance; // �̱��� �ν��Ͻ�

        public int currentTurn = 0; // ���� �������� ��
        public int selectedEnemyIndex = 0; // ���� ���õ� ���� �ε���
        public int randomIndex; // ���� ĳ���͸���Ʈ �ε���
        public int randomPoint; // ���� �� ��ų����Ʈ
        public List<BaseCharacterControl> playerCharacters; // �÷��̾� ĳ���� ����Ʈ
        public List<BaseEnemyControl> enemyCharacters; // �� ĳ���� ����Ʈ
        private List<object> allCharacters; // ��� ĳ���͸� �����ϴ� ����Ʈ
        public Transform playerTargetPosition;

        public List<BaseEnemyControl> activeEnemies = new List<BaseEnemyControl>(); // �� Ÿ�� ����Ʈ
        public List<BaseUnit> allUnits; // ������ �ִ� ��� ĳ���� (���� ������ ����Ʈ)

        public int currentTurnIndex = 0; // ���� ���� ����ϴ� ĳ������ �ε���
        public TurnOrderUI turnOrderUI;  // �� ���� UI ���� ��ũ��Ʈ

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // ��� ĳ���͸� ������ ����Ʈ�� �߰�
            playerCharacters = FindObjectsOfType<BaseCharacterControl>().ToList();
            enemyCharacters = FindObjectsOfType<BaseEnemyControl>().ToList();

            // ��� ĳ���͸� �ϳ��� ����Ʈ�� �߰�
            allCharacters = new List<object>();
            allCharacters.AddRange(playerCharacters);
            allCharacters.AddRange(enemyCharacters);
      
            // unitSpeed�� �������� �������� ����
            allCharacters = allCharacters.OrderByDescending(character =>
            {
                if (character is BaseCharacterControl player)
                    return player.unitSpeed;
                else if (character is BaseEnemyControl enemy)
                    return enemy.unitSpeed;
                return 0;
            }).ToList();

            SortEnemiesByPosition();

            StartTurn(); // ù ��° �� ����
        }

        // �� ó�� ����
        private void StartTurn()
        {
            if (currentTurnIndex >= allCharacters.Count)
                currentTurnIndex = 0; // �ε����� ����Ʈ�� �ʰ��ϸ� �ٽ� ó������

            foreach (var unit in allUnits)
            {
                unit.OnTurnStart(); // �� ĳ������ ���� ���� �� ����
            }

            if (allCharacters[currentTurnIndex] is BaseCharacterControl playerCharacter)
            {
                playerCharacter.isTurn = true;
                EnemySelectorUI.instance.isTurn = true;
                if (playerCharacter.isBlock)
                {
                    playerCharacter.isBlock = false;
                    playerCharacter.startBlocking = false;
                    playerCharacter.DoneBlock();
                }
            }
            else if (allCharacters[currentTurnIndex] is BaseEnemyControl enemyCharacter)
            {
                if(enemyCharacter.enemySkillPoint >= 2)
                {
                    enemyCharacter.skillAttack = true;
                    Debug.Log("�� ��ų����");
                }
                enemyCharacter.isTurn = true;
            }

            // ���� �÷��̾� Ÿ��
            RandomPlayer();

            // UI ����
            turnOrderUI.Initialize(allCharacters, currentTurnIndex);
        }

        // ���� ������ ȣ��
        public void EndTurn()
        {
            // ���� �� ĳ������ isTurn�� false�� ����
            if (allCharacters[currentTurnIndex] is BaseCharacterControl playerCharacter)
            {
                playerCharacter.isTurn = false;
                EnemySelectorUI.instance.isTurn = false;
                
            }
            else if (allCharacters[currentTurnIndex] is BaseEnemyControl enemyCharacter)
            {
                enemyCharacter.isTurn = false;
                randomPoint = Random.Range(1, 3);
                enemyCharacter.enemySkillPoint += randomPoint;
            }

            // ���� ĳ���ͷ� �Ѿ
            currentTurnIndex++;
            if (currentTurnIndex >= allCharacters.Count)
            {
                currentTurnIndex = 0; // �ε����� ����Ʈ�� �ʰ��ϸ� �ٽ� ó������
            }
            
            StartTurn(); // ���� �� ����
        }

        private void SortEnemiesByPosition()
        {
            // �� ����Ʈ�� x�� �������� �������� ����
            enemyCharacters = enemyCharacters.OrderBy(enemy => enemy.transform.position.x).ToList();
        }

        public void RemoveCharacterFromTurnOrder(object character)
        {
            // ����� ĳ���͸� ��ü �� ����Ʈ���� ����
            allCharacters.Remove(character);

            // �� ĳ���� ����Ʈ���� ����
            if (character is BaseEnemyControl enemy)
            {
                enemyCharacters.Remove(enemy);
            }

            // �÷��̾� ĳ���� ����Ʈ������ ���� ����
            if (character is BaseCharacterControl player)
            {
                playerCharacters.Remove(player);
            }

            // ���� �� �ε����� ����Ʈ ������ �ʰ����� �ʵ��� ����
            if (currentTurnIndex >= allCharacters.Count)
            {
                currentTurnIndex = 0;
            }
        }

        // �� �߰� �Լ�
        public void RegisterEnemy(BaseEnemyControl enemy)
        {
            // �� ����Ʈ�� �߰�
            enemyCharacters.Add(enemy);

            // ��ü �� ����Ʈ���� �߰�
            allCharacters.Add(enemy);

            // unitSpeed �������� �ٽ� ����
            allCharacters = allCharacters.OrderByDescending(character =>
            {
                if (character is BaseCharacterControl player)
                    return player.unitSpeed;
                else if (character is BaseEnemyControl enemyControl)
                    return enemyControl.unitSpeed;
                return 0;
            }).ToList();
        }

        public void RandomPlayer()
        {
            // isBlock�� true�� �÷��̾ �ִ��� Ȯ��
            BaseCharacterControl blockingCharacter = playerCharacters.FirstOrDefault(player => player.isBlock);

            if (blockingCharacter != null)
            {
                // ��� ���� ĳ���Ͱ� ������ �ش� ĳ���� �켱 Ÿ��
                playerTargetPosition = blockingCharacter.transform;
            }
            else
            {
                // ��� ���� ĳ���Ͱ� ������ ���� Ÿ�� ����
                randomIndex = Random.Range(0, playerCharacters.Count);
                playerTargetPosition = playerCharacters[randomIndex].transform;
            }
        }
    }
}
