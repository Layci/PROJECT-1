using Project1;
using ProJect1;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
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
        public EnemyData enemyData;

        //protected Animator animator;
        public Vector3 initialPosition;
        protected Quaternion initialRotation;

        [Header("적 정보")]
        //public float maxHealth;                  // 최대 체력
        //public float curHealth;                  // 현재 체력
        //public float moveSpeed;                  // 이동 속도
        //public float unitSpeed;                  // 유닛 속도(턴 순서 관련)
        //public float AttackPower;           // 적 기본 공격력
        //public float SkillAttackPower;      // 플레이어 스킬공격력
        //public float attackRange;                // 공격 거리
        public float enemySkillPoint;            // 적 공격 스킬 포인트
        //public float damageReduction = 1f;  // 적 피해 감소
        //public float damageIncreased = 1;     // 피해 증가
        public bool startAttacking;              // 공격중을 알리는 연산자
        //public bool skillAttack;                 // 스킬공격을 할지 알리는 연산자
        public bool isTurn = false;              // 본인 턴인지 알려주는 연산자
        //public Transform currentTarget;        // 플레이어 참조
        public Slider hpBarSlider;               // HP바
        //public string unitName;                  // 캐릭터 이름
        //public Sprite unitIcon;                  // 캐릭터 아이콘

        [Header("적 움직임")]
        public EnemyState currentState = EnemyState.Idle;
        protected bool isAttackExecuted = false;
        private TurnSystem turnSystem;

        protected override void Awake()
        {
            base.Awake();
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
                currentTarget = TurnSystem.instance.playerTargetPosition;
                if (currentState == EnemyState.Idle)
                {
                    if (isTurn)
                    {
                        initialRotation = transform.rotation;
                        StartMove();
                    }
                }
            }
        }

        public override List<BaseUnit> GetAttackTargets(int range)
        {
            int centerIndex = GetMyIndexInPlayerList();
            return PlayerSelection.instance.GetAOETargetsByIndex(centerIndex, range)
                .ConvertAll(go => go.GetComponent<BaseUnit>());
        }

        public override List<BaseUnit> GetHealTargets(int range)
        {
            var result = new List<BaseUnit>();

            var players = TurnSystem.instance.enemyCharacters;
            if (players == null || players.Count == 0)
                return result;

            int centerIndex = TurnSystem.instance.enemyCharacters.IndexOf(this);

            int left = Mathf.Max(0, centerIndex - range);
            int right = Mathf.Min(players.Count - 1, centerIndex + range);

            for (int i = left; i <= right; i++)
                result.Add(players[i]);

            return result;
        }

        private int GetMyIndexInPlayerList()
        {
            return TurnSystem.instance.playerCharacters.IndexOf(currentTarget.GetComponent<BaseCharacterControl>());
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
                    if (currentTarget != null)
                    {
                        PerformAttack(currentTarget.gameObject);
                    }
                    else
                    {
                        Debug.LogWarning("playerTransform이 파괴되어 공격을 수행할 수 없습니다.");
                        currentState = EnemyState.Returning;
                    }
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
            if (currentTarget != null)
            {
                currentState = EnemyState.MovingToAttack;
            }
        }

        protected virtual void MoveToAttack()
        {
            // 플레이어를 향해 움직이기
            if (currentTarget != null)
            {
                
                transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);
                animator.SetFloat("Speed", 1);

                int range = skillAttack ? skillAttackRange : normalAttackRange;

                // 현재 적의 인덱스를 턴시스템에서 찾기
                var turnSystem = FindObjectOfType<TurnSystem>();
                int playerIndex = turnSystem.playerCharacters.IndexOf(currentTarget.GetComponent<BaseCharacterControl>());
                // 인덱스 기반으로 AOE 범위 계산
                var targets = PlayerSelection.instance.GetAOETargetsFromEnemy(range, playerIndex);
                TargetIndicatorManager.Instance.ShowTargetIndicators(targets);

                // 외곽선 표시
                //EnemyAOEHighlighter.Instance.ShowAOETargets(targets);
                
                float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);               
                Debug.Log($"[Enemy] 이동 전 AOE 표시됨, 대상 수: {targets.Count}");
                if (!skillAttack)
                {
                    if (distanceToTarget <= attackRange)
                    {
                        currentState = EnemyState.Attacking;
                    }
                }
                else if (skillAttack)
                {
                    if (distanceToTarget <= skillRange)
                    {
                        currentState = EnemyState.Attacking;
                    }
                }
            }
        }

        protected virtual void PerformAttack(GameObject target)
        {
            if (isAttackExecuted) return;

            // 애니메이션 트리거
            animator.SetFloat("Speed", 0);
            animator.SetTrigger(skillAttack ? "Trigger EnemySkillAttack" : "Trigger EnemyAttack");
            if (skillAttack) enemySkillPoint -= 2;

            isAttackExecuted = true;

            // 추가효과 (버프 등)
            TasterPlayerControl taster = target.GetComponent<TasterPlayerControl>();
            if (taster != null)
            {
                taster.IncreaseBuffPower();
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

                if (TargetIndicatorManager.Instance != null)
                    TargetIndicatorManager.Instance.ClearIndicators();

                TurnSystem.instance.EndTurn();
            }
        }

        public void ApplyEnemyData()
        {
            if (enemyData != null)
            {
                unitName = enemyData.enemyName;
                maxHealth = enemyData.maxHealth;
                curHealth = enemyData.maxHealth;
                AttackPower = enemyData.attackPower;
                SkillAttackPower = enemyData.skillAttackPower;
                unitSpeed = enemyData.unitSpeed;
                //attackRange = enemyData.attackRange;
            }
        }

        // TakeDamage 메서드 추가
        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);

            CheckHP();
        }

        public override void CheckHP()
        {
            if (hpBarSlider != null)
            {
                hpBarSlider.value = curHealth / maxHealth;
            }
        }

        // 적이 죽었을 때 처리
        public override void Die()
        {
            base.Die();
            Destroy(gameObject);
            TurnSystem.instance.RemoveCharacterFromTurnOrder(this);
            Debug.Log("faye 포인트 증가");
            BattleManager.instance.RefreshUnitLists();
            BattleManager.instance.RepositionEnemyUnits();
            if (EnemySelection.instance.selectedEnemyIndex == TurnSystem.instance.enemyCharacters.Count)
            {
                if (TurnSystem.instance.enemyCharacters.Count > 0)
                {
                    EnemySelection.instance.selectedEnemyIndex = TurnSystem.instance.enemyCharacters.Count - 1;
                    return;
                }
                EnemySelection.instance.selectedEnemyIndex = 0;
                Debug.Log("선택적 맨 오른쪽임");
            }
            else if (EnemySelection.instance.selectedEnemyIndex == 0)
            {
                EnemySelection.instance.selectedEnemyIndex = 0;
                Debug.Log("선택적 맨 왼쪽임");
            }

            // FayePlayerControl 인스턴스를 찾아 버프 파워 증가 및 UI 업데이트
            FayePlayerControl faye = FayePlayerControl.instance;
            if (faye != null)
            {
                faye.IncreaseBuffPower();
            }
        }
    }
}