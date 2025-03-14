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

        protected override void Awake()
        {
            base.Awake();  // �θ� Ŭ������ Awake()�� ȣ�� (�ʿ����� ������ �����ص� ��)

            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            instance = this;  // �ν��Ͻ� ����
        }

        private void Start()
        {
            turnSystem = FindObjectOfType<TurnSystem>();
            // �� ĳ���Ϳ��� ����� BuffPowerUI ã�� (ĳ���� ������Ʈ�� �ڽ����� ����)
            buffUI = GetComponentInChildren<BuffUI>();
        }

        protected override void HandleAttackInput()
        {
            if (currentState == PlayerState.Idle)
            {
                Buff FayeAttackBuff = null;
                switch (buffPower)
                {
                    case 1:
                        FayeAttackBuff = new Buff("Faye���ݷ� ����", 1, 0.2f, 0);
                        break;
                    case 2:
                        FayeAttackBuff = new Buff("Faye���ݷ� ����", 3, 0.5f, 0);
                        break;
                    case 3:
                        FayeAttackBuff = new Buff("Faye���ݷ� ����", 3, 0.8f, 0);
                        break;
                }

                if (FayeAttackBuff != null)
                {
                    AddBuff(FayeAttackBuff);
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
            }
        }

        public void IncreaseBuffPower()
        {
            if (buffPower < 3)
                buffPower++;

            UpdateBuffUI();  // ���� �Ŀ� �� ���� �� UI ������Ʈ ȣ��
        }

        public void DecreaseBuffPower()
        {
            if (buffPower > 0)
                buffPower--;

            UpdateBuffUI();  // ���� �Ŀ� �� ���� �� UI ������Ʈ ȣ��
        }
    }
}
