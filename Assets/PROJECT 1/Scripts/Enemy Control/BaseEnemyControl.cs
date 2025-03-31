using Project1;
using ProJect1;
using System.Collections;
using System.Collections.Generic;
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

    public abstract class BaseEnemyControl : BaseUnit
    {
        public static BaseEnemyControl instance;

        protected Animator animator;
        protected Vector3 initialPosition;
        protected Quaternion initialRotation;

        [Header("적 정보")]
        //public float maxHealth;                  // 최대 체력
        //public float curHealth;                  // 현재 체력
        //public float moveSpeed;                  // 이동 속도
        //public float unitSpeed;                  // 유닛 속도(턴 순서 관련)
        public float enemyAttackPower;           // 적 기본 공격력
        public float enemySkillAttackPower;      // 플레이어 스킬공격력
        //public float attackRange;                // 공격 거리
        public float enemySkillPoint;            // 적 공격 스킬 포인트
        //public float damageReduction = 1f;  // 적 피해 감소
        //public float damageIncreased = 1;     // 피해 증가
        public bool startAttacking;              // 공격중을 알리는 연산자
        public bool skillAttack;                 // 스킬공격을 할지 알리는 연산자
        public bool isTurn = false;              // 본인 턴인지 알려주는 연산자
        public Transform playerTransform;        // 플레이어 참조
        public Slider hpBarSlider;               // HP바
        public string unitName;                  // 캐릭터 이름
        public Sprite unitIcon;                  // 캐릭터 아이콘

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
            if (isTurn)
            {
                HandleState();
                playerTransform = TurnSystem.instance.playerTargetPosition;
                if (currentState == EnemyState.Idle)
                {
                    if (isTurn)
                    {
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
                    PerformAttack();
                    break;
                case EnemyState.Returning:
                    ReturnToInitialPosition();
                    break;
            }
        }

        // 공격을 시작하도록 호출되는 메서드
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
            // 플레이어를 향해 움직이기
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

        protected virtual void PerformAttack()
        {
            if (!isAttackExecuted && !skillAttack)
            {
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Trigger EnemyAttack");
                isAttackExecuted = true; // 공격을 수행한 것으로 표시
            }
            else if (!isAttackExecuted && skillAttack)
            {
                // 스킬 공격 로직
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Trigger EnemySkillAttack");
                isAttackExecuted = true;

                enemySkillPoint -= 2;
            }
        }

        protected virtual void ReturnToInitialPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);  // 캐릭터가 원래 방향을 바라보도록 회전
            animator.SetFloat("Speed", 1);

            if (Vector3.Distance(transform.position, initialPosition) <= 0.1f)
            {
                transform.position = initialPosition;  // 위치 보정
                transform.rotation = initialRotation;  // 회전 보정
                animator.SetFloat("Speed", 0);

                currentState = EnemyState.Idle;
                isAttackExecuted = false;

                isTurn = false;
                skillAttack = false;
                TurnSystem.instance.EndTurn();
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
            TurnSystem.instance.RemoveCharacterFromTurnOrder(this);
            FayePlayerControl.instance.IncreaseBuffPower();
        }
    }
}