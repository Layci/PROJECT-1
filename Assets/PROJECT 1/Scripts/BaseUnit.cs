using Project1;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ProJect1
{
    // [방법 선택] 
    // AttackTriggerType.All    : 모든 공격 시 30% 확률
    // AttackTriggerType.Normal : 일반 공격 시에만 30% 확률
    // AttackTriggerType.Skill  : 스킬 공격 시에만 30% 확률
    public enum AttackTriggerType
    {
        Normal, // 일반 공격만
        Skill,  // 스킬 공격만
        All     // 모든 공격
    }

    public abstract class BaseUnit : MonoBehaviour
    {
        [Header("기본 정보")]
        public string unitName;               // 유닛 이름
        public Sprite unitIcon;               // 유닛 아이콘
        public float maxHealth;               // 최대 체력
        public float curHealth;               // 현재 체력
        public float moveSpeed = 3;           // 이동 속도
        public float unitSpeed;               // 유닛 속도(턴 순서 결정)
        public float attackRange;             // 공격 사거리
        public float skillRange;              // 스킬 사거리
        public float damageReduction = 1f;    // 데미지 감소
        public float damageIncreased = 1;     // 데미지 증가
        public float unitSpacing = 2f;        // 유닛 배치 간격
        public float animationSpeed = 1f;     // 애니메이션 배속
        public bool isDead = false;           // 사망 상태
        public bool isHealSkill;
        public float AttackPower;       // 기본 공격력
        public float SkillAttackPower;  // 스킬 공격력
        public bool skillAttack;        // 스킬 공격 여부

        public Transform attackAnchorTarget;

        [Header("기본 공격 설정")]
        [Tooltip("0 = 단일, 1 이상 = 범위")]
        public int normalAttackRange = 0;
        public EffectAsset normalAttackEffect;

        [Header("스킬 공격 설정")]
        [Tooltip("0 = 단일, 1 이상 = 범위")]
        public int skillAttackRange = 0;
        public EffectAsset skillAttackEffect;

        [Header("이펙트 위치")]
        public Transform centerPoint;   
        public Transform headPoint;     
        public Transform projectileSpawnPoint;

        [Header("버프 시스템 정보")]
        public int buffTrun;                  // 남은 버프 턴 (하위 호환용)
        public int buffPower;                 // 현재 버프 파워
        public int maxBuffPower = 3;          // 최대 버프 파워
        public bool buff = false;             

        public List<Buff> activeBuffs = new List<Buff>();
        protected Animator animator;

        public abstract List<BaseUnit> GetAttackTargets(int range);
        public abstract List<BaseUnit> GetHealTargets(int range);
        public abstract Transform GetAttackAnchorTarget();

        public virtual List<EffectAsset> GetAllEffects()
        {
            List<EffectAsset> list = new List<EffectAsset>();
            if (normalAttackEffect != null) list.Add(normalAttackEffect);
            if (skillAttackEffect != null) list.Add(skillAttackEffect);
            return list;
        }

        public void Heal(int amount)
        {
            curHealth = Mathf.Min(curHealth + amount, maxHealth);
            Debug.Log($"{unitName} {amount}만큼 회복");
            CheckHP();
        }

        public abstract void CheckHP();

        public Vector3 GetProjectileSpawnPosition(EffectAsset asset)
        {
            if (projectileSpawnPoint != null)
                return projectileSpawnPoint.position + asset.offset;

            return centerPoint != null
                ? centerPoint.position + asset.offset
                : transform.position + Vector3.up * 1.2f + asset.offset;
        }

        public Transform GetEffectTargetPivot(EffectSpawnType targetType)
        {
            switch (targetType)
            {
                case EffectSpawnType.Head:
                    return headPoint != null ? headPoint : (centerPoint != null ? centerPoint : transform);
                case EffectSpawnType.Center:
                    return centerPoint != null ? centerPoint : transform;
                default:
                    return transform;
            }
        }

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void AnimationSpeedCheck()
        {
            if(animator != null) animator.speed = animationSpeed;
        }

        public virtual void TakeDamage(float damage)
        {
            if (isDead) return;

            curHealth -= damage;

            if (this is BaseCharacterControl player)
            {
                if (!player.isBlock) animator?.SetTrigger("Trigger Hit");
            }
            else
            {
                animator?.SetTrigger("Trigger EnemyHit");
            }

            if (curHealth <= 0)
            {
                curHealth = 0;
                Die();
            }
        }

        public virtual void Die()
        {
            isDead = true;
            Debug.Log($"{unitName} 사망");
            BattleManager.instance.CheckDefeat();
        }

        // -------------------------------- 버프/디버프 시스템
        
        public virtual void OnBuffsUpdated() { }
        public virtual void OnBuffPowerUpdated(int currentPower) { }

        // 공격 적중 시 실행되는 훅 (하위 클래스에서 오버라이드)
        public virtual void OnHitTarget(BaseUnit target, bool isSkill) { }

        // 확률적으로 버프를 시도하는 헬퍼 메소드
        public void TryApplyBuff(Buff buff, BaseUnit target, float chance = 1.0f)
        {
            if (target == null || buff == null) return;

            if (UnityEngine.Random.value <= chance)
            {
                target.AddBuff(buff.Clone());
            }
        }

        // 특정 공격 타입(일반, 스킬, 전체)에 맞춰 버프 시도
        public void TryApplyBuffOnHit(Buff buff, BaseUnit target, bool isSkill, AttackTriggerType triggerType, float chance = 1.0f)
        {
            bool canTrigger = false;
            switch (triggerType)
            {
                case AttackTriggerType.Normal: canTrigger = !isSkill; break;
                case AttackTriggerType.Skill:  canTrigger = isSkill;  break;
                case AttackTriggerType.All:    canTrigger = true;     break;
            }

            if (canTrigger)
            {
                TryApplyBuff(buff, target, chance);
            }
        }

        public void AddBuff(Buff newBuff)
        {
            Buff existingBuff = activeBuffs.Find(b => b.buffName == newBuff.buffName);

            if (existingBuff != null)
            {
                // 1. 지속 시간 갱신 (리셋)
                existingBuff.remainingTurns = newBuff.originalDuration;

                // 2. 수치 갱신 (새로운 버프가 더 강력한 경우에만 교체)
                // 버프(양수)일 때는 클수록, 디버프(음수)일 때는 절대값이 클수록 강력한 것으로 간주
                bool isStronger = (Mathf.Abs(newBuff.attackBoost) > Mathf.Abs(existingBuff.attackBoost)) ||
                                 (Mathf.Abs(newBuff.defenseBoost) > Mathf.Abs(existingBuff.defenseBoost)) ||
                                 (Mathf.Abs(newBuff.tickValue) > Mathf.Abs(existingBuff.tickValue));

                if (isStronger)
                {
                    existingBuff.RemoveEffect(this);
                    existingBuff.attackBoost = newBuff.attackBoost;
                    existingBuff.defenseBoost = newBuff.defenseBoost;
                    existingBuff.tickValue = newBuff.tickValue;
                    existingBuff.ApplyEffect(this);
                    Debug.Log($"{unitName}의 {newBuff.buffName} 효과가 더 강력한 수치로 갱신되었습니다.");
                }
                else
                {
                    Debug.Log($"{unitName}의 {newBuff.buffName} 지속 시간이 리셋되었습니다.");
                }
            }
            else
            {
                activeBuffs.Add(newBuff);
                newBuff.ApplyEffect(this);
            }
            
            OnBuffsUpdated();
        }

        public virtual void OnTurnStart()
        {
            for (int i = activeBuffs.Count - 1; i >= 0; i--)
            {
                var buffItem = activeBuffs[i];
                // 시작형 버프/디버프만 처리 (Tick 실행 + 지속시간 감소)
                if (buffItem.isTickAtStart)
                {
                    buffItem.TickEffect(this);
                    UpdateBuffDuration(i);
                }
            }
            OnBuffsUpdated();
        }

        public virtual void OnTurnEnd()
        {
            for (int i = activeBuffs.Count - 1; i >= 0; i--)
            {
                var buffItem = activeBuffs[i];
                // 종료형 버프/디버프만 처리 (Tick 실행 + 지속시간 감소)
                if (!buffItem.isTickAtStart)
                {
                    buffItem.TickEffect(this);
                    UpdateBuffDuration(i);
                }
            }
            OnBuffsUpdated();
        }

        // 버프 지속시간 감소 및 만료 처리를 위한 공용 메소드
        protected void UpdateBuffDuration(int index)
        {
            if (index < 0 || index >= activeBuffs.Count) return;

            var buffItem = activeBuffs[index];
            buffItem.remainingTurns--;
            buffTrun = buffItem.remainingTurns;

            if (buffItem.remainingTurns <= 0)
            {
                if (buffItem.resetPowerOnExpire) ResetBuffPower();
                buffItem.RemoveEffect(this);
                activeBuffs.RemoveAt(index);
            }
        }

        public virtual void IncreaseBuffPower()
        {
            if (buffPower < maxBuffPower)
            {
                buffPower++;
                OnBuffPowerUpdated(buffPower);
            }
        }

        public virtual void ResetBuffPower()
        {
            buffPower = 0;
            OnBuffPowerUpdated(0);
        }
    }
}
