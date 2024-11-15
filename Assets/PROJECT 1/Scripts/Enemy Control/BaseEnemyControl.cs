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

        [Header("적 정보")]
        public float maxHealth;
        public float curHealth;
        public float moveSpeed;
        public float enemyAttackPower;
        public float attackRange; // 공격 거리
        public bool startAttacking;
        public bool skillAttack;
        public Transform player; // 플레이어 참조
        public Slider hpBarSlider; // HP바

        [Header("적 움직임")]
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

        // TakeDamage 메서드 추가
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

        // 적이 죽었을 때 처리
        protected virtual void Die()
        {
            Debug.Log("적 사망");
            Destroy(gameObject);
        }

        // 공격을 시작하도록 호출되는 메서드
        public void StartAttack()
        {
            currentState = EnemyState.MovingToAttack;
        }

        protected virtual void MoveToAttack()
        {
            // 플레이어를 향해 움직이기
            if (!isAttackExecuted)
            {
                transform.LookAt(player.position);
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                animator.SetFloat("Speed", 1);

                // 공격 범위에 도달하면 상태를 변경
                if (Vector3.Distance(transform.position, player.position) <= attackRange)
                {
                    currentState = EnemyState.Attacking;
                }
            }
            else
            {
                // 공격을 이미 수행한 경우 되돌아가는 상태로 변경
                currentState = EnemyState.Returning;
            }
        }

        protected virtual void PerformAttack()
        {
            if (currentState == EnemyState.Attacking && !isAttackExecuted)
            {
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Trigger EnemyAttack");
                isAttackExecuted = true; // 공격을 수행한 것으로 표시
            }

            // 공격이 끝난 후 상태를 되돌아가는 상태로 설정
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
                isAttackExecuted = false; // 공격 수행 상태 초기화

                // 턴 종료 시 호출
                turnSystem?.EndTurn();
            }
        }
    }
}