using Project1;
using UnityEngine;

namespace ProJect1
{
    public class AnimationManager : MonoBehaviour
    {
        public void PlayerMeleeAttack()
        {
            // 플레이어의 공격 애니메이션중 이벤트에서 호출되는 메서드
            BaseCharacterControl player = GetComponentInParent<BaseCharacterControl>();
            if (player != null && player.currentState == PlayerState.Attacking)
            {
                // 적에게 피해를 입힘
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
            // 적의 공격 애니메이션중 이벤트에서 호출되는 메서드
            BaseEnemyControl enemy = GetComponentInParent<BaseEnemyControl>();
            if (enemy != null && enemy.currentState == EnemyState.Attacking)
            {
                // 플레이어에게 피해를 입힘
                BaseCharacterControl player = enemy.player.GetComponent<BaseCharacterControl>();
                if (player != null)
                {
                    player.TakeDamage(enemy.enemyAttackPower);
                }
            }
        }
    }
}