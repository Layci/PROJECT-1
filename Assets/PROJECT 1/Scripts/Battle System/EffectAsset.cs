using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    [CreateAssetMenu(menuName = "Effect/Effect Asset")]
    public class EffectAsset : ScriptableObject
    {
        public EffectType effectType;       // Normal / Skill / Heal
        public EffectBase effectPrefab;     // Ω«¡¶ ¿Ã∆Â∆Æ «¡∏Æ∆’
        public bool isAOECenter;
    }
}
