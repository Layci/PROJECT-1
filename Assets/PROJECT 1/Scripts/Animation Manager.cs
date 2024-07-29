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
            // ���� ���� �ִϸ��̼��� �Ϸ�� �� ȣ��Ǵ� �޼���
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