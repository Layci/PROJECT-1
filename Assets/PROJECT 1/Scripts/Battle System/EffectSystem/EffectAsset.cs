using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    [CreateAssetMenu(menuName = "Effect/Effect Asset")]
    public class EffectAsset : ScriptableObject
    {
        [Header("Effect")]
        public EffectBase effectPrefab;     // 실제 이펙트 프리팹
        //public EffectType effectType;

        [Header("Usage")]
        [Tooltip("(True = 직접 타겟한 적 중심, False = 타겟마다)")]
        public bool isAOECenter;
        [Tooltip("(True = 직접 타겟한 적 중심, False = 타겟마다)")]
        public bool isProjectile;

        [Header("EffectSpawn")]
        public EffectSpawnType spawnType;   // 핵심
        public Vector3 offset;              // 미세 조정용

        [Header("Pooling")]
        public int poolSize = 5;   // 자동 풀 크기
    }
}
