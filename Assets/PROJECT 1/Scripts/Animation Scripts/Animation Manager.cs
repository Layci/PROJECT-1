using Project1;
using ProJect1;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Project1
{
    public class AnimationManager : MonoBehaviour
    {
        public static AnimationManager instance;

        private BaseCharacterControl player;
        private BaseEnemyControl enemy;
        public Transform target;

        public int totalDamage = 0;

        private void Awake()
        {
            player = GetComponentInParent<BaseCharacterControl>();
            enemy = GetComponentInParent<BaseEnemyControl>();

            instance = this;
        }

        private void Update()
        {
            // 플레이어가 참조하는 경우
            if (player != null)
            {
                // 플레이어의 currentTarget을 타겟으로 설정
                target = player.attackAnchorTarget;
            }
            // 적이 참조하는 경우
            else if (enemy != null)
            {
                // 적의 currentTarget을 타겟으로 설정
                target = enemy.attackAnchorTarget;
            }
        }

        // 애니메이션 이벤트에서 호출
        public void OnActionEvent()
        {
            var cur = TurnSystem.instance
        .allCharacters[TurnSystem.instance.currentTurnIndex] as BaseCharacterControl;

            if (cur == null) return;

            bool isSkill = cur.skillAttack;
            bool isHeal = cur.isHealSkill;

            int range = isSkill ? cur.skillAttackRange : cur.normalAttackRange;
            float value = isSkill ? cur.SkillAttackPower : cur.AttackPower;
            value *= cur.damageIncreased;

            EffectAsset effectAsset = isSkill ? cur.skillAttackEffect : cur.normalAttackEffect;

            List<BaseUnit> targets = isHeal
                ? cur.GetHealTargets(range)
                : cur.GetAttackTargets(range);

            // 이펙트
            EffectManager.Instance.PlayAttackEffect(
                attacker: cur,
                targets: targets,
                isSkill: isSkill,
                range: range,
                damage: value
            );

            // 수치 적용
            foreach (var target in targets)
            {
                if (target == null) continue;

                if (isHeal)
                {
                    target.Heal((int)value);
                }
                else
                {
                    float finalDamage = value * target.damageReduction;
                    target.TakeDamage(finalDamage);

                    DamageTextSpawner.Instance?.SpawnDamageText(
                        target.transform.position + Vector3.up * 1.5f,
                        (int)finalDamage
                    );
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
            bool isSkill = cur.skillAttack;

            // 스킬 공격인지 아닌지에 따라 공격 사거리값 가져오기
            int range = isSkill ? cur.skillAttackRange : cur.normalAttackRange;
            // 공격 방식에 따른 데미지 가져오기
            float damage = isSkill ? cur.SkillAttackPower : cur.AttackPower;
            // 데미지 * 피해 증가량
            damage *= cur.damageIncreased;
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

        public void EndHeal()
        {
            player.HealEnd();
            BattleCameraManager.Instance.SwitchToDefault();
        }
    }
}