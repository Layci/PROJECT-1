using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProJect1.BaseUnit;

namespace ProJect1
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void PlayAttackEffect(BaseUnit attacker, List<BaseEnemyControl> targets, bool isSkill, int range)
        {
            // 1. 공격자가 사용하는 EffectAsset 선택
            EffectAsset effectAsset = isSkill ? attacker.skillAttackEffect : attacker.normalAttackEffect;

            if (effectAsset == null)
                return;

            // 2. 범위 공격 판정
            bool isAOE = range > 0;

            if (!isAOE)
            {
                PlaySingle(targets, effectAsset);
                return;
            }

            if (effectAsset.isAOECenter)
            {
                PlayAOECenter(targets[0], effectAsset);
            }
            else
            {
                PlayAOEMulti(targets, effectAsset);
            }
        }

        // 단일
        private void PlaySingle(List<BaseEnemyControl> targets, EffectAsset asset)
        {
            foreach (var target in targets)
            {
                var effect = EffectPoolManager.Instance.Get(asset.effectPrefab);

                effect.transform.position = target.transform.position;
                effect.Play(target.transform.position);
            }
        }

        // 타겟 마다
        private void PlayAOEMulti(List<BaseEnemyControl> targets, EffectAsset asset)
        {
            foreach (var enemy in targets)
            {
                var effect = EffectPoolManager.Instance.Get(asset.effectPrefab);
                effect.Play(enemy.transform.position);
            }
        }

        // 타겟 중심 적
        private void PlayAOECenter(BaseEnemyControl centerEnemy, EffectAsset asset)
        {
            var effect = EffectPoolManager.Instance.Get(asset.effectPrefab);
            effect.Play(centerEnemy.transform.position);
        }
    }
}
