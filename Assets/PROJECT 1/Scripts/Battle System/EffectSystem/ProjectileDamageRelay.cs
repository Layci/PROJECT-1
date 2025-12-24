using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProJect1
{
    public class ProjectileDamageRelay : MonoBehaviour
    {
        private float damage;
        private BaseUnit target;
        private bool hasHit;
        private RFX1_TransformMotion motion;
        private GameObject effectRoot;

        void Awake()
        {
            motion = GetComponent<RFX1_TransformMotion>();
        }

        void OnEnable()
        {
            hasHit = false;
            if (motion != null)
                motion.CollisionEnter += OnCollision;
        }

        void OnDisable()
        {
            if (motion != null)
                motion.CollisionEnter -= OnCollision;
        }

        public void Init(BaseUnit target, float damage, GameObject effectRoot, EffectAsset asset)
        {
            this.target = target;
            this.damage = damage;
            this.effectRoot = effectRoot;

            if (motion == null || target == null)
                return;

            // 1. 이전 타겟 완전히 끊기 (중요)
            motion.Target = null;

            // 2. 새 타겟의 "피벗"을 가져온다
            Transform pivot = target.GetEffectTargetPivot(asset.spawnType);

            if (pivot != null)
            {
                motion.Target = pivot.gameObject;
            }
        }

        private void OnCollision(object sender, RFX1_TransformMotion.RFX1_CollisionInfo info)
        {
            if (hasHit) return;
            hasHit = true;

            if (target == null)
                return;

            float finalDamage = damage * target.damageReduction;
            target.TakeDamage(finalDamage);

            DamageTextSpawner.Instance?.SpawnDamageText(
                target.transform.position + Vector3.up * 1.5f,
                (int)finalDamage
            );

            // 풀로 돌리지 말고 제거
            Destroy(effectRoot, 0.1f);
        }
    }
}
