using Project1;
using System;
using UnityEngine;

namespace Project1
{
    public class FayePlayerControl : BaseCharacterControl
    {
        public static new FayePlayerControl instance;

        protected override void Awake()
        {
            base.Awake();
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            instance = this;  
        }

        public override void OnBuffPowerUpdated(int currentPower)
        {
            // 부모 클래스의 UI 업데이트 호출
            base.OnBuffPowerUpdated(currentPower);

            // 버프 파워가 변경될 때만 새로운 버프 적용
            Buff FayeAttackBuff = null;
            switch (currentPower)
            {
                case 1:
                    // isTickAtStart를 false로 설정하여 턴 종료 시 지속시간이 줄어들게 함
                    FayeAttackBuff = new Buff("Faye공격력 증가", 2, 0.2f, 0, EffectType.Buff, 0, false, typeof(FayePlayerControl), true);
                    break;
                case 2:
                    FayeAttackBuff = new Buff("Faye공격력 증가", 2, 0.4f, 0, EffectType.Buff, 0, false, typeof(FayePlayerControl), true);
                    break;
                case 3:
                    FayeAttackBuff = new Buff("Faye공격력 증가", 2, 0.6f, 0, EffectType.Buff, 0, false, typeof(FayePlayerControl), true);
                    break;
            }

            if (FayeAttackBuff != null)
            {
                AddBuff(FayeAttackBuff);
            }
        }

        protected override void HandleAttackInput()
        {
            base.HandleAttackInput();
        }
    }
}
