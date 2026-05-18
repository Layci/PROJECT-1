using Project1;
using ProJect1;
using System;
using System.Linq;
using UnityEngine;

namespace Project1
{
    public class TasterPlayerControl : BaseCharacterControl
    {
        public static new TasterPlayerControl instance;

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
            base.OnBuffPowerUpdated(currentPower);

            // 버프 파워가 최대치에 도달하면 힐 발동
            if (currentPower >= maxBuffPower)
            {
                ResetBuffPower();
                HealManager.instance.PlayHealEffect();
                Debug.Log("Taster: 버프 파워 풀 충전! 파티 힐 발동!");
            }
        }

        protected override void HandleAttackInput()
        {
            // E 키 스킬 공격
            if (Input.GetKeyDown(KeyCode.E) && SkillPointManager.instance.curSkillPoint > 0)
            {
                if (prepareState == AttackPrepareState.Skill)
                {
                    StartBlock();
                }
            }
            
            if (currentState != PlayerState.Blocking)
            {
                base.HandleAttackInput();
            }
        }

        public void StartBlock()
        {
            isBlock = true;
            startBlocking = true;
            currentState = PlayerState.Blocking;
            // 방어 중 방어력 증가 버프 (DoT/HoT는 없음)
            Buff defense = new Buff("방어태세 + 강화", 1, 0, 0.3f, EffectType.Buff, 0, true, typeof(TasterPlayerControl), false);
            AddBuff(defense);
        }
    }
}
