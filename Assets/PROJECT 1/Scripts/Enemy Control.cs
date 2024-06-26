using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class EnemyControl : MonoBehaviour
    {
        Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Weapon")
            {
                animator.SetTrigger("Trigger Hit");
            }
        }
    }
}
