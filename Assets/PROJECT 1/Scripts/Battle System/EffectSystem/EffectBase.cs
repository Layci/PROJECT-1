using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class EffectBase : MonoBehaviour
    {
        protected ParticleSystem ps;

        protected virtual void Awake()
        {
            ps = GetComponent<ParticleSystem>();
        }

        public virtual void Play(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);

            ps.Play();
            Invoke(nameof(ReturnToPool), ps.main.duration);
        }

        protected virtual void ReturnToPool()
        {
            gameObject.SetActive(false);
        }
    }
}
