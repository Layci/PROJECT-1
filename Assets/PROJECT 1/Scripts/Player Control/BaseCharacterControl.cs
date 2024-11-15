using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Project1
{
    public enum PlayerState
    {
        Idle,
        MovingToAttack,
        Attacking,
        Returning
    }

    public abstract class BaseCharacterControl : MonoBehaviour
    {
        public static BaseCharacterControl instance;

        protected Animator animator;
        protected Vector3 initialPosition;
        protected Quaternion initialRotation;

        [Header("캐릭터 정보")]
        public float maxHealth;               // 최대 체력
        public float curHealth;               // 현재 체력
        public float moveSpeed;               // 이동 속도
        public float unitSpeed;               // 유닛 속도(턴 순서 관련)
        public float playerAttackPower;       // 플레이어 기본공격력
        public float playerSkillAttackPower;  // 플레이어 스킬공격력
        public float attackRange;             // 공격 거리
        public bool startAttacking;           // 공격중을 알리는 연산자
        public bool skillAttack;              // 스킬공격을 할지 알리는 연산자
        public bool isTurn = false;           // 본인 턴인지 알려주는 연산자
        public Transform enemyTransform1;     // 적 위치 참조
        public Slider hpBarSlider;            // HP바

        [Header("캐릭터 움직임")]
        public PlayerState currentState = PlayerState.Idle; // 현재 상태 추가
        protected bool isAttackExecuted = false;

        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        protected virtual void Update()
        {
            // 현재 턴의 캐릭터가 자신인 경우에만 입력을 처리
            /*if ((GameManager.instance.turnSystem.CurrentCharacter as BaseCharacterControl) == this)
            {
                HandleState();
                HandleAttackInput();
            }*/
            // 캐릭터가 자신의 턴일 경우에 입력 처리
            if (isTurn)
            {
                HandleState();
                HandleAttackInput();
            }
        }



        protected void HandleState()
        {
            switch (currentState)
            {
                case PlayerState.Idle:
                    // Idle 상태의 로직
                    break;
                case PlayerState.MovingToAttack:
                    MoveToAttack();
                    break;
                case PlayerState.Attacking:
                    PerformAttack();
                    break;
                case PlayerState.Returning:
                    ReturnToInitialPosition();
                    break;
            }
        }

        protected virtual void MoveToAttack()
        {
            if (enemyTransform1 != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, enemyTransform1.position, moveSpeed * Time.deltaTime);
                animator.SetFloat("Speed", 1);

                float distanceToTarget = Vector3.Distance(transform.position, enemyTransform1.position);
                if (distanceToTarget <= attackRange)
                {
                    currentState = PlayerState.Attacking;
                }
            }
        }

        protected virtual void PerformAttack()
        {
            if (!isAttackExecuted && !skillAttack)
            {
                // 공격 로직
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Trigger Attack");
                isAttackExecuted = true;
            }
            else if (!isAttackExecuted && skillAttack)
            {
                // 스킬 공격 로직
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Trigger SkillAttack");
                isAttackExecuted = true;
            }
        }

        protected virtual void ReturnToInitialPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, -180f, 0f);  // 캐릭터가 원래 방향을 바라보도록 회전
            animator.SetFloat("Speed", 1);

            if (Vector3.Distance(transform.position, initialPosition) <= 0.1f)
            {
                transform.position = initialPosition;  // 위치 보정
                transform.rotation = initialRotation;  // 회전 보정
                animator.SetFloat("Speed", 0);

                currentState = PlayerState.Idle;
                isAttackExecuted = false;

                isTurn = false;
                // 다음 캐릭터로 턴을 넘김
                TurnSystem.instance.EndTurn();
                //GameManager.instance.turnSystem.EndTurn();  // 턴 종료
            }
        }


        protected virtual void HandleAttackInput()
        {
            /*// 현재 턴의 캐릭터만 입력을 처리하도록 설정
            if ((GameManager.instance.turnSystem.CurrentCharacter as BaseCharacterControl) == this && currentState == PlayerState.Idle)
            {
                // 공격 및 스킬 입력 처리
            }*/
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

            animator.SetTrigger("Trigger Hit");

            curHealth -= damage;

            CheckHP();

            if (curHealth <= 0)
            {
                Debug.Log("죽음");
            }
            Debug.Log("hit");
        }

        // StartMoveToAttack 메서드 추가
        public void StartMoveToAttack()
        {
            currentState = PlayerState.MovingToAttack;
        }

        public void StartAttack()
        {
            currentState = PlayerState.Attacking;
        }

        public void StopAction()
        {
            currentState = PlayerState.Idle;
        }

        // 기본적인 WaitForInput 메서드, 모든 캐릭터가 이를 가질 수 있도록 정의
        public virtual void WaitForInput()
        {
            // 기본적으로 아무것도 하지 않음 (오버라이드 목적)
        }
    }
}
