using Project1;
using System.Collections;
using System.Collections.Generic;
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

        public void Init(BaseUnit target, float damage)
        {
            this.target = target;
            this.damage = damage;
        }

        private void OnCollision(object sender, RFX1_TransformMotion.RFX1_CollisionInfo info)
        {
            if (hasHit) return;
            hasHit = true;

            if (target == null) return;

            Debug.Log("hit");
            float finalDamage = damage * target.damageReduction;
            target.TakeDamage(finalDamage);

            DamageTextSpawner.Instance?.SpawnDamageText(
                target.transform.position + Vector3.up * 1.5f,
                (int)finalDamage
            );
        }
    }
}
