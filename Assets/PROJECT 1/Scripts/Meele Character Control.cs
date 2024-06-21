using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace ProJect1
{
    public class MeeleCharacterControl : MonoBehaviour
    {
        Animator animator;
        public GameObject targetPosition;
        public GameObject resetPosition;

        public float moveSpeed = 0f;
        public bool attackMove = false;
        public bool attacking = false;
        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            //Vector3 playerPos = transform.position;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && !attacking)
            {
                attackMove = true;
                attacking = true;
                animator.SetFloat("Speed", 1);
            }
            if (attackMove)
            {
                transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition.transform.position, moveSpeed * Time.deltaTime);

                if(transform.position == targetPosition.transform.position)
                {
                    animator.SetFloat("Speed", 0);
                    attackMove = false;

                    animator.SetTrigger("Trigger Attack");
                    attacking = false;
                }
            }
        }

        public void ResetPos()
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, resetPosition.transform.position, moveSpeed * Time.deltaTime);
        }
    }
}
