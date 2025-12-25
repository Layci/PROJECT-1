using Project1;
using ProJect1;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
                        float damage = player.skillAttack ? player.SkillAttackPower : player.AttackPower;
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
            // 현재 턴인 유닛 가져오기
            var cur = TurnSystem.instance.allCharacters[TurnSystem.instance.currentTurnIndex] as BaseUnit;
            if (cur == null) return;

            // 스킬 공격인지 확인
            bool isSkill = player.skillAttack;

            // 스킬 공격인지 아닌지에 따라 공격 사거리값 가져오기
            int range = isSkill ? cur.skillAttackRange : cur.normalAttackRange;
            // 공격 방식에 따른 데미지 가져오기
            float damage = isSkill ? cur.SkillAttackPower : cur.AttackPower;
            // 데미지 * 피해 증가량
            damage *= player.damageIncreased;
            // 공격 방식에 따른 이펙트 에셋 가져오기
            EffectAsset effectAsset = isSkill ? cur.skillAttackEffect : cur.normalAttackEffect;

            //var targets = EnemySelection.instance.GetAOETargets(range);
            var targets = cur.GetAttackTargets(range);

            // 이펙트 호출
            EffectManager.Instance.PlayAttackEffect(
                attacker: cur,
                targets: targets,
                isSkill: isSkill,
                range: range,
                damage: damage
            );

            // 데미지 처리
            if (!effectAsset.isProjectile)
            {
                foreach (var enemy in targets)
                {
                    float finalDamage = damage * enemy.damageReduction;
                    enemy.TakeDamage(finalDamage);
                    
                    DamageTextSpawner.Instance?.SpawnDamageText(
                        enemy.transform.position + Vector3.up * 1.5f,
                        (int)finalDamage
                    );
                }
            }
        }
        /*public void OnAttackEvent()
        {
            if (player == null) return;

            // 현재 턴 캐릭터 가져오기
            var cur = TurnSystem.instance.allCharacters[TurnSystem.instance.currentTurnIndex] as BaseUnit;
            if (cur == null) return;

            // 공격 종류별 범위 구분
            int range = player.skillAttack ? cur.skillAttackRange : cur.normalAttackRange;

            var targets = EnemySelection.instance.GetAOETargets(range);

            // 공격 종류별 데미지 구분
            float damage = player.skillAttack ? player.SkillAttackPower : player.AttackPower;
            damage *= player.damageIncreased;

            //totalDamage = 0;

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
        }*/

        public void EnemyMeleeAttack()
        {
            if (enemy == null) return;

            // TurnSystem이 playerCharacters를 BaseCharacterControl 리스트로 가지고 있다고 가정
            var turnSystem = TurnSystem.instance;
            if (turnSystem == null)
            {
                Debug.LogError("[AnimationManager] TurnSystem 없음");
                return;
            }

            if (enemy.playerTransform == null)
            {
                Debug.LogError("[AnimationManager] enemy.playerTransform is null");
                return;
            }

            // playerTransform이 모델 하위(본 등)를 가리키더라도 부모에서 BaseCharacterControl을 찾도록 안전 처리
            var targetControl = enemy.playerTransform.GetComponentInParent<BaseCharacterControl>();
            if (targetControl == null)
            {
                Debug.LogError("[AnimationManager] targetControl not found on playerTransform");
                return;
            }

            // playerCharacters는 List<BaseCharacterControl>
            int targetIndex = turnSystem.playerCharacters.IndexOf(targetControl);
            if (targetIndex == -1)
            {
                Debug.LogError("[AnimationManager] 타겟 인덱스 못 찾음");
                return;
            }

            int range = enemy.skillAttack ? enemy.skillAttackRange : enemy.normalAttackRange;

            // PlayerSelection이 인덱스 기반으로 AOE 목록을 반환하도록 구현되어야 함
            var targets = PlayerSelection.instance.GetAOETargetsByIndex(targetIndex, range);
            Debug.Log($"[AnimationManager] EnemyMeleeAttack targets: {targets.Count}");

            foreach (var playerGO in targets)
            {
                if (playerGO == null) continue;
                var playerControl2 = playerGO.GetComponent<BaseCharacterControl>();
                if (playerControl2 == null) continue;

                float damage = enemy.skillAttack ? enemy.SkillAttackPower : enemy.AttackPower;
                damage *= enemy.damageIncreased * playerControl2.damageReduction;

                playerControl2.TakeDamage(damage);

                if (DamageTextSpawner.Instance != null)
                    DamageTextSpawner.Instance.SpawnDamageText(playerControl2.transform.position + Vector3.up * 1.5f, (int)damage);
            }

            // 공격 끝나면 하이라이트 제거
            if (EnemyAOEHighlighter.Instance != null)
                EnemyAOEHighlighter.Instance.ClearAllHighlights();
        }

        // Taster 캐릭터가 피격 시 버프 파워상승 ((((안씀
        public void TasterTakeDamaged()
        {
            //BuffIconUI.instance.IncreaseBuffPower();
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