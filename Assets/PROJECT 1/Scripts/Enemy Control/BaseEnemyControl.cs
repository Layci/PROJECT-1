using ProJect1;
using System.Collections;
using Unity.VisualScripting;
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

    public abstract class BaseEnemyControl : MonoBehaviour, IUnit
    {
        public static BaseEnemyControl instance;

        protected Animator animator;
        protected Vector3 initialPosition;
        protected Quaternion initialRotation;

        [Header("�� ����")]
        public float maxHealth;
        public float curHealth;
        public float moveSpeed;
        public float unitSpeed;
        public float enemyAttackPower;
        public float attackRange; // ���� �Ÿ�
        public bool startAttacking;
        public bool skillAttack;
        public Transform player; // �÷��̾� ����
        public Slider hpBarSlider; // HP��

        [Header("�� ������")]
        public EnemyState currentState = EnemyState.Idle; // ���� ���� �߰�
        protected bool isAttackExecuted = false;

        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        protected virtual void Start()
        {
            // ���� ü���� �ִ� ü������ �ʱ�ȭ
            curHealth = maxHealth;
            Debug.Log($"���� �ʱ� ü��: {curHealth}");
        }

        protected virtual void Update()
        {
            HandleState();
            CheckPlayerDistance();
        }

        protected void HandleState()
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    // �ƹ��͵� ���� ����
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

        protected virtual void MoveToAttack()
        {
            if (!isAttackExecuted)
            {
                transform.LookAt(player.position);
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                animator.SetFloat("Speed", 1);
            }
            else
            {
                currentState = EnemyState.Returning;
            }
        }

        protected virtual void PerformAttack()
        {
            if (!startAttacking && !skillAttack)
            {
                if (currentState == EnemyState.Attacking)
                {
                    animator.SetFloat("Speed", 0);
                    animator.SetTrigger("Trigger EnemyAttack");
                }
            }
            else if (!startAttacking && skillAttack)
            {
                if (currentState == EnemyState.Attacking)
                {
                    animator.SetFloat("Speed", 0);
                    animator.SetTrigger("Trigger EnemySkillAttack");
                }
            }
        }

        protected virtual void ReturnToInitialPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            animator.SetFloat("Speed", 1);
            skillAttack = false;

            if (transform.position == initialPosition)
            {
                animator.SetFloat("Speed", 0);
                transform.rotation = initialRotation;
                currentState = EnemyState.Idle;
                isAttackExecuted = false; // ��ġ�� ���ƿ��� ���� ���� ���� �ʱ�ȭ
                startAttacking = false; // ��ġ�� ���ƿ��� ���� �ʱ�ȭ
            }
        }

        protected void CheckPlayerDistance()
        {
            if (currentState == EnemyState.MovingToAttack)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                if (distanceToPlayer <= attackRange)
                {
                    currentState = EnemyState.Attacking;
                }
            }
        }

        public void CheckHP()
        {
            if (hpBarSlider != null)
            {
                hpBarSlider.value = curHealth / maxHealth;
            }
        }

        public void TakeDamage(float damage)
        {
            if (maxHealth == 0 || curHealth == 0)
                return;

            animator.SetTrigger("Trigger EnemyHit");

            curHealth -= damage;

            CheckHP();

            if (curHealth <= 0)
            {
                Debug.Log("�� ���");
                Destroy(gameObject);
            }
            Debug.Log("�� �ǰ�");
        }

        public void StartAttack()
        {
            currentState = EnemyState.MovingToAttack;
        }

        public void StopAttack()
        {
            currentState = EnemyState.Idle;
        }

        public float UnitSpeed => unitSpeed;

        public void TakeTurn()
        {
            // ������ �ൿ�� ����
            StartCoroutine(ExecuteTurn());
        }

        private IEnumerator ExecuteTurn()
        {
            isAttackExecuted = false;
            currentState = EnemyState.MovingToAttack;

            // ���� ó�� ����
            while (currentState != EnemyState.Idle)
            {
                HandleState();
                yield return null; // ���� �����ӱ��� ���
            }

            // ������ �ൿ�� �Ϸ�Ǹ� GameManager�� �˸�
            GameManager.instance.OnUnitTurnCompleted();
        }
    }
}