using ProJect1;
using System.Collections;
using UnityEngine;

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

        [Header("적 정보")]
        public float maxHealth;
        public float curHealth;
        public float moveSpeed;
        public float enemyAttackPower;
        public float unitSpeed;
        public float attackRange; // 공격 범위 추가
        public bool startAttacking;
        public Transform player; // 플레이어 참조 추가

        [Header("적 움직임")]
        public EnemyState currentState = EnemyState.Idle; // 현재 상태 추가
        protected bool isAttackExecuted = false;

        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        protected virtual void Start()
        {
            // 적의 체력을 최대 체력으로 초기화
            curHealth = maxHealth;
            Debug.Log($"적의 초기 체력: {curHealth}");
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
                    // 아무것도 하지 않음
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
            if (!startAttacking)
            {
                if (currentState == EnemyState.Attacking)
                {
                    animator.SetFloat("Speed", 0);
                    animator.SetTrigger("Trigger EnemyAttack");
                }
            }
        }

        protected virtual void ReturnToInitialPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            animator.SetFloat("Speed", 1);

            if (transform.position == initialPosition)
            {
                animator.SetFloat("Speed", 0);
                transform.rotation = initialRotation;
                currentState = EnemyState.Idle;
                isAttackExecuted = false; // 위치로 돌아오면 공격 수행 상태 초기화
                startAttacking = false; // 위치로 돌아오면 상태 초기화
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

        public void TakeDamage(float damage)
        {
            if (maxHealth == 0 || curHealth == 0)
                return;

            animator.SetTrigger("Trigger EnemyHit");

            curHealth -= damage;

            if (curHealth <= 0)
            {
                Debug.Log("적 사망");
                Destroy(gameObject);
            }
            Debug.Log("적 피격");
        }

        public IEnumerator ExecuteAction()
        {
            yield return new WaitForSeconds(2f);
        }

        public void StartAttack()
        {
            currentState = EnemyState.MovingToAttack;
        }

        public void StopAttack()
        {
            currentState = EnemyState.Idle;
        }
    }
}