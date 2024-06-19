using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ProJect1
{
    public class MeeleCharacterControl : MonoBehaviour
    {
        Animator animator;
        public GameObject targetEnemy;

        public float moveSpeed = 1f;

        bool attackMove = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {

                Vector3 targetMove = targetEnemy.transform.position + transform.position;

                targetMove.Normalize();

                transform.position += targetMove * moveSpeed * Time.deltaTime;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.SetTrigger("Trigger Attack");
            }
        }
    }
}
