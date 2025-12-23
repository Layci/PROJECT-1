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

        private void PlayProjectile(BaseUnit attacker, List<BaseUnit> targets, EffectAsset asset, float damage)
        {
            if (asset.isAOECenter)
            {
                var effect = EffectPoolManager.Instance.Get(asset.effectPrefab);
                if (effect == null) return;

                effect.transform.position = attacker.transform.position + asset.offset;

                // 1. TransformMotion 직접 세팅
                var motion = effect.GetComponentInChildren<RFX1_TransformMotion>();
                if (motion != null)
                    motion.Target = targets[0].gameObject;

                // 2. DamageRelay 초기화
                var relay = effect.GetComponentInChildren<ProjectileDamageRelay>();
                if (relay != null)
                    relay.Init(targets[0], damage); // damage는 외부에서 계산해서 넘겨도 됨

                effect.gameObject.SetActive(true);
            }
            else
            {
                foreach (var target in targets)
                {
                    var effect = EffectPoolManager.Instance.Get(asset.effectPrefab);
                    if (effect == null) continue;

                    effect.transform.position = attacker.transform.position + asset.offset;

                    var motion = effect.GetComponentInChildren<RFX1_TransformMotion>();
                    if (motion != null)
                        motion.Target = target.gameObject;

                    var relay = effect.GetComponentInChildren<ProjectileDamageRelay>();
                    if (relay != null)
                        relay.Init(target, damage);

                    effect.gameObject.SetActive(true);
                }
            }
        }
        /*private void PlayProjectile(BaseUnit attacker, List<BaseUnit> targets, EffectAsset asset, float damage)
        {
            if (asset.isAOECenter)
            {
                var effect = EffectPoolManager.Instance.Get(asset.effectPrefab);
                if (effect == null) return;

                effect.transform.position = attacker.transform.position + asset.offset;

                var rfx = effect.GetComponent<RFX1_Target>();
                if (rfx != null)
                    rfx.Target = targets[0].gameObject;

                var relay = effect.GetComponent<ProjectileDamageRelay>();
                if (relay != null)
                    relay.Init(targets[0], damage);

                //effect.transform.position = attacker.transform.position + asset.offset;
                effect.gameObject.SetActive(true);
            }
            else
            {
                foreach (var target in targets)
                {
                    var effect = EffectPoolManager.Instance.Get(asset.effectPrefab);
                    if (effect == null) continue;

                    effect.transform.position = attacker.transform.position + asset.offset;

                    var rfxTarget = effect.GetComponent<RFX1_Target>();
                    if (rfxTarget != null)
                        rfxTarget.Target = target.gameObject;

                    var relay = effect.GetComponent<ProjectileDamageRelay>();
                    if (relay != null)
                        relay.Init(target, damage); // ← OnAttackEvent에서 계산한 값 전달해도 됨
                    Debug.Log(damage);
                    effect.gameObject.SetActive(true);

                    *//*var rfx = effect.GetComponent<RFX1_Target>();
                    rfx.Target = target.gameObject;

                    effect.transform.position = attacker.transform.position + asset.offset;
                    effect.gameObject.SetActive(true);*//*
                }
            }
        }*/

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
