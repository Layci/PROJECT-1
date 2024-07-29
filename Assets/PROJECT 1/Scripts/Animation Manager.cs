using Project1;
using UnityEngine;

namespace ProJect1
{
    public class AnimationManager : MonoBehaviour
    {
        public void OnAttackAnimationFinished()
        {
            BaseCharacterControl player = GetComponentInParent<BaseCharacterControl>();
            if (player != null)
            {
                player.StopAction();
            }
        }

        public void OnSkillAnimationFinished()
        {
            BaseCharacterControl player = GetComponentInParent<BaseCharacterControl>();
            if (player != null)
            {
                player.StopAction();
            }
        }

        public void EnemyMeleeAttack()
        {
            // 적의 공격 애니메이션이 완료될 때 호출되는 메서드
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