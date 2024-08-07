using Project1;
using UnityEngine;

namespace ProJect1
{
    public class AnimationManager : MonoBehaviour
    {
        public void PlayerMeleeAttack()
        {
            // �÷��̾��� ���� �ִϸ��̼��� �̺�Ʈ���� ȣ��Ǵ� �޼���
            BaseCharacterControl player = GetComponentInParent<BaseCharacterControl>();
            if (player != null && player.currentState == PlayerState.Attacking)
            {
                // ������ ���ظ� ����
                BaseEnemyControl enemy = player.enemy.GetComponent<BaseEnemyControl>();
                if (enemy != null && !player.skillAttack)
                {
                    enemy.TakeDamage(player.playerAttackPower);
                }
                else
                {
                    enemy.TakeDamage(player.playerSKillAttackPower);
                }
            }
        }

        public void EnemyMeleeAttack()
        {
            // ���� ���� �ִϸ��̼��� �̺�Ʈ���� ȣ��Ǵ� �޼���
            BaseEnemyControl enemy = GetComponentInParent<BaseEnemyControl>();
            if (enemy != null && enemy.currentState == EnemyState.Attacking)
            {
                // �÷��̾�� ���ظ� ����
                BaseCharacterControl player = enemy.player.GetComponent<BaseCharacterControl>();
                if (player != null)
                {
                    player.TakeDamage(enemy.enemyAttackPower);
                }
            }
        }
    }
}