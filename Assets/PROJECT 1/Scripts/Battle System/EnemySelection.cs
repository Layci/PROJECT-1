using Project1;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project1
{
    public class EnemySelection : MonoBehaviour
    {
        public TurnSystem turnSystem;  // �� �ý��� ����
        public EnemySelectorUI enemySelectorUI; // ���� UI ���� ��ũ��Ʈ
        private int selectedEnemyIndex = 0;     // ���� ���õ� ���� �ε���
        public bool isMove = false;

        public static EnemySelection instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        private void Start()
        {
            EnemySelectorUI.instance.selectorUI.gameObject.SetActive(true);

            // ó�� ����� ó�� �� Ÿ��
            selectedEnemyIndex = 0;

            UpdateSelectedEnemy();
        }

        private void Update()
        {
            if (turnSystem.enemyCharacters.Count == 0) return;
            if (EnemySelectorUI.instance.isTurn && !isMove)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    selectedEnemyIndex--;
                    if (selectedEnemyIndex < 0)
                        selectedEnemyIndex = turnSystem.enemyCharacters.Count - 1;

                    UpdateSelectedEnemy(); // �߾� Ÿ�� �ð��� ����
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    selectedEnemyIndex++;
                    if (selectedEnemyIndex >= turnSystem.enemyCharacters.Count)
                        selectedEnemyIndex = 0;

                    UpdateSelectedEnemy(); // �߾� Ÿ�� �ð��� ����
                }
            }

            /*if (EnemySelectorUI.instance.isTurn && !isMove)
            {
                // �������� �� ����
                if (Input.GetKeyDown(KeyCode.A))
                {
                    selectedEnemyIndex--;
                    if (selectedEnemyIndex < 0)
                    {
                        selectedEnemyIndex = turnSystem.enemyCharacters.Count - 1; // ������ ������ ��ȯ
                    }
                    UpdateSelectedEnemy();
                }

                // ���������� �� ����
                if (Input.GetKeyDown(KeyCode.D))
                {
                    selectedEnemyIndex++;
                    if (selectedEnemyIndex >= turnSystem.enemyCharacters.Count)
                    {
                        selectedEnemyIndex = 0; // ù ��° ������ ��ȯ
                    }
                    UpdateSelectedEnemy();
                }
            }*/

        }

        public List<int> GetAOETargetIndices(int range = 1)
        {
            List<int> indices = new List<int>();
            int count = turnSystem.enemyCharacters.Count;

            for (int i = -range; i <= range; i++)
            {
                int index = selectedEnemyIndex + i;
                if (index >= 0 && index < count)
                {
                    indices.Add(index);
                }
            }

            return indices;
        }

        public int GetSelectedEnemyIndex()
        {
            return selectedEnemyIndex;
        }

        private void UpdateSelectedEnemy()
        {
            // ���õ� ���� UI�� �ݿ�
            BaseEnemyControl selectedEnemy = turnSystem.enemyCharacters[selectedEnemyIndex];
            enemySelectorUI.SetSelectedEnemy(selectedEnemy.transform);
        }

        // ���� �� �� ��ü(BaseEnemyControl) ����Ʈ ��ȯ - ���� ó����
        public List<BaseEnemyControl> GetAOETargets(int range)
        {
            List<BaseEnemyControl> targets = new List<BaseEnemyControl>();

            int start = Mathf.Max(0, selectedEnemyIndex - range);
            int end = Mathf.Min(turnSystem.enemyCharacters.Count - 1, selectedEnemyIndex + range);

            for (int i = start; i <= end; i++)
            {
                targets.Add(turnSystem.enemyCharacters[i]);
            }

            return targets;
        }
        /*public List<BaseEnemyControl> GetAOETargets(int range = 1)
        {
            List<BaseEnemyControl> targets = new List<BaseEnemyControl>();
            var allEnemies = turnSystem.enemyCharacters;

            for (int i = -range; i <= range; i++)
            {
                int index = selectedEnemyIndex + i;
                if (index >= 0 && index < allEnemies.Count)
                {
                    targets.Add(allEnemies[index]);
                }
            }

            return targets;
        }*/

        // ���� �� �� Transform ����Ʈ ��ȯ - UI ó����
        public List<Transform> GetAOETargets()
        {
            return GetAOETargets(1).Select(enemy => enemy.transform).ToList();
        }

        /*public BaseEnemyControl GetSelectedEnemy()
        {
            // ���� ���õ� �� ��ȯ
            if (turnSystem.enemyCharacters.Count > 0)
            {
                return turnSystem.enemyCharacters[selectedEnemyIndex];
            }
            return null;
        }*/
    }
}
