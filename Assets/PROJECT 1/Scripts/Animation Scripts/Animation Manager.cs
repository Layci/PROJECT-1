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
            // 플레이어가 참조하는 경우
            if (player != null)
            {
                // 플레이어의 currentTarget을 타겟으로 설정
                target = player.currentTarget;
            }
            // 적이 참조하는 경우
            else if (enemy != null)
            {
                // 적의 currentTarget을 타겟으로 설정
                target = enemy.playerTransform;
            }
        }

        public void PlayerMeleeAttack()
        {
            if (player != null && player.currentState == PlayerState.Attacking)
            {
                // 적에게 피해를 입힘
                if (player.currentTarget != null)
                {
                    BaseEnemyControl enemyControl = player.currentTarget.GetComponent<BaseEnemyControl>();
                    if (enemyControl != null)
                    {
                        float damage = player.skillAttack ? player.playerSkillAttackPower : player.playerAttackPower;
                        enemyControl.TakeDamage(damage);

                        // 싱글턴을 사용하여 DamageTextSpawner 호출
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
                // 플레이어에게 피해를 입힘
                if (enemy.playerTransform != null)
                {
                    BaseCharacterControl playerControl = enemy.playerTransform.GetComponent<BaseCharacterControl>();
                    if (playerControl != null)
                    {
                        float damage = enemy.skillAttack ? enemy.enemySkillAttackPower : enemy.enemyAttackPower;
                        playerControl.TakeDamage(damage);

                        // 싱글턴을 사용하여 DamageTextSpawner 호출
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