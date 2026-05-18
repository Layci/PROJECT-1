using System;
using UnityEngine;
using ProJect1;

namespace Project1
{
    public enum EffectType
    {
        Buff,
        Debuff
    }

    public class Buff
    {
        public string buffName;        // 효과 이름
        public EffectType effectType;  // 버프/디버프 구분
        public int remainingTurns;     // 남은 지속 턴
        public int originalDuration;   // 초기 지속 턴 (리셋용)
        
        [Header("스탯 변동")]
        public float attackBoost;      // 공격력 증가치 (0.2 = +20%)
        public float defenseBoost;     // 방어력 증가치 (0.2 = +20%)
        
        [Header("지속 효과 (DoT/HoT)")]
        public float tickValue;        // 턴 시작/종료 시 변동될 수치 (양수: 힐, 음수: 데미지)
        public bool isTickAtStart = true; // true: 턴 시작 시, false: 턴 종료 시 효과 발생

        public Type exclusiveCharacter;  // 특정 캐릭터 전용 (필요한 경우)
        public bool resetPowerOnExpire;  // 만료 시 버프 파워 초기화 여부

        public Buff(string name, int duration, float atkBoost, float defBoost, 
                    EffectType type = EffectType.Buff, float tick = 0, bool tickAtStart = true,
                    Type exclusiveCharacter = null, bool resetPowerOnExpire = true)
        {
            buffName = name;
            remainingTurns = duration;
            originalDuration = duration; // 초기 지속시간 저장
            attackBoost = atkBoost;
            defenseBoost = defBoost;
            effectType = type;
            tickValue = tick;
            isTickAtStart = tickAtStart;
            this.exclusiveCharacter = exclusiveCharacter;
            this.resetPowerOnExpire = resetPowerOnExpire;
        }

        // 새로운 타겟에게 적용하기 위해 버프 객체 복사
        public Buff Clone()
        {
            return new Buff(buffName, originalDuration, attackBoost, defenseBoost, 
                            effectType, tickValue, isTickAtStart, exclusiveCharacter, resetPowerOnExpire);
        }

        // 스탯 효과 적용 (최초 1회)
        public void ApplyEffect(BaseUnit unit)
        {
            unit.damageIncreased += attackBoost;
            unit.damageReduction -= defenseBoost;
            Debug.Log($"{unit.unitName}에게 {buffName} ({effectType}) 적용! (ATK +{attackBoost * 100}%, DEF +{defenseBoost * 100}%)");
        }

        // 턴마다 발생하는 효과 (DoT/HoT)
        public void TickEffect(BaseUnit unit)
        {
            if (tickValue == 0) return;

            if (tickValue > 0)
            {
                unit.Heal((int)tickValue);
                Debug.Log($"{unit.unitName}이 {buffName} 효과로 {tickValue}만큼 회복!");
            }
            else
            {
                unit.TakeDamage(Mathf.Abs(tickValue));
                Debug.Log($"{unit.unitName}이 {buffName} 효과로 {Mathf.Abs(tickValue)}만큼 피해!");
            }
        }

        // 효과 제거 (만료 시)
        public void RemoveEffect(BaseUnit unit)
        {
            unit.damageIncreased -= attackBoost;
            unit.damageReduction += defenseBoost;
            Debug.Log($"{unit.unitName}의 {buffName} 효과 만료.");
        }
    }
}
