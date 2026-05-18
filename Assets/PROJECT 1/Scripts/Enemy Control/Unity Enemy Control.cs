using Project1;
using ProJect1;
using UnityEngine;

namespace Project1
{
    public class UnityEnemyControl : BaseEnemyControl
    {
        // 공격 적중 시 실행되는 메소드 오버라이드
        public override void OnHitTarget(BaseUnit target, bool isSkill)
        {
            base.OnHitTarget(target, isSkill);

            // 20% 확률로 화염 데미지 (2턴 지속, 턴 종료 시 체력 감소)
            // isTickAtStart를 false로 설정하여 턴 종료 시 데미지가 발생하고 지속시간이 줄어들게 함
            Buff fireDebuff = new Buff("화염 데미지", 2, 0, 0, EffectType.Debuff, -20, false);

            TryApplyBuffOnHit(fireDebuff, target, isSkill, AttackTriggerType.All, 0.2f);
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}
