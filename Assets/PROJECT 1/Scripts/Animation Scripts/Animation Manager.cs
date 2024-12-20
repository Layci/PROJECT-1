using Project1;
using UnityEngine;

namespace Project1
{
    public class AnimationManager : MonoBehaviour
    {
        private BaseCharacterControl player;
        private BaseEnemyControl enemy;
        public Transform target;

        private void Awake()
        {
            player = GetComponentInParent<BaseCharacterControl>();
            enemy = GetComponentInParent<BaseEnemyControl>();
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
                        enemyControl.TakeDamage(damage);

                        // �̱����� ����Ͽ� DamageTextSpawner ȣ��
                        if (DamageTextSpawner.Instance != null)
                        {
                            DamageTextSpawner.Instance.SpawnDamageText(target.position + Vector3.up * 1.5f, (int)damage);
                        }
                    }
                }
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
    }
}