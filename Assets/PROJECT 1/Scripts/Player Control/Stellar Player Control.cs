using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class StellarPlayerControl : BaseCharacterControl
    {
        // 공격 적중 시 실행되는 메소드 오버라이드
        public override void OnHitTarget(BaseUnit target, bool isSkill)
        {
            base.OnHitTarget(target, isSkill);

            // 30% 확률로 공격력 감소 디버프 (2턴 지속, 공격력 -20%)
            Buff atkDebuff = new Buff("공격력 감소", 2, -0.2f,   0, EffectType.Debuff);

            TryApplyBuffOnHit(atkDebuff, target, isSkill, AttackTriggerType.All, 0.3f);
        }

        protected override void HandleAttackInput()
        {
            base.HandleAttackInput();
        }
    }
}
