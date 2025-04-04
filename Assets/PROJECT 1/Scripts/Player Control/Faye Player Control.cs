using Project1;
using ProJect1;
using System;
using UnityEngine;

namespace Project1
{
    public class FayePlayerControl : BaseCharacterControl
    {
        public static new FayePlayerControl instance;
        private TurnSystem turnSystem;
        public BuffTurnUI buffTurnUI;
        public BuffIconUI buffIconUI;

        protected override void Awake()
        {
            base.Awake();  // 부모 클래스의 Awake()도 호출 (필요하지 않으면 삭제해도 됨)

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
                    FayeAttackBuff = new Buff("Faye공격력 증가", 1, 0.2f, 0, typeof(FayePlayerControl));
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
            turnSystem = FindObjectOfType<TurnSystem>();
            buffTurnUI = GetComponent<BuffTurnUI>();
            buffIconUI = GetComponent<BuffIconUI>();
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
                }
                else if (SkillPointManager.instance.curSkillPoint > 0 && Input.GetKeyDown(KeyCode.E))
                {
                    skillAttack = true;
                    StartMove();
                    SkillPointManager.instance.UseSkillPoint();
                }
            }
        }

        private void StartMove()
        {
            currentState = PlayerState.MovingToAttack;
        }

        /*public void UpdateBuffUI()
        {
            if (buffIconUI != null)
            {
                // buffPower 값에 따라 아이콘 활성화 업데이트
                buffIconUI.UpdateBuffUI(buffPower);
                Debug.Log("버프 UI 활성화");
            }
        }*/

        /*public void IncreaseBuffPower()
        {
            if (buffPower < 3)
                buffPower++;

            UpdateBuffUI();  // 버프 파워 값 변경 시 UI 업데이트 호출
        }*/

        /*public void DecreaseBuffPower()
        {
            UpdateBuffUI();  // 버프 파워 값 변경 시 UI 업데이트 호출
        }*/
    }
}
