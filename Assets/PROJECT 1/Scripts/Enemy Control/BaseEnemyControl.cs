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
        public float maxHealth;
        public float curHealth;
        public float moveSpeed;
        public float enemyAttackPower;
        public float attackRange; // ���� �Ÿ�
        public bool startAttacking;
        public bool skillAttack;
        public Transform player; // �÷��̾� ����
        public Slider hpBarSlider; // HP��

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
            HandleState();
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
            Destroy(gameObject);
        }

        // ������ �����ϵ��� ȣ��Ǵ� �޼���
        public void StartAttack()
        {
            currentState = EnemyState.MovingToAttack;
        }

        protected virtual void MoveToAttack()
        {
            // �÷��̾ ���� �����̱�
            if (!isAttackExecuted)
            {
                transform.LookAt(player.position);
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                animator.SetFloat("Speed", 1);

                // ���� ������ �����ϸ� ���¸� ����
                if (Vector3.Distance(transform.position, player.position) <= attackRange)
                {
                    currentState = EnemyState.Attacking;
                }
            }
            else
            {
                // ������ �̹� ������ ��� �ǵ��ư��� ���·� ����
                currentState = EnemyState.Returning;
            }
        }

        protected virtual void PerformAttack()
        {
            if (currentState == EnemyState.Attacking && !isAttackExecuted)
            {
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Trigger EnemyAttack");
                isAttackExecuted = true; // ������ ������ ������ ǥ��
            }

            // ������ ���� �� ���¸� �ǵ��ư��� ���·� ����
            currentState = EnemyState.Returning;
        }

        protected virtual void ReturnToInitialPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            animator.SetFloat("Speed", 1);

            if (transform.position == initialPosition)
            {
                animator.SetFloat("Speed", 0);
                transform.rotation = initialRotation;
                currentState = EnemyState.Idle;
                isAttackExecuted = false; // ���� ���� ���� �ʱ�ȭ

                // �� ���� �� ȣ��
                turnSystem?.EndTurn();
            }
        }
    }
}