using Project1;
using UnityEngine;

namespace Project1
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
                }
            }
        }
    }
}