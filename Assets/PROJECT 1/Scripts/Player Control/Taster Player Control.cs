using Project1;
using ProJect1;
using System;
using UnityEngine;

namespace Project1
{
    public class TasterPlayerControl : BaseCharacterControl
    {
        public static new TasterPlayerControl instance;

        public ParticleSystem healEffect;

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

        }

        protected override void HandleAttackInput()
        {
            if (currentState == PlayerState.Idle)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    skillAttack = false;
                    StartMove();
                    SkillPointManager.instance.SkillPointUp();
                    if (buffIconUI.buffPower >= 3)
                    {
                        buffIconUI.buffPower = 0;
                        buffIconUI.UpdateBuffUI();
                        healEffect.Play();
                    }
                }
                /*else if (SkillPointManager.instance.curSkillPoint > 0 && Input.GetKeyDown(KeyCode.E))
                {
                    StartBlock();
                    SkillPointManager.instance.UseSkillPoint();
                    if (buffIconUI.buffPower >= 3)
                    {
                        buffIconUI.buffPower = 0;
                        buffIconUI.UpdateBuffUI();
                        healEffect.Play();
                    }
                }*/
                else if (SkillPointManager.instance.curSkillPoint > 0 && Input.GetKeyDown(KeyCode.E))
                {
                    StartBlock();
                    SkillPointManager.instance.UseSkillPoint();

                    Debug.Log("E키 눌림");
                    Debug.Log("버프 파워 현재 값: " + buffIconUI.buffPower);
                }
            }
        }

        private void StartMove()    
        {
            currentState = PlayerState.MovingToAttack;
        }

        private void StartBlock()
        {
            isBlock = true;
            startBlocking = true;
            currentState = PlayerState.Blocking;
            Buff defance = new Buff("방어력증가 + 도발", 1, 0, 0.3f, typeof(TasterPlayerControl));
            AddBuff(defance);
        }
    }
}
