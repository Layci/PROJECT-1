using Project1;
using ProJect1;
using System;
using UnityEngine;

namespace Project1
{
    public class FayePlayerControl : BaseCharacterControl
    {
        public static new FayePlayerControl instance;
        public BuffTurnUI buffTurnUI;
        public BuffIconUI buffIconUI;

        protected override void Awake()
        {
            base.Awake();
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            instance = this;  // 인스턴스 설정
        }

        protected override void Update()
        {
            base.Update();

            Buff FayeAttackBuff = null;
            switch (buffIconUI.buffPower)
            {
                case 1:
                    FayeAttackBuff = new Buff("Faye공격력 증가", 2, 0.2f, 0, typeof(FayePlayerControl));
                    break;
                case 2:
                    FayeAttackBuff = new Buff("Faye공격력 증가", 2, 0.4f, 0, typeof(FayePlayerControl));
                    break;
                case 3:
                    FayeAttackBuff = new Buff("Faye공격력 증가", 2, 0.6f, 0, typeof(FayePlayerControl));
                    break;
            }

            if (FayeAttackBuff != null)
            {
                AddBuff(FayeAttackBuff);
                buffTurnUI.UpdateBuffTurn(buffTrun);
            }
        }

        private void Start()
        {
            buffTurnUI = GetComponent<BuffTurnUI>();
            buffIconUI = GetComponent<BuffIconUI>();
        }

        protected override void HandleAttackInput()
        {
            base.HandleAttackInput();
        }

        /*protected override void HandleAttackInput()
        {
            if (currentState == PlayerState.Idle)
            {
                

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    skillAttack = false;
                    StartMove();
                    SkillPointManager.instance.SkillPointUp();
                }
                else if (SkillPointManager.instance.curSkillPoint > 0 && Input.GetKeyDown(KeyCode.E))
                {
                    skillAttack = true;
                    StartMove();
                    SkillPointManager.instance.UseSkillPoint();
                }
            }
        }*/

        /*private void StartMove()
        {
            currentState = PlayerState.MovingToAttack;
        }*/
    }
}
