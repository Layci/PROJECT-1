using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class EffectPoolManager : MonoBehaviour
    {
        public static EffectPoolManager Instance;

        [System.Serializable]
        public class Pool
        {
            public EffectBase prefab;
            public int size = 5;
        }

        public List<Pool> pools;
        private Dictionary<EffectBase, Queue<EffectBase>> poolDict;

        private void Awake()
        {
            Instance = this;
            poolDict = new Dictionary<EffectBase, Queue<EffectBase>>();

            foreach (var pool in pools)
            {
                Queue<EffectBase> q = new Queue<EffectBase>();

                for (int i = 0; i < pool.size; i++)
                {
                    var obj = Instantiate(pool.prefab, transform);
                    obj.gameObject.SetActive(false);
                    q.Enqueue(obj);
                }

                poolDict.Add(pool.prefab, q);
            }
        }

        public EffectBase Get(EffectBase prefab)
        {
            if (!poolDict.TryGetValue(prefab, out var q))
            {
                Debug.LogError($"No pool found for prefab: {prefab.name}");
                return null;
            }

            var effect = q.Dequeue();
            q.Enqueue(effect);
            return effect;
        }
    }
}
