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
        public BuffIconUI buffIconUI;

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

        private void Start()
        {
            buffIconUI = GetComponent<BuffIconUI>();
        }

        protected override void Update()
        {
            base.Update();
            HealBuff();
        }

        protected override void HandleAttackInput()
        {
            base.HandleAttackInput();

            // E �� ��ų ����
            if (Input.GetKeyDown(KeyCode.E) && SkillPointManager.instance.curSkillPoint > 0)
            {
                if (prepareState == AttackPrepareState.Skill)
                {
                    // �̹� �غ� ���� �� Ȯ�� ����
                    StartBlock();
                }
            }
        }

        void HealBuff()
        {
            if (buffIconUI.buffPower >= 3)
            {
                buffIconUI.buffPower = 0;
                buffIconUI.UpdateBuffUI();
                HealManager.instance.PlayHealEffect();
            }
        }

        private void StartBlock()
        {
            isBlock = true;
            startBlocking = true;
            currentState = PlayerState.Blocking;
            Buff defance = new Buff("�������� + ����", 1, 0, 0.3f, typeof(TasterPlayerControl));
            AddBuff(defance);
        }
    }
}
