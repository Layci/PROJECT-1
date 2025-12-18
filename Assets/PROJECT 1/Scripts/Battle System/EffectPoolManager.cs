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
            public EffectType type;
            public EffectBase prefab;
            public int size = 5;
        }

        public List<Pool> pools;
        private Dictionary<EffectType, Queue<EffectBase>> poolDict;

        private void Awake()
        {
            Instance = this;
            poolDict = new Dictionary<EffectType, Queue<EffectBase>>();

            foreach (var pool in pools)
            {
                Queue<EffectBase> q = new Queue<EffectBase>();
                for (int i = 0; i < pool.size; i++)
                {
                    var obj = Instantiate(pool.prefab, transform);
                    obj.gameObject.SetActive(false);
                    q.Enqueue(obj);
                }
                poolDict.Add(pool.type, q);
            }
        }

        public EffectBase Get(EffectType type)
        {
            var q = poolDict[type];
            var effect = q.Dequeue();
            q.Enqueue(effect);
            return effect;
        }
    }
}
