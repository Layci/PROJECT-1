using Project1;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class BaseUnit : MonoBehaviour
    {
        [Header("유닛 정보")]
        public string unitName;               // 유닛 이름
        public Sprite unitIcon;               // 유닛 아이콘
        public float maxHealth;               // 최대 체력
        public float curHealth;               // 현재 체력
        public float moveSpeed = 3;           // 이동 속도
        public float unitSpeed;               // 유닛 속도(턴 순서 관련)
        public float attackRange;             // 공격 거리
        public float skillRange;              // 스킬 거리
        public float damageReduction = 1f;    // 피해 감소
        public float damageIncreased = 1;     // 피해 증가
        public float unitSpacing = 2f;        // 각 유닛마다 간격 조절 거리
        public float animationSpeed = 1f;     // 애니메이션 스피드
        public bool isDead = false;           // 사망 판정

        [Header("공격 범위 설정")]
        [Tooltip("기본 공격 범위 (0 = 단일, 1 = 양옆 1칸 포함)")]
        public int normalAttackRange = 0;

        [Header("범위 공격")]
        [Tooltip("0이면 단일 대상, 1 이상이면 범위 공격")]
        public int skillAttackRange = 0;

        [Header("버프 정보")]
        public int buffTrun;                  // 남은 버프 턴
        public int buffPower;                 // 버프 파워
        public int maxBuffPower = 3;          // 최대로 증가할 파워
        public bool buff = false;             // 버프 적용 확인 연산자

        public List<Buff> activeBuffs = new List<Buff>();
        protected Animator animator;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            //curHealth = maxHealth;
        }

        public void AnimationSpeedCheck()
        {
            animator.speed = animationSpeed;
        }

        public virtual void TakeDamage(float damage)
        {
            if (isDead) return;

            curHealth -= damage;

            // Hit 애니메이션 처리 - 플레이어에서만 isBlock 고려
            if (this is BaseCharacterControl player)
            {
                if (!player.isBlock)
                    animator?.SetTrigger("Trigger Hit");
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
        }

        // -------------------------------- 버프관련
        public void AddBuff(Buff newBuff)
        {
            // 기존 버프 중 이름이 같은 것 찾기
            Buff existingBuff = activeBuffs.Find(buff => buff.buffName == newBuff.buffName);

            // 기존 버프가 있을 경우
            if (existingBuff != null)
            {
                // 새 버프가 기존 버프보다 약하면 적용하지 않음
                if (newBuff.attackBoost <= existingBuff.attackBoost &&
                    newBuff.defenseBoost <= existingBuff.defenseBoost)
                {
                    Debug.Log($"{newBuff.buffName} 는 기존 버프보다 약해 적용되지 않음.");
                    return;
                }

                // 새 버프가 더 강하면 기존 버프 제거 후 갱신
                existingBuff.RemoveEffect(this);
                activeBuffs.Remove(existingBuff);
            }

            // 버프 턴 수 적용
            buffTrun = newBuff.remainingTurns;

            // UI 업데이트 (아군만)
            BaseCharacterControl player = this as BaseCharacterControl;
            if (player != null && player.ui != null)
            {
                player.ui.UpdateBuff();
            }

            // 적 UI는 나중에 필요하면 동일하게 적용 가능
            // 적은 UI가 나중에 필요하면 여기서
            // BaseEnemyControl enemy = this as BaseEnemyControl;

            // 새 버프 추가 및 적용
            activeBuffs.Add(newBuff);
            newBuff.ApplyEffect(this);

            Debug.Log($"{newBuff.buffName} 버프 적용됨! (ATK {newBuff.attackBoost}, DEF {newBuff.defenseBoost})");
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
                buffTrun = buff.remainingTurns;

                Debug.Log($"{buff.remainingTurns} 남은버프턴");

                if (buff.remainingTurns <= 0)
                {
                    // Buff의 설정에 따라 BuffPower 초기화
                    BaseCharacterControl player = this as BaseCharacterControl;

                    if (player != null && player.ui != null)
                    {
                        BuffIconUI iconUI = player.ui.buffIconUI;

                        // resetPowerOnExpire 옵션이 있을 때만 초기화
                        if (iconUI != null && buff.resetPowerOnExpire)
                        {
                            buffPower = 0;
                            player.ui.UpdateBuffPower(0);
                        }
                    }

                    buff.RemoveEffect(this);
                }
            }

            RemoveExpiredBuffs();
        }
        public virtual void IncreaseBuffPower()
        {
            if (buffPower < maxBuffPower)
                buffPower++;
        }

        public virtual void ResetBuffPower()
        {
            buffPower = 0;
        }
    }
}
