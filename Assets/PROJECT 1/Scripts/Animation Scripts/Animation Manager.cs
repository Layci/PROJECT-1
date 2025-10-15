using Project1;
using ProJect1;
using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public class AnimationManager : MonoBehaviour
    {
        public static AnimationManager instans;

        private BaseCharacterControl player;
        private BaseEnemyControl enemy;
        public Transform target;

        public int totalDamage = 0;

        private void Awake()
        {
            player = GetComponentInParent<BaseCharacterControl>();
            enemy = GetComponentInParent<BaseEnemyControl>();

            instans = this;
        }

        private void Update()
        {
            // �÷��̾ �����ϴ� ���
            if (player != null)
            {
                // �÷��̾��� currentTarget�� Ÿ������ ����
                target = player.currentTarget;
            }
            // ���� �����ϴ� ���
            else if (enemy != null)
            {
                // ���� currentTarget�� Ÿ������ ����
                target = enemy.playerTransform;
            }
        }

        public void PlayerMeleeAttack()
        {
            if (player != null && player.currentState == PlayerState.Attacking)
            {
                // ������ ���ظ� ����           
                if (player.currentTarget != null)
                {
                    BaseEnemyControl enemyControl = player.currentTarget.GetComponent<BaseEnemyControl>();
                    if (enemyControl != null)
                    {
                        float damage = player.skillAttack ? player.playerSkillAttackPower : player.playerAttackPower;
                        // ������ ���ظ� �������� ���� �������� ���� ������ ����
                        damage *= player.damageIncreased;
                        // ������ ���ظ� ������ �� ���� �������� ���� ������ ����
                        damage *= enemyControl.damageReduction;
                        enemyControl.TakeDamage(damage);
                        totalDamage += (int)damage;

                        // �̱����� ����Ͽ� DamageTextSpawner ȣ��
                        if (DamageTextSpawner.Instance != null)
                        {
                            DamageTextSpawner.Instance.SpawnDamageText(target.position + Vector3.up * 1.5f, (int)damage);
                        }

                        TotalDamageUI.Instance.ShowTotalDamage(totalDamage);
                    }
                }
            }
        }

        // �ִϸ��̼� �̺�Ʈ���� ȣ��
        public void OnAttackEvent()
        {
            if (player == null) return;

            // ���� �� ĳ���� ��������
            var cur = TurnSystem.instance.allCharacters[TurnSystem.instance.currentTurnIndex] as BaseUnit;
            if (cur == null) return;

            // ���� ������ ���� ����
            int range = player.skillAttack ? cur.skillAttackRange : cur.normalAttackRange;

            var targets = EnemySelection.instance.GetAOETargets(range);

            // ���� ������ ������ ����
            float damage = player.skillAttack ? player.playerSkillAttackPower : player.playerAttackPower;
            damage *= player.damageIncreased;

            totalDamage = 0;

            foreach (var enemyControl in targets)
            {
                if (enemyControl == null) continue;
                // ���� ���� ������ ����
                float finalDamage = damage * enemyControl.damageReduction;
                enemyControl.TakeDamage(finalDamage);
                totalDamage += (int)finalDamage;

                if (DamageTextSpawner.Instance != null)
                    DamageTextSpawner.Instance.SpawnDamageText(enemyControl.transform.position + Vector3.up * 1.5f, (int)finalDamage);
            }

            if (TotalDamageUI.Instance != null)
                TotalDamageUI.Instance.ShowTotalDamage(totalDamage);
        }

        // ��� �ڵ�
        /*public void OnDamageEvent()
        {
            if (player == null) return;

            // ���� �� ĳ���� ��������
            var cur = TurnSystem.instance.allCharacters[TurnSystem.instance.currentTurnIndex] as BaseUnit;
            if (cur == null) return;

            int range = cur.skillAttackRange;
            List<BaseEnemyControl> targets;

            if (range > 0)
            {
                // ���� ����
                targets = EnemySelection.instance.GetAOETargets(range);
            }
            else
            {
                // ���� ����
                targets = new List<BaseEnemyControl>();
                if (player.currentTarget != null)
                {
                    BaseEnemyControl enemyControl = player.currentTarget.GetComponent<BaseEnemyControl>();
                    if (enemyControl != null) targets.Add(enemyControl);
                }
            }

            // ���ݷ� ��� (��ų ���δ� player.skillAttack���� ����)
            float damage = player.skillAttack ? player.playerSkillAttackPower : player.playerAttackPower;
            damage *= player.damageIncreased;

            totalDamage = 0;

            foreach (var enemyControl in targets)
            {
                if (enemyControl == null) continue;

                float finalDamage = damage * enemyControl.damageReduction;
                enemyControl.TakeDamage(finalDamage);
                totalDamage += (int)finalDamage;

                if (DamageTextSpawner.Instance != null)
                {
                    DamageTextSpawner.Instance.SpawnDamageText(enemyControl.transform.position + Vector3.up * 1.5f, (int)finalDamage);
                }
            }

            if (TotalDamageUI.Instance != null && totalDamage > 0)
            {
                TotalDamageUI.Instance.ShowTotalDamage(totalDamage);
            }
        }*/

        // �ִϸ��̼� �̺�Ʈ�� ȣ��Ǵ� ���� ����
        public void OnAOEDamageEvent()
        {
            if (player == null) return;

            // ���� �� ĳ���� ��������
            var cur = TurnSystem.instance.allCharacters[TurnSystem.instance.currentTurnIndex] as BaseUnit;
            if (cur == null) return;
            // ���� �������� �Ǵ� (��: �غ� �÷��׿� ����)
            int range = cur.skillAttackRange;
            var targets = EnemySelection.instance.GetAOETargets(range);

            float damage = player.skillAttack ? player.playerSkillAttackPower : player.playerAttackPower;
            damage *= player.damageIncreased;

            totalDamage = 0;

            foreach (var enemyControl in targets)
            {
                if (enemyControl == null) continue;

                // ���� ���� ������ ����
                float finalDamage = damage * enemyControl.damageReduction;
                enemyControl.TakeDamage(finalDamage);
                totalDamage += (int)finalDamage;

                if (DamageTextSpawner.Instance != null)
                {
                    DamageTextSpawner.Instance.SpawnDamageText(enemyControl.transform.position + Vector3.up * 1.5f, (int)finalDamage);
                }
            }

            if (TotalDamageUI.Instance != null)
            {
                TotalDamageUI.Instance.ShowTotalDamage(totalDamage);
            }
        }

        public void EnemyMeleeAttack()
        {
            if (enemy != null && enemy.currentState == EnemyState.Attacking)
            {
                // �÷��̾�� ���ظ� ����
                if (enemy.playerTransform != null)
                {
                    BaseCharacterControl playerControl = enemy.playerTransform.GetComponent<BaseCharacterControl>();
                    if (playerControl != null)
                    {
                        float damage = enemy.skillAttack ? enemy.enemySkillAttackPower : enemy.enemyAttackPower;
                        // �Ʊ����� ���ظ� �������� ���� �������� ���� ������ ����
                        damage *= enemy.damageIncreased;
                        // �Ʊ����� ���ظ� ������ �Ʊ� ���� �������� ���� ������ ����
                        damage *= playerControl.damageReduction;
                        playerControl.TakeDamage(damage);

                        // �̱����� ����Ͽ� DamageTextSpawner ȣ��
                        if (DamageTextSpawner.Instance != null)
                        {
                            DamageTextSpawner.Instance.SpawnDamageText(target.position + Vector3.up * 1.5f, (int)damage);
                        }
                    }
                }
            }
        }

        // Taster ĳ���Ͱ� �ǰ� �� ���� �Ŀ����
        public void TasterTakeDamaged()
        {
            BuffIconUI.instance.IncreaseBuffPower();
        }

        public void EndAttack()
        {
            // �� ���ط� �ʱ�ȭ
            totalDamage = 0;
        }

        public void EndBlock()
        {
            player.BlockEnd();
        }
    }
}