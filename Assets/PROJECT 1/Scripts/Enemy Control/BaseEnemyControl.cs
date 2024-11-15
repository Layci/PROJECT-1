using Project1;
using System.Collections;
using UnityEngine;
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

    public abstract class BaseEnemyControl : MonoBehaviour
    {
        public static BaseEnemyControl instance;

        protected Animator animator;
        protected Vector3 initialPosition;
        protected Quaternion initialRotation;

        [Header("�� ����")]
        public float maxHealth;              // �ִ� ü��
        public float curHealth;              // ���� ü��
        public float moveSpeed;              // �̵� �ӵ�
        public float unitSpeed;              // ���� �ӵ�(�� ���� ����)
        public float enemyAttackPower;       // �� �⺻ ���ݷ�
        public float enemySkillAttackPower;  // �÷��̾� ��ų���ݷ�
        public float attackRange;            // ���� �Ÿ�
        public bool startAttacking;          // �������� �˸��� ������
        public bool skillAttack;             // ��ų������ ���� �˸��� ������
        public bool isTurn = false;          // ���� ������ �˷��ִ� ������
        public bool isSkillTurn = false;
        public Transform playerTransform1;    // �÷��̾� ����
        public Slider hpBarSlider;           // HP��

        [Header("�� ������")]
        public EnemyState currentState = EnemyState.Idle;
        protected bool isAttackExecuted = false;
        private TurnSystem turnSystem;

        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        protected virtual void Start()
        {
            curHealth = maxHealth;
            turnSystem = FindObjectOfType<TurnSystem>();
        }

        protected virtual void Update()
        {
            if (isTurn || isSkillTurn)
            {
                HandleState();

                if (currentState == EnemyState.Idle)
                {
                    if (isTurn)
                    {
                        skillAttack = false;
                        StartMove();
                        //EndPlayerTurn(); // ���� �� �� ����
                    }
                    if (isSkillTurn)
                    {
                        skillAttack = true;
                        StartMove();
                        //EndPlayerTurn(); // ��ų ��� �� �� ����
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
                    PerformAttack();
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
            if (playerTransform1 != null)
            {
                currentState = EnemyState.MovingToAttack;
            }
        }

        /*private void EndPlayerTurn()
        {
            if (turnSystem != null)
            {
                turnSystem.EndTurn(); // �� ����
            }
        }*/

        protected virtual void MoveToAttack()
        {
            // �÷��̾ ���� �����̱�
            if (playerTransform1 != null)
            {
                //transform.LookAt(playerTransform1.position);
                transform.position = Vector3.MoveTowards(transform.position, playerTransform1.position, moveSpeed * Time.deltaTime);
                animator.SetFloat("Speed", 1);

                float distanceToTarget = Vector3.Distance(transform.position, playerTransform1.position);
                if (distanceToTarget <= attackRange)
                {
                    currentState = EnemyState.Attacking;
                }
            }
        }

        protected virtual void PerformAttack()
        {
            if (!isAttackExecuted && !skillAttack)
            {
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Trigger EnemyAttack");
                isAttackExecuted = true; // ������ ������ ������ ǥ��
            }
            else if (!isAttackExecuted && skillAttack)
            {
                // ��ų ���� ����
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Trigger EnemySkillAttack");
                isAttackExecuted = true;
            }

            // ������ ���� �� ���¸� �ǵ��ư��� ���·� ����
            //currentState = EnemyState.Returning;
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
                isSkillTurn = false;
                TurnSystem.instance.EndTurn();
            }
        }

        // TakeDamage �޼��� �߰�
        public void TakeDamage(float damage)
        {
            if (curHealth <= 0)
                return;

            animator.SetTrigger("Trigger EnemyHit");
            curHealth -= damage;

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
        protected virtual void Die()
        {
            Debug.Log("�� ���");
            //Destroy(gameObject);
        }
    }
}