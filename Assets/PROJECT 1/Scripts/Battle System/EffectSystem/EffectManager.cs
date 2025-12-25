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

        private void SpawnProjectile(BaseUnit attacker, BaseUnit target, EffectAsset asset, float damage)
        {
            if (target == null)
                return;

            // Projectile은 풀링하지 않는다
            var effect = Instantiate(asset.effectPrefab);

            // 1. 발사 위치
            Vector3 spawnPos = attacker.GetProjectileSpawnPosition(asset);
            effect.transform.position = spawnPos;
            effect.transform.rotation = Quaternion.identity;

            // 2. 타겟 피벗 설정
            var pivot = target.GetEffectTargetPivot(asset.spawnType);
            var motion = effect.GetComponentInChildren<RFX1_TransformMotion>();
            if (motion != null && pivot != null)
            {
                motion.Target = pivot.gameObject;
            }

            // 3. 데미지 릴레이 초기화
            var damageRelay = effect.GetComponentInChildren<ProjectileDamageRelay>();
            if (damageRelay != null)
            {
                damageRelay.Init(target, damage, effect.gameObject, asset);
            }

            effect.gameObject.SetActive(true);
        }

        private void PlayProjectile(BaseUnit attacker, List<BaseUnit> targets, EffectAsset asset, float damage)
        {
            if (targets == null || targets.Count == 0)
                return;

            if (asset.isAOECenter)
            {
                SpawnProjectile(attacker, targets[0], asset, damage);
            }
            else
            {
                foreach (var target in targets)
                {
                    SpawnProjectile(attacker, target, asset, damage);
                }
            }
        }

        public void PlayAttackEffect(BaseUnit attacker, List<BaseUnit> targets, bool isSkill, int range, float damage)
        {
            // 1. 공격자가 사용하는 EffectAsset 선택
            EffectAsset effectAsset = isSkill ? attacker.skillAttackEffect : attacker.normalAttackEffect;

            if (effectAsset == null)
                return;

            // 2. 범위 공격 판정
            bool isAOE = range > 0;

            if (effectAsset.isProjectile)
            {
                PlayProjectile(attacker, targets, effectAsset, damage);
                return;
            }

            if (!isAOE)
            {
                PlaySingle(targets, effectAsset);
                Debug.Log("PlaySingle");
                return;
            }

            if (effectAsset.isAOECenter)
            {
                PlayAOECenter(targets[0], effectAsset);
                Debug.Log("PlayAOECenter");
            }
            else
            {
                PlayAOEMulti(targets, effectAsset);
                Debug.Log("PlayAOEMulti");
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
