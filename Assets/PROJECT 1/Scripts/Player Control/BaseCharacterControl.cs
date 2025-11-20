using Project1;
using ProJect1;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
//using DG.Tweening;

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
        Basic,   // 일반공격
        Skill,   // 범위공격
        Buff     // 자기강화 or 선택버프
    }

    public abstract class BaseCharacterControl : BaseUnit
    {
        public static BaseCharacterControl instance;

        public Vector3 initialPosition;
        protected Quaternion initialRotation;

        [Header("UI Prefab")]
        public GameObject uiPrefab;   // 캐릭터 UI 프리팹

        [HideInInspector] public CharacterUI ui; // 런타임 생성되는 UI

        [Header("캐릭터 정보")]
        //public float maxHealth;               // 최대 체력
        //public float curHealth;               // 현재 체력
        //public float moveSpeed;               // 이동 속도
        //public float unitSpeed;               // 유닛 속도(턴 순서 관련)
        public float playerAttackPower;       // 플레이어 기본공격력
        public float playerSkillAttackPower;  // 플레이어 스킬공격력
        //public int buffPower = 0;             // 플레이어 버프 파워
        //public float attackRange;             // 공격 거리
        //public float damageReduction;         // 피해 감소
        //public float damageIncreased = 1;     // 피해 증가
        public bool startAttacking;           // 공격중을 알리는 연산자
        public bool startBlocking;            // 방어중을 알리는 연산자
        public bool skillAttack;              // 스킬공격을 할지 알리는 연산자
        public bool isTurn = false;           // 본인 턴인지 알려주는 연산자
        public bool isBlock = false;          // 본인이 방어 상태인지 알려주는 연산자
        public bool isPreparingAOEAttack = false;
        
        public AttackPrepareState prepareState = AttackPrepareState.None;

        public Slider hpBarSlider;            // HP바
        public Text hpText;                   // HP 텍스트
        public EnemySelection enemySelection; // 선택된 적 관리
        public Transform currentTarget;       // 현재 이동 중인 적의 Transform
        //public string unitName;               // 캐릭터 이름
        //public Sprite unitIcon;               // 캐릭터 아이콘


        [Header("캐릭터 움직임")]
        public PlayerState currentState = PlayerState.Idle; // 현재 상태 추가
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
            // 캐릭터가 자신의 턴일 경우에 입력 처리
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

            // Q → 기본 공격
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("현재 prepareState: " + prepareState);

                if (prepareState != AttackPrepareState.Basic)
                {
                    TurnSystem.instance.SetAllPlayersPrepareState(AttackPrepareState.Basic);

                    int range = normalAttackRange;
                    var targets = EnemySelection.instance.GetAOETargets(range);
                    EnemySelectorUI.instance.ShowAOETargets(targets.Select(e => e.transform).ToList());
                    ButtonManager.instance.HighlightBtn();
                }
                else
                {
                    ExecuteBasicAttack();
                    SkillPointManager.instance.SkillPointUp();
                }

                /*if (prepareState == AttackPrepareState.Basic)
                {
                    Debug.Log("기본공격 확정 실행!");
                    ExecuteBasicAttack();
                    SkillPointManager.instance.SkillPointUp();
                }
                else
                {
                    Debug.Log("기본공격 준비 상태 진입");
                    prepareState = AttackPrepareState.Basic;

                    int range = normalAttackRange;

                    if (range == 0)
                    {
                        // 단일 공격 모드
                        EnemySelectorUI.instance.ShowSingleTargetUI();
                        EnemySelectorUI.instance.HideAOEUI();
                    }
                    else
                    {
                        // 범위 공격 모드
                        var targets = EnemySelection.instance.GetAOETargets(range);
                        EnemySelectorUI.instance.ShowAOETargets(targets.Select(e => e.transform).ToList());
                        EnemySelectorUI.instance.HideSingleTargetUI();
                    }
                }*/
            }

            // E → 스킬 공격
            if (Input.GetKeyDown(KeyCode.E) && SkillPointManager.instance.curSkillPoint > 0)
            {
                if (prepareState == AttackPrepareState.Skill)
                {
                    Debug.Log("현재 prepareState2: " + prepareState);
                    // 이미 준비 상태 → 확정 실행
                    ExecuteSkillAttack();
                    SkillPointManager.instance.UseSkillPoint();
                }
                else
                {
                    Debug.Log("스킬공격 준비 상태 진입");
                    Debug.Log("현재 prepareState2: " + prepareState);
                    Debug.Log(skillAttackRange);
                    // 준비 상태 진입
                    TurnSystem.instance.SetAllPlayersPrepareState(AttackPrepareState.Skill);
                    var cur = TurnSystem.instance.allCharacters[TurnSystem.instance.currentTurnIndex] as BaseUnit;
                    int range = cur != null ? cur.skillAttackRange : 0;

                    // 인덱스 기반 AOE 대상 가져오기
                    var targets = EnemySelection.instance.GetAOETargets(range);
                    Debug.Log("AOE 대상 수: " + targets.Count);
                    // UI 반영
                    EnemySelectorUI.instance.ShowAOETargets(targets.Select(e => e.transform).ToList());
                    EnemySelectorUI.instance.HideSingleTargetUI();

                    ButtonManager.instance.HighlightBtn();
                }
            }

            // R → 버프
            /*if (Input.GetKeyDown(KeyCode.R))
            {
                if (prepareState == AttackPrepareState.Buff)
                {
                    // 이미 준비 상태 → 확정 실행
                    ExecuteBuff();
                }
                else
                {
                    // 준비 상태 진입
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
            currentState = PlayerState.Buffing; // 필요시 Idle로도 가능
            ApplySelfBuff();
        }

        private void ApplySelfBuff()
        {
            // 예시: 공격력 +20%, 2턴 유지
            Buff selfBuff = null;
            selfBuff = new Buff("본인 강화", 2, 0.2f, 0, typeof(This));
            Debug.Log($"{unitName} 버프 발동!");
        }

        protected bool CanAttack()
        {
            // 기본 조건은 턴 중이어야 하고, 적 선택 중 이동 중이 아니어야 함
            if (EnemySelection.instance.isMove)
                return false;

            // Idle일 때는 준비 상태 진입 가능
            if (currentState == PlayerState.Idle && prepareState == AttackPrepareState.None)
                return true;

            // 이미 준비 상태일 때도 입력 허용 (확정 실행 위해)
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
                if (!skillAttack)
                {
                    if (distanceToTarget <= attackRange && !isBlock)
                    {
                        currentState = PlayerState.Attacking;
                    }
                }
                else if (skillAttack)
                {
                    if (distanceToTarget <= skillRange)
                    {
                        currentState = PlayerState.Attacking;
                    }
                }
            }
        }

        protected virtual void PerformAttack()
        {
            if (!isAttackExecuted && !skillAttack)
            {
                // 공격 로직
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Trigger PlayerAttack");
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

                EnemySelection.instance.isMove = false;
                isTurn = false;
                // 다음 캐릭터로 턴을 넘김
                TurnSystem.instance.EndTurn();
            }
        }

        public void CheckHP()
        {
            /*if (hpBarSlider != null)
            {
                hpBarSlider.value = curHealth / maxHealth;
                hpText.text = Mathf.RoundToInt(curHealth).ToString(); // 반올림해서 정수로 표시
            }*/
            if (ui != null)
                ui.UpdateHP();
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

        // 아군 사망시 호출
        public override void Die()
        {
            base.Die();
            Destroy(gameObject);
            TurnSystem.instance.RemoveCharacterFromTurnOrder(this);            
        }
    }
}
