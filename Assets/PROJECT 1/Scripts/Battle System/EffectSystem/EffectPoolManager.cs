using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class EffectPoolManager : MonoBehaviour
    {
        public static EffectPoolManager Instance;

        private Dictionary<EffectBase, Queue<EffectBase>> poolDict;

        private void Awake()
        {
            Instance = this;
            poolDict = new Dictionary<EffectBase, Queue<EffectBase>>();
        }

        public void RegisterEffect(EffectAsset asset)
        {
            if (asset == null || asset.effectPrefab == null)
                return;

            if (poolDict.ContainsKey(asset.effectPrefab))
                return; // ¿ÃπÃ µÓ∑œµ 

            Queue<EffectBase> q = new Queue<EffectBase>();

            for (int i = 0; i < asset.poolSize; i++)
            {
                var obj = Instantiate(asset.effectPrefab, transform);
                obj.gameObject.SetActive(false);
                q.Enqueue(obj);
            }

            poolDict.Add(asset.effectPrefab, q);
        }

        public EffectBase Get(EffectBase prefab)
        {
            if (!poolDict.ContainsKey(prefab))
            {
                Debug.LogError($"No pool found for prefab: {prefab.name}");
                return null;
            }

            var q = poolDict[prefab];
            var effect = q.Dequeue();
            q.Enqueue(effect);
            return effect;
        }
    }
}
