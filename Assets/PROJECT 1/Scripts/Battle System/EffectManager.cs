using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        // 단일 타겟
        public void PlaySingle(EffectType type, Transform target)
        {
            var effect = EffectPoolManager.Instance.Get(type);
            effect.Play(target.position);
        }

        // 범위 공격 (중심 1개만)
        public void PlayAOE(EffectType type, Transform center)
        {
            var effect = EffectPoolManager.Instance.Get(type);
            effect.Play(center.position);
        }

        // 다중 타겟 (히트 이펙트용)
        public void PlayMultiHit(EffectType type, List<BaseEnemyControl> targets)
        {
            foreach (var t in targets)
            {
                var effect = EffectPoolManager.Instance.Get(type);
                effect.Play(t.transform.position);
            }
        }
    }
}
