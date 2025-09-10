using Project1;
using ProJect1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

namespace Project1
{
    public enum EnemyState
    {
        Idle,
        MovingToAttack,
        Attacking,
        Returning
    }

    public abstract class BaseEnemyControl : BaseUnit
    {
        public static BaseEnemyControl instance;
        public EnemyData enemyData;

        //protected Animator animator;
        public Vector3 initialPosition;
        protected Quaternion initialRotation;

        [Header("�� ����")]
        //public float maxHealth;                  // �ִ� ü��
        //public float curHealth;                  // ���� ü��
        //public float moveSpeed;                  // �̵� �ӵ�
        //public float unitSpeed;                  // ���� �ӵ�(�� ���� ����)
        public float enemyAttackPower;           // �� �⺻ ���ݷ�
        public float enemySkillAttackPower;      // �÷��̾� ��ų���ݷ�
        //public float attackRange;                // ���� �Ÿ�
        public float enemySkillPoint;            // �� ���� ��ų ����Ʈ
        //public float damageReduction = 1f;  // �� ���� ����
        //public float damageIncreased = 1;     // ���� ����
        public bool startAttacking;              // �������� �˸��� ������
        public bool skillAttack;                 // ��ų������ ���� �˸��� ������
        public bool isTurn = false;              // ���� ������ �˷��ִ� ������
        public Transform playerTransform;        // �÷��̾� ����
        public Slider hpBarSlider;               // HP��
        //public string unitName;                  // ĳ���� �̸�
        //public Sprite unitIcon;                  // ĳ���� ������

        [Header("�� ������")]
        public EnemyState currentState = EnemyState.Idle;
        protected bool isAttackExecuted = false;
        private TurnSystem turnSystem;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponentInChildren<Animator>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        protected virtual void Start()
        {
            curHealth = maxHealth;
            turnSystem = FindObjectOfType<TurnSystem>();
            //ApplyEnemyData();
        }

        protected virtual void Update()
        {
            if (isTurn)
            {
                HandleState();
                playerTransform = TurnSystem.instance.playerTargetPosition;
                if (currentState == EnemyState.Idle)
                {
                    if (isTurn)
                    {
                        initialRotation = transform.rotation;
                        StartMove();
                    }
                }
            }
        }

        protected void HandleState()
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    break;
                case EnemyState.MovingToAttack:
                    MoveToAttack();
                    break;
                case EnemyState.Attacking:
                    if (playerTransform != null)
                    {
                        PerformAttack(playerTransform.gameObject);
                    }
                    else
                    {
                        Debug.LogWarning("playerTransform�� �ı��Ǿ� ������ ������ �� �����ϴ�.");
                        currentState = EnemyState.Returning;
                    }
                    break;
                case EnemyState.Returning:
                    ReturnToInitialPosition();
                    break;
            }
        }

        // ������ �����ϵ��� ȣ��Ǵ� �޼���
        public void StartAttack()
        {
            currentState = EnemyState.MovingToAttack;
        }

        private void StartMove()
        {
            if (playerTransform != null)
            {
                currentState = EnemyState.MovingToAttack;
            }
        }

        protected virtual void MoveToAttack()
        {
            // �÷��̾ ���� �����̱�
            if (playerTransform != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
                animator.SetFloat("Speed", 1);

                float distanceToTarget = Vector3.Distance(transform.position, playerTransform.position);
                if (distanceToTarget <= attackRange)
                {
                    currentState = EnemyState.Attacking;
                }
            }
        }

        protected virtual void PerformAttack(GameObject target)
        {
            if (!isAttackExecuted && !skillAttack)
            {
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Trigger EnemyAttack");
                isAttackExecuted = true;

                // ���� ����� TasterPlayerControl ������Ʈ�� ������ �ִ��� Ȯ��
                TasterPlayerControl taster = target.GetComponent<TasterPlayerControl>();
                if (taster != null)
                {
                    // �ش� ����� BuffIconUI ������Ʈ�� �����ͼ� ���� �Ŀ� ����
                    BuffIconUI buffIcon = taster.GetComponent<BuffIconUI>();
                    if (buffIcon != null)
                    {
                        buffIcon.IncreaseBuffPower();
                        Debug.Log("buffPower ����: " + buffIcon.buffPower);
                    }
                    else
                    {
                        Debug.LogWarning("BuffIconUI ������Ʈ�� ã�� �� �����ϴ�.");
                    }
                }
            }
            else if (!isAttackExecuted && skillAttack)
            {
                // ��ų ���� ����
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Trigger EnemySkillAttack");
                isAttackExecuted = true;

                // ���� ����� TasterPlayerControl ������Ʈ�� ������ �ִ��� Ȯ��
                TasterPlayerControl taster = target.GetComponent<TasterPlayerControl>();
                if (taster != null)
                {
                    // �ش� ����� BuffIconUI ������Ʈ�� �����ͼ� ���� �Ŀ� ����
                    BuffIconUI buffIcon = taster.GetComponent<BuffIconUI>();
                    if (buffIcon != null)
                    {
                        buffIcon.IncreaseBuffPower();
                        Debug.Log("buffPower ����: " + buffIcon.buffPower);
                    }
                    else
                    {
                        Debug.LogWarning("BuffIconUI ������Ʈ�� ã�� �� �����ϴ�.");
                    }
                }

                enemySkillPoint -= 2;
            }
        }

        protected virtual void ReturnToInitialPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);  // ĳ���Ͱ� ���� ������ �ٶ󺸵��� ȸ��
            animator.SetFloat("Speed", 1);

            if (Vector3.Distance(transform.position, initialPosition) <= 0.1f)
            {
                transform.position = initialPosition;  // ��ġ ����
                transform.rotation = initialRotation;  // ȸ�� ����
                animator.SetFloat("Speed", 0);

                currentState = EnemyState.Idle;
                isAttackExecuted = false;

                isTurn = false;
                skillAttack = false;
                TurnSystem.instance.EndTurn();
            }
        }

        public void ApplyEnemyData()
        {
            if (enemyData != null)
            {
                unitName = enemyData.enemyName;
                maxHealth = enemyData.maxHealth;
                curHealth = enemyData.maxHealth;
                enemyAttackPower = enemyData.attackPower;
                enemySkillAttackPower = enemyData.skillAttackPower;
                unitSpeed = enemyData.unitSpeed;
                attackRange = enemyData.attackRange;
            }
        }

        // TakeDamage �޼��� �߰�
        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            /*if (curHealth <= 0)
                return;

            animator.SetTrigger("Trigger EnemyHit");
            curHealth -= damage;*/

            if (hpBarSlider != null)
            {
                hpBarSlider.value = curHealth / maxHealth;
            }

            if (curHealth <= 0)
            {
                Die();
            }
        }

        // ���� �׾��� �� ó��
        public override void Die()
        {
            base.Die();
            //Debug.Log("�� ���");
            Destroy(gameObject);
            TurnSystem.instance.RemoveCharacterFromTurnOrder(this);

            BattleManager.instance.RefreshUnitLists();
            BattleManager.instance.RepositionEnemyUnits();

            // FayePlayerControl �ν��Ͻ��� ã�� ���� �Ŀ� ���� �� UI ������Ʈ
            FayePlayerControl faye = FayePlayerControl.instance;
            if (faye != null)
            {
                BuffIconUI.instance.IncreaseBuffPower();
            }
        }
    }
}