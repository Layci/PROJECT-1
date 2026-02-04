using Project1;
using ProJect1;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
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
        Buffing,
        Healing
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
        public bool startAttacking;           // 공격중을 알리는 연산자
        public bool startBlocking;            // 방어중을 알리는 연산자
        public bool isTurn = false;           // 본인 턴인지 알려주는 연산자
        public bool isBlock = false;          // 본인이 방어 상태인지 알려주는 연산자
        public bool isPreparingAOEAttack = false;
        

        public AttackPrepareState prepareState = AttackPrepareState.None;

        public Slider hpBarSlider;            // HP바
        public Text hpText;                   // HP 텍스트
        public EnemySelection enemySelection; // 선택된 적 관리

        [Header("캐릭터 움직임")]
        public PlayerState currentState = PlayerState.Idle; // 현재 상태 추가
        protected bool isAttackExecuted = false;

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

        public override Transform GetAttackAnchorTarget()
        {
            if (prepareState == AttackPrepareState.Skill && isHealSkill)
            {
                // 힐 → 아군 기준
                return AllySelection.instance.GetAnchorTarget()?.transform;
            }
            else
            {
                // 공격 → 적 기준
                return EnemySelection.instance.GetAnchorTarget()?.transform;
            }
        }

        public override List<BaseUnit> GetAttackTargets(int range)
        {
            return EnemySelection.instance.GetAOETargets(range);
        }

        public override List<BaseUnit> GetHealTargets(int range)
        {
            var result = new List<BaseUnit>();

            var players = TurnSystem.instance.playerCharacters;
            if (players == null || players.Count == 0)
                return result;

            int centerIndex = AllySelection.instance.selectedIndex;

            int left = Mathf.Max(0, centerIndex - range);
            int right = Mathf.Min(players.Count - 1, centerIndex + range);

            for (int i = left; i <= right; i++)
                result.Add(players[i]);

            return result;
        }
        /*public override List<BaseUnit> GetHealTargets(int range)
        {
            var result = new List<BaseUnit>();

            var players = TurnSystem.instance.playerCharacters;
            if (players == null || players.Count == 0)
                return result;

            int centerIndex = TurnSystem.instance.playerCharacters.IndexOf(this);

            int left = Mathf.Max(0, centerIndex - range);
            int right = Mathf.Min(players.Count - 1, centerIndex + range);

            for (int i = left; i <= right; i++)
                result.Add(players[i]);

            return result;
        }*/

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
                    AllySelectorUI.instance.HideAll();
                    TurnSystem.instance.SetAllPlayersPrepareState(AttackPrepareState.Basic);
                    BattleCameraManager.Instance.SwitchToDefault();
                    int range = normalAttackRange;
                    var targets = EnemySelection.instance.GetAOETargets(range);
                    EnemySelection.instance.UpdateSelectedEnemy();
                    EnemySelectorUI.instance.ShowAOETargets(targets.Select(e => e.transform).ToList());
                    ButtonManager.instance.HighlightBtn();
                }
                else
                {
                    ExecuteBasicAttack();
                    SkillPointManager.instance.SkillPointUp();
                }
            }

            // E → 스킬 공격
            if (Input.GetKeyDown(KeyCode.E) && SkillPointManager.instance.curSkillPoint > 0)
            {
                if (prepareState == AttackPrepareState.Skill)
                {
                    if (isHealSkill)
                        ExecuteHeal();
                    else
                        ExecuteSkillAttack();

                    SkillPointManager.instance.UseSkillPoint();
                }
                else
                {
                    TurnSystem.instance.SetAllPlayersPrepareState(AttackPrepareState.Skill);

                    int range = skillAttackRange;

                    if (isHealSkill)
                    {
                        EnemySelectorUI.instance.HideAOEUI();
                        AllySelection.instance.UpdateSelectedAlly();
                        BattleCameraManager.Instance.SwitchToHeal();
                    }
                    else
                    {
                        EnemySelection.instance.UpdateSelectedEnemy();
                    }

                    ButtonManager.instance.HighlightBtn();
                }
            }

            /*if (Input.GetKeyDown(KeyCode.E) && SkillPointManager.instance.curSkillPoint > 0)
            {
                if (prepareState == AttackPrepareState.Skill)
                {
                    // 이미 준비 상태 → 확정 실행
                    ExecuteSkillAttack();
                    SkillPointManager.instance.UseSkillPoint();
                }
                else
                {
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
            }*/

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

        private void ExecuteHeal()
        {
            prepareState = AttackPrepareState.None;
            currentState = PlayerState.Healing;
            skillAttack = true;
            var targets = AllySelection.instance.GetTargets(skillAttackRange);
            //HealSystem.Instance.ApplyHeal(this, targets);

            AllySelectorUI.instance.HideAll();
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
            attackAnchorTarget = GetAttackAnchorTarget();
        }
        /*public void TargetUpdate()
        {
            Transform targetposition = EnemySelectorUI.instance.selectedEnemy;
            attackAnchorTarget = targetposition;
        }*/

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
                case PlayerState.Healing:
                    PerformHeal();
                    break;
            }
        }

        protected virtual void MoveToAttack()
        {
            if (attackAnchorTarget == null)
                return;

            EnemySelection.instance.isMove = true;

            transform.position = Vector3.MoveTowards(transform.position, attackAnchorTarget.position, moveSpeed * Time.deltaTime);

            animator.SetFloat("Speed", 1);

            float distanceToTarget = Vector3.Distance(transform.position, attackAnchorTarget.position);

            if (!skillAttack)
            {
                if (distanceToTarget <= attackRange && !isBlock)
                    currentState = PlayerState.Attacking;
            }
            else
            {
                if (distanceToTarget <= skillRange)
                    currentState = PlayerState.Attacking;
            }
        }

        /*protected virtual void MoveToAttack()
        {
            if (attackAnchorTarget != null)
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
        }*/

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

        protected virtual void PerformHeal()
        {
            if (!isAttackExecuted)
            {
                animator.SetFloat("Speed", 0);
                animator.SetBool("Trigger Heal", true);
                isAttackExecuted = true;
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

        public void HealEnd()
        {
            isTurn = false;
            // 다음 캐릭터로 턴을 넘김
            TurnSystem.instance.EndTurn();
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

        public override void CheckHP()
        {
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
            TurnSystem.instance.RemoveCharacterFromTurnOrder(this);            
        }

        public override void IncreaseBuffPower()
        {
            base.IncreaseBuffPower();

            if (ui != null)
                ui.UpdateBuffPower(buffPower);
        }

        public override void ResetBuffPower()
        {
            base.ResetBuffPower();

            if (ui != null)
                ui.UpdateBuffPower(0);
        }
    }
}
