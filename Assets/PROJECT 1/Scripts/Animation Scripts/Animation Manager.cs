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
                        // 적에게 피해를 입히기전 피해 증가율에 따라 데미지 조정
                        damage *= player.damageIncreased;
                        // 적에게 피해를 입힐때 적 피해 감소율에 따라 데미지 조정
                        damage *= enemyControl.damageReduction;
                        enemyControl.TakeDamage(damage);
                        totalDamage += (int)damage;

                        // 싱글턴을 사용하여 DamageTextSpawner 호출
                        if (DamageTextSpawner.Instance != null)
                        {
                            DamageTextSpawner.Instance.SpawnDamageText(target.position + Vector3.up * 1.5f, (int)damage);
                        }

                        TotalDamageUI.Instance.ShowTotalDamage(totalDamage);
                    }
                }
            }
        }

        // 애니메이션 이벤트에서 호출
        public void OnAttackEvent()
        {
            if (player == null) return;

            // 현재 턴 캐릭터 가져오기
            var cur = TurnSystem.instance.allCharacters[TurnSystem.instance.currentTurnIndex] as BaseUnit;
            if (cur == null) return;

            // 공격 종류별 범위 구분
            int range = player.skillAttack ? cur.skillAttackRange : cur.normalAttackRange;

            var targets = EnemySelection.instance.GetAOETargets(range);

            // 공격 종류별 데미지 구분
            float damage = player.skillAttack ? player.playerSkillAttackPower : player.playerAttackPower;
            damage *= player.damageIncreased;

            totalDamage = 0;

            foreach (var enemyControl in targets)
            {
                if (enemyControl == null) continue;
                // 적의 피해 감소율 적용
                float finalDamage = damage * enemyControl.damageReduction;
                enemyControl.TakeDamage(finalDamage);
                totalDamage += (int)finalDamage;

                if (DamageTextSpawner.Instance != null)
                    DamageTextSpawner.Instance.SpawnDamageText(enemyControl.transform.position + Vector3.up * 1.5f, (int)finalDamage);
            }

            if (TotalDamageUI.Instance != null)
                TotalDamageUI.Instance.ShowTotalDamage(totalDamage);
        }

        // 백업 코드
        /*public void OnDamageEvent()
        {
            if (player == null) return;

            // 현재 턴 캐릭터 가져오기
            var cur = TurnSystem.instance.allCharacters[TurnSystem.instance.currentTurnIndex] as BaseUnit;
            if (cur == null) return;

            int range = cur.skillAttackRange;
            List<BaseEnemyControl> targets;

            if (range > 0)
            {
                // 범위 공격
                targets = EnemySelection.instance.GetAOETargets(range);
            }
            else
            {
                // 단일 공격
                targets = new List<BaseEnemyControl>();
                if (player.currentTarget != null)
                {
                    BaseEnemyControl enemyControl = player.currentTarget.GetComponent<BaseEnemyControl>();
                    if (enemyControl != null) targets.Add(enemyControl);
                }
            }

            // 공격력 계산 (스킬 여부는 player.skillAttack으로 구분)
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

        // 애니메이션 이벤트로 호출되는 범위 공격
        public void OnAOEDamageEvent()
        {
            if (player == null) return;

            // 현재 턴 캐릭터 가져오기
            var cur = TurnSystem.instance.allCharacters[TurnSystem.instance.currentTurnIndex] as BaseUnit;
            if (cur == null) return;
            // 범위 공격인지 판단 (예: 준비 플래그에 따라)
            int range = cur.skillAttackRange;
            var targets = EnemySelection.instance.GetAOETargets(range);

            float damage = player.skillAttack ? player.playerSkillAttackPower : player.playerAttackPower;
            damage *= player.damageIncreased;

            totalDamage = 0;

            foreach (var enemyControl in targets)
            {
                if (enemyControl == null) continue;

                // 적의 피해 감소율 적용
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
                // 플레이어에게 피해를 입힘
                if (enemy.playerTransform != null)
                {
                    BaseCharacterControl playerControl = enemy.playerTransform.GetComponent<BaseCharacterControl>();
                    if (playerControl != null)
                    {
                        float damage = enemy.skillAttack ? enemy.enemySkillAttackPower : enemy.enemyAttackPower;
                        // 아군에게 피해를 입히기전 피해 증가율에 따라 데미지 조정
                        damage *= enemy.damageIncreased;
                        // 아군에게 피해를 입힐때 아군 피해 감소율에 따라 데미지 조정
                        damage *= playerControl.damageReduction;
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

        // Taster 캐릭터가 피격 시 버프 파워상승
        public void TasterTakeDamaged()
        {
            BuffIconUI.instance.IncreaseBuffPower();
        }

        public void EndAttack()
        {
            // 총 피해량 초기화
            totalDamage = 0;
        }

        public void EndBlock()
        {
            player.BlockEnd();
        }
    }
}