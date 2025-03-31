using UnityEngine;
using System.Collections;
using Project1;
using System.Collections.Generic;
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

    public abstract class BaseCharacterControl : BaseUnit
    {
        public static BaseCharacterControl instance;

        protected Animator animator;
        protected Vector3 initialPosition;
        protected Quaternion initialRotation;

        [Header("캐릭터 정보")]
        //public float maxHealth;               // 최대 체력
        //public float curHealth;               // 현재 체력
        //public float moveSpeed;               // 이동 속도
        //public float unitSpeed;               // 유닛 속도(턴 순서 관련)
        public float playerAttackPower;       // 플레이어 기본공격력
        public float playerSkillAttackPower;  // 플레이어 스킬공격력
        public int buffTurn;                  // 남은 버프 턴
        //public int buffPower = 0;             // 플레이어 버프 파워
        //public float attackRange;             // 공격 거리
        //public float damageReduction;         // 피해 감소
        //public float damageIncreased = 1;     // 피해 증가
        public bool startAttacking;           // 공격중을 알리는 연산자
        public bool startBlocking;            // 방어중을 알리는 연산자
        public bool skillAttack;              // 스킬공격을 할지 알리는 연산자
        public bool isTurn = false;           // 본인 턴인지 알려주는 연산자
        public bool isBlock = false;          // 본인이 방어 상태인지 알려주는 연산자
        public Slider hpBarSlider;            // HP바
        public Text hpText;                   // HP 텍스트
        public EnemySelection enemySelection; // 선택된 적 관리
        public Transform currentTarget;       // 현재 이동 중인 적의 Transform
        public string unitName;               // 캐릭터 이름
        public Sprite unitIcon;               // 캐릭터 아이콘


        [Header("캐릭터 움직임")]
        public PlayerState currentState = PlayerState.Idle; // 현재 상태 추가
        protected bool isAttackExecuted = false;

        //private List<Buff> activeBuffs = new List<Buff>();

        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            CheckHP();
        }

        protected virtual void Update()
        {
            // 캐릭터가 자신의 턴일 경우에 입력 처리
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
                    // Idle 상태의 로직
                    CheckIdle();
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

        /*public void AddBuff(Buff newBuff)
        {
            activeBuffs.Add(newBuff);
            newBuff.ApplyEffect(this);
        }

        public void RemoveExpiredBuffs()
        {
            activeBuffs.RemoveAll(buff => buff.remainingTurns <= 0);
        }

        public void OnTurnStart()
        {
            foreach (var buff in activeBuffs)
            {
                buff.remainingTurns--;
                if (buff.remainingTurns <= 0)
                {
                    buff.RemoveEffect(this);
                }
            }

            RemoveExpiredBuffs();
        }*/

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
                // 다음 캐릭터로 턴을 넘김
                TurnSystem.instance.EndTurn();
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
            }
        }


        protected virtual void HandleAttackInput()
        {
            // 각 플레이어 HandleAttackInput 참조
        }


        public void CheckHP()
        {
            if (hpBarSlider != null)
            {
                hpBarSlider.value = curHealth / maxHealth;
                hpText.text = curHealth.ToString();
            }
        }

        public void CheckIdle()
        {
            //damageReduction = 1f;
        }
        public void DoneBlock()
        {
            animator.SetBool("Trigger Block", false);
            isBlock = false;
            startBlocking = false;
            //damageReduction = 1f;
            currentState = PlayerState.Idle;
        }

        public void TakeDamage(float damage)
        {
            if (maxHealth == 0 || curHealth == 0)
                return;

            if (!isBlock)
            {
                animator.SetTrigger("Trigger Hit");
            }

            curHealth -= damage;

            CheckHP();

            if (curHealth <= 0)
            {
                Die();
            }
            Debug.Log("hit");
        }

        // 아군 사망시 호출
        public void Die()
        {
            Debug.Log("아군 사망");
            Destroy(gameObject);
            TurnSystem.instance.RemoveCharacterFromTurnOrder(this);            
        }
    }
}
