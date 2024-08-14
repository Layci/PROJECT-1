using Project1;
using UnityEngine;

namespace ProJect1
{
    public class AnimationManager : MonoBehaviour
    {
        private BaseCharacterControl player;
        private BaseEnemyControl enemy;

        private void Awake()
        {
            player = GetComponentInParent<BaseCharacterControl>();
            enemy = GetComponentInParent<BaseEnemyControl>();
        }

        public void PlayerMeleeAttack()
        {
            if (player != null && player.currentState == PlayerState.Attacking)
            {
                // ������ ���ظ� ����
                if (player.enemy != null)
                {
                    BaseEnemyControl enemyControl = player.enemy.GetComponent<BaseEnemyControl>();
                    if (enemyControl != null)
                    {
                        float damage = player.skillAttack ? player.playerSkillAttackPower : player.playerAttackPower;
                        enemyControl.TakeDamage(damage);
                    }
                    else
                    {
                        Debug.LogWarning("EnemyControl component not found on enemy.");
                    }
                }
                else
                {
                    Debug.LogWarning("Player's enemy reference is null.");
                }
            }
        }

        public void EnemyMeleeAttack()
        {
            if (enemy != null && enemy.currentState == EnemyState.Attacking)
            {
                // �÷��̾�� ���ظ� ����
                if (enemy.player != null)
                {
                    BaseCharacterControl playerControl = enemy.player.GetComponent<BaseCharacterControl>();
                    if (playerControl != null)
                    {
                        playerControl.TakeDamage(enemy.enemyAttackPower);
                    }
                    else
                    {
                        Debug.LogWarning("PlayerControl component not found on player.");
                    }
                }
                else
                {
                    Debug.LogWarning("Enemy's player reference is null.");
                }
            }
        }
    }
}