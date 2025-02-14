using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ProJect1;

namespace Project1
{
    public enum PlayerState
    {
        Idle,
        MovingToAttack,
        Attacking,
        Blocking,
        Returning
    }

    public abstract class BaseCharacterControl : MonoBehaviour
    {
        public static BaseCharacterControl instance;

        protected Animator animator;
        protected Vector3 initialPosition;
        protected Quaternion initialRotation;

        [Header("ĳ���� ����")]
        public float maxHealth;               // �ִ� ü��
        public float curHealth;               // ���� ü��
        public float moveSpeed;               // �̵� �ӵ�
        public float unitSpeed;               // ���� �ӵ�(�� ���� ����)
        public float playerAttackPower;       // �÷��̾� �⺻���ݷ�
        public float playerSkillAttackPower;  // �÷��̾� ��ų���ݷ�
        public float attackRange;             // ���� �Ÿ�
        public bool startAttacking;           // �������� �˸��� ������
        public bool startBlocking;            // ������� �˸��� ������
        public bool skillAttack;              // ��ų������ ���� �˸��� ������
        public bool isTurn = false;           // ���� ������ �˷��ִ� ������
        public bool isBlock = false;          // ������ ��� �������� �˷��ִ� ������
        public Slider hpBarSlider;            // HP��
        public EnemySelection enemySelection; // ���õ� �� ����
        public Transform currentTarget;       // ���� �̵� ���� ���� Transform
        public string unitName;               // ĳ���� �̸�
        public Sprite unitIcon;               // ĳ���� ������


        [Header("ĳ���� ������")]
        public PlayerState currentState = PlayerState.Idle; // ���� ���� �߰�
        protected bool isAttackExecuted = false;

        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
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
                    // Idle ������ ����
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
            isTurn = false;
            // ���� ĳ���ͷ� ���� �ѱ�
            TurnSystem.instance.EndTurn();
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

                isTurn = false;
                // ���� ĳ���ͷ� ���� �ѱ�
                TurnSystem.instance.EndTurn();
            }
        }


        protected virtual void HandleAttackInput()
        {
            // �� �÷��̾� HandleAttackInput ����
        }


        public void CheckHP()
        {
            if (hpBarSlider != null)
            {
                hpBarSlider.value = curHealth / maxHealth;
            }
        }

        public void CheckBlocking()
        {
            if (!isBlock)
            {
                animator.SetBool("Trigger Block", false);
            }
        }

        public void TakeDamage(float damage)
        {
            if (maxHealth == 0 || curHealth == 0)
                return;

            animator.SetTrigger("Trigger Hit");

            curHealth -= damage;

            CheckHP();

            if (curHealth <= 0)
            {
                Die();
            }
            Debug.Log("hit");
        }

        // �Ʊ� ����� ȣ��
        public void Die()
        {
            Debug.Log("�Ʊ� ���");
            Destroy(gameObject);
            TurnSystem.instance.RemoveCharacterFromTurnOrder(this);
        }
    }
}
