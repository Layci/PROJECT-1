using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Project1
{
    public abstract class BaseCharacterControl : MonoBehaviour
    {
        protected Animator animator;
        protected Vector3 initialPosition;
        protected Quaternion initialRotation;

        public Slider hpBarSlider;
        public GameObject targetPosition;

        [Header("캐릭터 정보")]
        public float maxHealth;
        public float curHealth;
        public float moveSpeed;
        public float attackPower;
        public float unitSpeed;

        [Header("캐릭터 움직임")]
        public bool attackMove = false;
        public bool attacking = false;
        public bool skillAttackMove = false;
        public bool skillAttacking = false;
        protected bool isAttackExecuted = false;

        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        protected virtual void Update()
        {
            HandleAttackInput();
            HandleMovement();
        }

        protected virtual void HandleAttackInput()
        {
            // Implement in child classes
        }

        protected virtual void HandleMovement()
        {
            // Implement in child classes
        }

        public void CheckHP()
        {
            if (hpBarSlider != null)
            {
                hpBarSlider.value = curHealth / maxHealth;
            }
        }

        public void TakeDamage(float damage)
        {
            if (maxHealth == 0 || curHealth == 0)
                return;

            animator.SetTrigger("Trigger Hit");

            curHealth -= damage;

            CheckHP();

            if (curHealth <= 0)
            {
                Debug.Log("죽음");
            }
            Debug.Log("hit");
        }

        public IEnumerator ExecuteAction()
        {
            yield return new WaitForSeconds(2f);
        }
    }
}