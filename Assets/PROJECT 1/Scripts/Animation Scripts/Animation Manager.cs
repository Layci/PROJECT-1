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
            // ÷̾ ϴ 
            if (player != null)
            {
                // ÷̾ currentTarget Ÿ 
                target = player.attackAnchorTarget;
            }
            //  ϴ 
            else if (enemy != null)
            {
                //  currentTarget Ÿ 
                target = enemy.attackAnchorTarget;
            }
        }

        // ִϸ̼ ̺Ʈ ȣ
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

            // Ʈ
            EffectManager.Instance.PlayAttackEffect(
                attacker: cur,
                targets: targets,
                isSkill: isSkill,
                range: range,
                damage: value
            );

            // ġ 
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

                    // 적중 훅 호출 (디버프 등 처리용)
                    cur.OnHitTarget(target, isSkill);

                    DamageTextSpawner.Instance?.SpawnDamageText(
                        target.transform.position + Vector3.up * 1.5f,
                        (int)finalDamage
                    );
                }
            }
        }

        // ִϸ̼ ̺Ʈ ȣ
        public void OnAttackEvent()
        {
            //    
            var cur = TurnSystem.instance.allCharacters[TurnSystem.instance.currentTurnIndex];
            if (cur == null) return;

            // ų  Ȯ
            bool isSkill = cur.skillAttack;

            // ų  ƴ   Ÿ 
            int range = isSkill ? cur.skillAttackRange : cur.normalAttackRange;
            //  Ŀ   
            float damage = isSkill ? cur.SkillAttackPower : cur.AttackPower;
            //  *  
            damage *= cur.damageIncreased;
            //  Ŀ  Ʈ  
            EffectAsset effectAsset = isSkill ? cur.skillAttackEffect : cur.normalAttackEffect;

            //var targets = EnemySelection.instance.GetAOETargets(range);
            var targets = cur.GetAttackTargets(range);

            // Ʈ ȣ
            EffectManager.Instance.PlayAttackEffect(
                attacker: cur,
                targets: targets,
                isSkill: isSkill,
                range: range,
                damage: damage
            );

            //  ó
            if (!effectAsset.isProjectile)
            {
                foreach (var enemy in targets)
                {
                    float finalDamage = damage * enemy.damageReduction;
                    enemy.TakeDamage(finalDamage);
                    
                    // 적중 훅 호출 (디버프 등 처리용)
                    cur.OnHitTarget(enemy, isSkill);

                    DamageTextSpawner.Instance?.SpawnDamageText(
                        enemy.transform.position + Vector3.up * 1.5f,
                        (int)finalDamage
                    );
                }
            }
        }

        // Taster ĳͰ ǰ   Ŀ ((((Ⱦ
        public void TasterTakeDamaged()
        {
            //BuffIconUI.instance.IncreaseBuffPower();
        }

        public void EndAttack()
        {
            //  ط ʱȭ
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
