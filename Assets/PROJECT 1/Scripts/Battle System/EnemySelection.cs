using Project1;
using ProJect1;
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
        public int selectedEnemyIndex = 0;     // ���� ���õ� ���� �ε���
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
            if (enemySelectorUI != null)
            {
                enemySelectorUI.SetSelectedEnemy(GetSelectedEnemyTransform());
                enemySelectorUI.ShowSingleTargetUI();
            }

            // ó�� ����� ó�� �� Ÿ��
            selectedEnemyIndex = 0;
            UpdateSelectedEnemy();
        }

        private void Update()
        {
            if (turnSystem.enemyCharacters.Count == 0) return;

            if (EnemySelectorUI.instance.isTurn && !isMove)
            {
                bool moved = false;

                if (Input.GetKeyDown(KeyCode.A))
                {
                    selectedEnemyIndex--;
                    if (selectedEnemyIndex < 0)
                        selectedEnemyIndex = turnSystem.enemyCharacters.Count - 1;
                    moved = true;
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    selectedEnemyIndex++;
                    if (selectedEnemyIndex >= turnSystem.enemyCharacters.Count)
                        selectedEnemyIndex = 0;
                    moved = true;
                }

                if (moved)
                {
                    UpdateSelectedEnemy();
                }
            }
        }

        public void CheckAOESelect()
        {
            // �Ʊ� �⺻������ ������ ���
            if (enemySelectorUI != null && BaseCharacterControl.instance.normalAttackRange > 0)
            {

            }
        }

        public Transform GetSelectedEnemyTransform()
        {
            if (turnSystem.enemyCharacters.Count == 0) return null;
            return turnSystem.enemyCharacters[selectedEnemyIndex].transform;
        }

        public List<BaseEnemyControl> GetAOETargets(int range = 1)
        {
            List<BaseEnemyControl> targets = new List<BaseEnemyControl>();
            var allEnemies = turnSystem.enemyCharacters;

            // ���� ���õ� �� �ε���
            if (selectedEnemyIndex < 0 || selectedEnemyIndex >= allEnemies.Count)
                return targets;

            int start = Mathf.Max(0, selectedEnemyIndex - range);
            int end = Mathf.Min(allEnemies.Count - 1, selectedEnemyIndex + range);

            for (int i = start; i <= end; i++)
            {
                targets.Add(allEnemies[i]);
            }

            return targets;
        }

        public void UpdateSelectedEnemy()
        {
            if (turnSystem.enemyCharacters.Count == 0) return;

            BaseEnemyControl selectedEnemy = turnSystem.enemyCharacters[selectedEnemyIndex];

            // ���� ĳ���� ��������
            var cur = TurnSystem.instance.allCharacters[TurnSystem.instance.currentTurnIndex] as BaseUnit;
            //int range = cur != null ? cur.skillAttackRange : 0;

            if (cur is BaseCharacterControl player)
            {
                if (player.prepareState == AttackPrepareState.Basic)
                {
                    // �⺻ ���� �� normalAttackRange ���
                    int normalRange = player.normalAttackRange;

                    enemySelectorUI.SetSelectedEnemy(selectedEnemy.transform);

                    if (normalRange == 0)
                    {
                        // ����
                        enemySelectorUI.ShowSingleTargetUI();
                        enemySelectorUI.HideAOEUI();
                    }
                    else
                    {
                        // ����
                        var targets = GetAOETargets(normalRange);
                        enemySelectorUI.ShowAOETargets(targets.Select(e => e.transform).ToList());
                        enemySelectorUI.HideSingleTargetUI();
                    }
                }
                else if (player.prepareState == AttackPrepareState.Skill)
                {
                    // ��ų ���� �� skillAttackRange ���
                    int skillRange = player.skillAttackRange;

                    enemySelectorUI.SetSelectedEnemy(selectedEnemy.transform);

                    if (skillRange == 0)
                    {
                        enemySelectorUI.ShowSingleTargetUI();
                        enemySelectorUI.HideAOEUI();
                    }
                    else
                    {
                        var targets = GetAOETargets(skillRange);
                        enemySelectorUI.ShowAOETargets(targets.Select(e => e.transform).ToList());
                        enemySelectorUI.HideSingleTargetUI();
                    }
                }
            }
            
            //����

            /*if (cur is BaseCharacterControl player)
            {
                if (player.prepareState == AttackPrepareState.Basic)
                {
                    // �⺻ ���� �� normalAttackRange ���
                    int normalRange = player.normalAttackRange;

                    enemySelectorUI.SetSelectedEnemy(selectedEnemy.transform);

                    if (normalRange == 0)
                    {
                        // ����
                        enemySelectorUI.ShowSingleTargetUI();
                        enemySelectorUI.HideAOEUI();
                    }
                    else
                    {
                        // ����
                        var targets = GetAOETargets(normalRange);
                        enemySelectorUI.ShowAOETargets(targets.Select(e => e.transform).ToList());
                        enemySelectorUI.HideSingleTargetUI();
                    }
                }
                else if (player.prepareState == AttackPrepareState.Skill)
                {
                    // ��ų ���� �� skillAttackRange ���
                    int skillRange = player.skillAttackRange;

                    enemySelectorUI.SetSelectedEnemy(selectedEnemy.transform);

                    if (skillRange == 0)
                    {
                        enemySelectorUI.ShowSingleTargetUI();
                        enemySelectorUI.HideAOEUI();
                    }
                    else
                    {
                        var targets = GetAOETargets(skillRange);
                        enemySelectorUI.ShowAOETargets(targets.Select(e => e.transform).ToList());
                        enemySelectorUI.HideSingleTargetUI();
                    }
                }
            }*/
        }
    }
}
