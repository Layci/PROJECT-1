using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Project1
{
    public enum PlayerState
    {
        Idle,
        MovingToAttack,
        Attacking,
        Returning
    }

    public abstract class BaseCharacterControl : MonoBehaviour
    {
        public static BaseCharacterControl instance;

        protected Animator animator;
        protected Vector3 initialPosition;
        protected Quaternion initialRotation;


        [Header("ĳ���� ����")]
        public float maxHealth;
        public float curHealth;
        public float moveSpeed;
        public float unitSpeed;
        public float playerAttackPower;
        public float playerSKillAttackPower;
        public float attackRange; // ���� �Ÿ�
        public bool startAttacking;
        public bool skillAttack;
        public Transform enemy; // �� ��ġ ����
        public Slider hpBarSlider; // HP��

        [Header("ĳ���� ������")]
        public PlayerState currentState = PlayerState.Idle; // ���� ���� �߰�
        protected bool isAttackExecuted = false;

        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        protected virtual void Update()
        {
            HandleState();
            HandleAttackInput();
        }

        protected void HandleState()
        {
            switch (currentState)
            {
                case PlayerState.Idle:
                    // Idle ������ ����
                    break;
                case PlayerState.MovingToAttack:
                    MoveToAttack();
                    break;
                case PlayerState.Attacking:
                    PerformAttack();
                    break;
                case PlayerState.Returning:
                    ReturnToInitialPosition();
                    break;
            }
        }

        protected virtual void MoveToAttack()
        {
            if (enemy != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, enemy.position, moveSpeed * Time.deltaTime);
                animator.SetFloat("Speed", 1);

                float distanceToTarget = Vector3.Distance(transform.position, enemy.position);
                if (distanceToTarget <= attackRange)
                {
                    currentState = PlayerState.Attacking;
                }
            }
        }

        protected virtual void PerformAttack()
        {
            if (!isAttackExecuted && !skillAttack)
            {
                // ���� ����
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Trigger Attack");
                isAttackExecuted = true;
            }
            else if (!isAttackExecuted && skillAttack)
            {
                // ��ų ���� ����
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Trigger SkillAttack");
                isAttackExecuted = true;
            }
        }

        protected virtual void ReturnToInitialPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, -180f, 0f);
            animator.SetFloat("Speed", 1);

            if (Vector3.Distance(transform.position, initialPosition) <= 0.1f)
            {
                transform.position = initialPosition; // ��ġ ����
                transform.rotation = initialRotation;
                animator.SetFloat("Speed", 0);
                currentState = PlayerState.Idle;
                isAttackExecuted = false; // ��ġ�� ���ƿ��� ���� ���� ���� �ʱ�ȭ
            }
        }

        protected virtual void HandleAttackInput()
        {
            // �⺻������ �ƹ��͵� ���� ����
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
                Debug.Log("����");
            }
            Debug.Log("hit");
        }

        public IEnumerator ExecuteAction()
        {
            yield return new WaitForSeconds(2f);
        }

        public void StartMove(Transform targetTransform)
        {
            enemy = targetTransform;
            currentState = PlayerState.MovingToAttack;
        }

        public void StartAttack()
        {
            currentState = PlayerState.Attacking;
        }

        public void StopAction()
        {
            currentState = PlayerState.Idle;
        }
    }
}