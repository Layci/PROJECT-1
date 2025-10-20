using UnityEngine;
using System.Collections;
using Project1;
using System.Collections.Generic;
using UnityEngine.UI;
using ProJect1;
using System.Linq;
using Unity.VisualScripting;

namespace Project1
{
    public enum PlayerState
    {
        Idle,
        MovingToAttack,
        Attacking,
        Blocking,
        Returning,
        Buffing
    }

    public enum AttackPrepareState
    {
        None,
        Basic,   // �Ϲݰ���
        Skill,   // ��������
        Buff     // �ڱⰭȭ or ���ù���
    }

    public abstract class BaseCharacterControl : BaseUnit
    {
        public static BaseCharacterControl instance;

        public Vector3 initialPosition;
        protected Quaternion initialRotation;

        [Header("ĳ���� ����")]
        //public float maxHealth;               // �ִ� ü��
        //public float curHealth;               // ���� ü��
        //public float moveSpeed;               // �̵� �ӵ�
        //public float unitSpeed;               // ���� �ӵ�(�� ���� ����)
        public float playerAttackPower;       // �÷��̾� �⺻���ݷ�
        public float playerSkillAttackPower;  // �÷��̾� ��ų���ݷ�
        //public int buffPower = 0;             // �÷��̾� ���� �Ŀ�
        //public float attackRange;             // ���� �Ÿ�
        //public float damageReduction;         // ���� ����
        //public float damageIncreased = 1;     // ���� ����
        public bool startAttacking;           // �������� �˸��� ������
        public bool startBlocking;            // ������� �˸��� ������
        public bool skillAttack;              // ��ų������ ���� �˸��� ������
        public bool isTurn = false;           // ���� ������ �˷��ִ� ������
        public bool isBlock = false;          // ������ ��� �������� �˷��ִ� ������
        public bool isPreparingAOEAttack = false;
        
        public AttackPrepareState prepareState = AttackPrepareState.None;

        public Slider hpBarSlider;            // HP��
        public Text hpText;                   // HP �ؽ�Ʈ
        public EnemySelection enemySelection; // ���õ� �� ����
        public Transform currentTarget;       // ���� �̵� ���� ���� Transform
        //public string unitName;               // ĳ���� �̸�
        //public Sprite unitIcon;               // ĳ���� ������


        [Header("ĳ���� ������")]
        public PlayerState currentState = PlayerState.Idle; // ���� ���� �߰�
        protected bool isAttackExecuted = false;

        //private List<Buff> activeBuffs = new List<Buff>();

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponentInChildren<Animator>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            CheckHP();
        }

        protected virtual void Update()
        {
            // ĳ���Ͱ� �ڽ��� ���� ��쿡 �Է� ó��
            if (isTurn)
            {
                HandleState();
                HandleAttackInput();
                TargetUpdate();
            }
        }

        protected virtual void HandleAttackInput()
        {
            if (!CanAttack())
                return;

            // Q �� �⺻ ����
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("���� prepareState: " + prepareState);

                if (prepareState != AttackPrepareState.Basic)
                {
                    prepareState = AttackPrepareState.Basic;

                    int range = normalAttackRange;
                    var targets = EnemySelection.instance.GetAOETargets(range);
                    EnemySelectorUI.instance.ShowAOETargets(targets.Select(e => e.transform).ToList());
                }
                else
                {
                    ExecuteBasicAttack();
                    SkillPointManager.instance.SkillPointUp();
                }

                /*if (prepareState == AttackPrepareState.Basic)
                {
                    Debug.Log("�⺻���� Ȯ�� ����!");
                    ExecuteBasicAttack();
                    SkillPointManager.instance.SkillPointUp();
                }
                else
                {
                    Debug.Log("�⺻���� �غ� ���� ����");
                    prepareState = AttackPrepareState.Basic;

                    int range = normalAttackRange;

                    if (range == 0)
                    {
                        // ���� ���� ���
                        EnemySelectorUI.instance.ShowSingleTargetUI();
                        EnemySelectorUI.instance.HideAOEUI();
                    }
                    else
                    {
                        // ���� ���� ���
                        var targets = EnemySelection.instance.GetAOETargets(range);
                        EnemySelectorUI.instance.ShowAOETargets(targets.Select(e => e.transform).ToList());
                        EnemySelectorUI.instance.HideSingleTargetUI();
                    }
                }*/
            }

            // E �� ��ų ����
            if (Input.GetKeyDown(KeyCode.E) && SkillPointManager.instance.curSkillPoint > 0)
            {
                if (prepareState == AttackPrepareState.Skill)
                {
                    Debug.Log("���� prepareState2: " + prepareState);
                    // �̹� �غ� ���� �� Ȯ�� ����
                    ExecuteSkillAttack();
                    SkillPointManager.instance.UseSkillPoint();
                }
                else
                {
                    Debug.Log("��ų���� �غ� ���� ����");
                    Debug.Log("���� prepareState2: " + prepareState);
                    Debug.Log(skillAttackRange);
                    // �غ� ���� ����
                    prepareState = AttackPrepareState.Skill;
                    var cur = TurnSystem.instance.allCharacters[TurnSystem.instance.currentTurnIndex] as BaseUnit;
                    int range = cur != null ? cur.skillAttackRange : 0;

                    // �ε��� ��� AOE ��� ��������
                    var targets = EnemySelection.instance.GetAOETargets(range);
                    Debug.Log("AOE ��� ��: " + targets.Count);
                    // UI �ݿ�
                    EnemySelectorUI.instance.ShowAOETargets(targets.Select(e => e.transform).ToList());
                    EnemySelectorUI.instance.HideSingleTargetUI();
                }
            }

            // R �� ����
            /*if (Input.GetKeyDown(KeyCode.R))
            {
                if (prepareState == AttackPrepareState.Buff)
                {
                    // �̹� �غ� ���� �� Ȯ�� ����
                    ExecuteBuff();
                }
                else
                {
                    // �غ� ���� ����
                    prepareState = AttackPrepareState.Buff;
                    EnemySelectorUI.instance.HideSingleTargetUI();
                    EnemySelectorUI.instance.HideAOEUI();
                }
            }*/
        }

        private void ExecuteBasicAttack()
        {
            prepareState = AttackPrepareState.None;
            currentState = PlayerState.MovingToAttack;
            skillAttack = false;
            EnemySelectorUI.instance.HideAOEUI();
        }

        private void ExecuteSkillAttack()
        {
            prepareState = AttackPrepareState.None;
            currentState = PlayerState.MovingToAttack;
            skillAttack = true;
            EnemySelectorUI.instance.HideAOEUI();
        }

        private void ExecuteBuff()
        {
            prepareState = AttackPrepareState.None;
            currentState = PlayerState.Buffing; // �ʿ�� Idle�ε� ����
            ApplySelfBuff();
        }

        private void ApplySelfBuff()
        {
            // ����: ���ݷ� +20%, 2�� ����
            Buff selfBuff = null;
            selfBuff = new Buff("", 2, 0.2f, 0, typeof(This));
            Debug.Log($"{unitName} ���� �ߵ�!");
        }

        protected bool CanAttack()
        {
            // �⺻ ������ �� ���̾�� �ϰ�, �� ���� �� �̵� ���� �ƴϾ�� ��
            if (EnemySelection.instance.isMove)
                return false;

            // Idle�� ���� �غ� ���� ���� ����
            if (currentState == PlayerState.Idle && prepareState == AttackPrepareState.None)
                return true;

            // �̹� �غ� ������ ���� �Է� ��� (Ȯ�� ���� ����)
            if (prepareState != AttackPrepareState.None)
                return true;

            return false;
        }

        public void TargetUpdate()
        {
            Transform targetposition = EnemySelectorUI.instance.selectedEnemy;
            currentTarget = targetposition;
        }

        protected void HandleState()
        {
            switch (currentState)
            {
                case PlayerState.Idle:
                    break;
                case PlayerState.MovingToAttack:
                    MoveToAttack();
                    break;
                case PlayerState.Attacking:
                    PerformAttack();
                    break;
                case PlayerState.Blocking:
                    PerformBlock();
                    break;
                case PlayerState.Returning:
                    ReturnToInitialPosition();
                    break;
            }
        }

        protected virtual void MoveToAttack()
        {
            if (currentTarget != null)
            {
                EnemySelection.instance.isMove = true;
                transform.position = Vector3.MoveTowards(transform.position, EnemySelectorUI.instance.selectedEnemy.position, moveSpeed * Time.deltaTime);
                animator.SetFloat("Speed", 1);

                float distanceToTarget = Vector3.Distance(transform.position, EnemySelectorUI.instance.selectedEnemy.position);

                if (distanceToTarget <= attackRange && !isBlock)
                {
                    currentState = PlayerState.Attacking;
                }
            }
        }

        protected virtual void PerformAttack()
        {
            if (!isAttackExecuted && !skillAttack)
            {
                // ���� ����
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Trigger Attack");
                isAttackExecuted = true;
            }
            else if (!isAttackExecuted && skillAttack)
            {
                // ��ų ���� ����
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Trigger SkillAttack");
                isAttackExecuted = true;
            }
        }

        protected virtual void PerformBlock()
        {
            if (!isAttackExecuted)
            {
                animator.SetFloat("Speed", 0);
                animator.SetBool("Trigger Block", true);
            }
        }

        public void BlockEnd()
        {
            if (isBlock)
            {
                isTurn = false;
                // ���� ĳ���ͷ� ���� �ѱ�
                TurnSystem.instance.EndTurn();
            }
        }

        protected virtual void ReturnToInitialPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, -180f, 0f);  // ĳ���Ͱ� ���� ������ �ٶ󺸵��� ȸ��
            animator.SetFloat("Speed", 1);

            if (Vector3.Distance(transform.position, initialPosition) <= 0.1f)
            {
                transform.position = initialPosition;  // ��ġ ����
                transform.rotation = initialRotation;  // ȸ�� ����
                animator.SetFloat("Speed", 0);

                currentState = PlayerState.Idle;
                isAttackExecuted = false;

                EnemySelection.instance.isMove = false;
                isTurn = false;
                // ���� ĳ���ͷ� ���� �ѱ�
                TurnSystem.instance.EndTurn();
            }
        }

        public void CheckHP()
        {
            if (hpBarSlider != null)
            {
                hpBarSlider.value = curHealth / maxHealth;
                hpText.text = Mathf.RoundToInt(curHealth).ToString(); // �ݿø��ؼ� ������ ǥ��
            }
        }

        public void DoneBlock()
        {
            animator.SetBool("Trigger Block", false);
            isBlock = false;
            startBlocking = false;
            currentState = PlayerState.Idle;
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);

            CheckHP();

            if (curHealth <= 0)
            {
                Die();
            }
            Debug.Log("hit");
        }

        // �Ʊ� ����� ȣ��
        public override void Die()
        {
            base.Die();
            Destroy(gameObject);
            TurnSystem.instance.RemoveCharacterFromTurnOrder(this);            
        }
    }
}
