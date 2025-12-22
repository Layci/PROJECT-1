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

        private void PlayProjectile(BaseUnit attacker, List<BaseUnit> targets, EffectAsset asset)
        {
            if (asset.isAOECenter)
            {
                var effect = EffectPoolManager.Instance.Get(asset.effectPrefab);
                if (effect == null) return;

                var rfx = effect.GetComponent<RFX1_Target>();
                rfx.Target = targets[0].gameObject;

                effect.transform.position = attacker.transform.position + asset.offset;
                effect.gameObject.SetActive(true);
            }
            else
            {
                foreach (var target in targets)
                {
                    var effect = EffectPoolManager.Instance.Get(asset.effectPrefab);
                    if (effect == null) continue;

                    var rfx = effect.GetComponent<RFX1_Target>();
                    rfx.Target = target.gameObject;

                    effect.transform.position = attacker.transform.position + asset.offset;
                    effect.gameObject.SetActive(true);
                }
            }
        }

        public void PlayAttackEffect(BaseUnit attacker, List<BaseUnit> targets, bool isSkill, int range)
        {
            // 1. 공격자가 사용하는 EffectAsset 선택
            EffectAsset effectAsset = isSkill ? attacker.skillAttackEffect : attacker.normalAttackEffect;

            if (effectAsset == null)
                return;

            // 2. 범위 공격 판정
            bool isAOE = range > 0;

            if (effectAsset.isProjectile)
            {
                PlayProjectile(attacker, targets, effectAsset);
            }

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
        private void PlaySingle(List<BaseUnit> targets, EffectAsset asset)
        {
            foreach (var target in targets)
            {
                var effect = EffectPoolManager.Instance.Get(asset.effectPrefab);

                Vector3 pos = GetEffectSpawnPosition(target, asset);
                effect.Play(pos);
            }
        }

        // 타겟 마다
        private void PlayAOEMulti(List<BaseUnit> targets, EffectAsset asset)
        {
            foreach (var enemy in targets)
            {
                var effect = EffectPoolManager.Instance.Get(asset.effectPrefab);
                //effect.Play(enemy.transform.position);
                Vector3 pos = GetEffectSpawnPosition(enemy, asset);
                effect.Play(pos);
            }
        }

        // 타겟 중심 적
        private void PlayAOECenter(BaseUnit centerEnemy, EffectAsset asset)
        {
            var effect = EffectPoolManager.Instance.Get(asset.effectPrefab);
            Vector3 pos = GetEffectSpawnPosition(centerEnemy, asset);
            effect.Play(pos);
        }

        private Vector3 GetEffectSpawnPosition(BaseUnit enemy, EffectAsset asset)
        {
            switch (asset.spawnType)
            {
                case EffectSpawnType.Ground:
                    return enemy.transform.position + asset.offset;

                case EffectSpawnType.Center:
                    return enemy.centerPoint != null
                        ? enemy.centerPoint.position + asset.offset
                        : enemy.transform.position + Vector3.up + asset.offset;

                case EffectSpawnType.Head:
                    return enemy.headPoint != null
                        ? enemy.headPoint.position + asset.offset
                        : enemy.transform.position + Vector3.up * 2f + asset.offset;
            }

            return enemy.transform.position;
        }
    }
}
