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
        RangedAttacking,
        Blocking,
        Returning,
        Buffing,
        Healing
    }

    public enum AttackPrepareState
    {
        None,
        Basic,   // 일반공격
        Skill,   // 스킬공격
        Buff     // 자기강화 or 아군보조
    }

    public abstract class BaseCharacterControl : BaseUnit
    {
        public static BaseCharacterControl instance;

        public Vector3 initialPosition;
        protected Quaternion initialRotation;

        [Header("UI Prefab")]
        public GameObject uiPrefab;   // 캐릭터 UI 프리셉

        [HideInInspector] public CharacterUI ui; // 나타나게 될 UI

        [Header("캐릭터 상태")]
        public bool startAttacking;           // 공격시작을 알리는 변수
        public bool startBlocking;            // 방어시작을 알리는 변수
        public bool isTurn = false;           // 자신의 턴임을 알려주는 변수
        public bool isBlock = false;          // 방어중인 상태임을 알려주는 변수
        public bool isPreparingAOEAttack = false;
        

        public AttackPrepareState prepareState = AttackPrepareState.None;

        public Slider hpBarSlider;            // HP바
        public Text hpText;                   // HP 텍스트
        public EnemySelection enemySelection; // 선택된 적 정보

        [Header("캐릭터 현재상태")]
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
            // 캐릭터가 자신의 턴일 경우에만 입력 처리
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
                // 팀 내 아군 대상
                return AllySelection.instance.GetAnchorTarget()?.transform;
            }
            else
            {
                // 적군 팀 내 대상
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

        protected virtual void HandleAttackInput()
        {
            if (!CanAttack())
                return;

            // Q 키 기본 공격
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

            // E 키 스킬 공격
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
        }

        private void ExecuteBasicAttack()
        {
            prepareState = AttackPrepareState.None;
            skillAttack = false;
            EnemySelectorUI.instance.HideAOEUI();

            if (attackRange >= 100f)
                currentState = PlayerState.RangedAttacking;
            else
                currentState = PlayerState.MovingToAttack;
        }

        private void ExecuteSkillAttack()
        {
            prepareState = AttackPrepareState.None; 
            skillAttack = true;
            EnemySelectorUI.instance.HideAOEUI();

            if (skillRange >= 100f)
                currentState = PlayerState.RangedAttacking;
            else
                currentState = PlayerState.MovingToAttack;
        }

        private void ExecuteBuff()
        {
            prepareState = AttackPrepareState.None;
            currentState = PlayerState.Buffing; // 필요한 경우 Idle이어도 상관없음
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
            // 예시: 공격력 +20%, 2턴 지속
            Buff selfBuff = null;
            selfBuff = new Buff("공격 강화", 2, 0.2f, 0);
            Debug.Log($"{unitName} 버프 발동!");
        }

        protected bool CanAttack()
        {
            // 기본 공격중일 때 움직여야 하고, 턴 진행 중 이동 중이 아니어야 함
            if (EnemySelection.instance.isMove)
                return false;

            // Idle인 상태 또는 준비 상태가 없을 경우 가능
            if (currentState == PlayerState.Idle && prepareState == AttackPrepareState.None)
                return true;

            // 이미 준비 상태라면 다시 입력 가능 (확정 및 변경)
            if (prepareState != AttackPrepareState.None)
                return true;

            return false;
        }

        public void TargetUpdate()
        {
            attackAnchorTarget = GetAttackAnchorTarget();
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
                case PlayerState.RangedAttacking:
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

        protected virtual void PerformAttack()
        {
            if (!isAttackExecuted && !skillAttack)
            {
                // 기본 공격 실행
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Trigger Attack");
                isAttackExecuted = true;
            }
            else if (!isAttackExecuted && skillAttack)
            {
                // 스킬 공격 실행
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
                EndTurnManually();
            }
        }

        public void HealEnd()
        {
            EndTurnManually();
        }

        protected virtual void ReturnToInitialPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, -180f, 0f);  // 캐릭터가 정면 방향을 바라보도록 회전
            animator.SetFloat("Speed", 1);

            if (Vector3.Distance(transform.position, initialPosition) <= 0.1f)
            {
                EndTurnManually();
            }
        }

        public void EndTurnManually()
        {
            transform.position = initialPosition;  // 위치 보정
            transform.rotation = initialRotation;  // 회전 보정
            animator.SetFloat("Speed", 0);

            // 방어 중이라면 상태 유지, 아니면 Idle로 변경
            if (!isBlock)
            {
                currentState = PlayerState.Idle;
            }
            else
            {
                currentState = PlayerState.Blocking;
            }

            isAttackExecuted = false;

            if (EnemySelection.instance != null)
                EnemySelection.instance.isMove = false;

            isTurn = false;
            // 다음 캐릭터로 턴 넘기기
            TurnSystem.instance.EndTurn();
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

        public override void OnBuffsUpdated()
        {
            if (ui != null)
                ui.UpdateBuff();
        }

        public override void OnBuffPowerUpdated(int currentPower)
        {
            if (ui != null)
                ui.UpdateBuffPower(currentPower);
        }

        // 아군 사망시 호출
        public override void Die()
        {
            base.Die();
            TurnSystem.instance.RemoveCharacterFromTurnOrder(this);            
        }
    }
}
