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
        private BuffUI buffUI;
        public int buffTurn; // 남은 버프 턴 확인용

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

        private void Start()
        {
            turnSystem = FindObjectOfType<TurnSystem>();
            // 이 캐릭터에게 연결된 BuffPowerUI 찾기 (캐릭터 오브젝트의 자식으로 설정)
            buffUI = GetComponent<BuffUI>();
        }

        protected override void HandleAttackInput()
        {
            if (currentState == PlayerState.Idle)
            {
                Buff FayeAttackBuff = null;
                switch (buffPower)
                {
                    case 1:
                        FayeAttackBuff = new Buff("Faye공격력 증가", 3, 0.2f, 0, typeof(FayePlayerControl));
                        break;
                    case 2:
                        FayeAttackBuff = new Buff("Faye공격력 증가", 3, 0.4f, 0, typeof(FayePlayerControl));
                        break;
                    case 3:
                        FayeAttackBuff = new Buff("Faye공격력 증가", 2, 0.6f, 0, typeof(FayePlayerControl));
                        break;
                }

                if (FayeAttackBuff != null)
                {
                    AddBuff(FayeAttackBuff);
                    buffUI.UpdateBuffTurn(FayeAttackBuff.remainingTurns);
                }

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

        void UpdateBuffUI()
        {
            if (buffUI != null)
            {
                buffUI.UpdateBuffUI(buffPower);
                Debug.Log("버프UI 활성화");
            }
        }

        public void IncreaseBuffPower()
        {
            if (buffPower < 3)
                buffPower++;

            UpdateBuffUI();  // 버프 파워 값 변경 시 UI 업데이트 호출
        }

        public void DecreaseBuffPower()
        {
            if (buffPower > 0)
                buffPower = 0;

            UpdateBuffUI();  // 버프 파워 값 변경 시 UI 업데이트 호출
        }
    }
}
