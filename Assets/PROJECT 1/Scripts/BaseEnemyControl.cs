using ProJect1;
using UnityEngine;

namespace Project1
{
    public enum EnemyState
    {
        Idle,
        MovingToAttack,
        Attacking,
        Returning
    }

    public abstract class BaseEnemyControl : MonoBehaviour
    {
        public static BaseEnemyControl instance;

        protected Animator animator;
        protected Vector3 initialPosition;
        protected Quaternion initialRotation;

        [Header("�� ����")]
        public float maxHealth;
        public float curHealth;
        public float moveSpeed;
        public float enemyAttackPower;
        public float unitSpeed;
        public float attackRange; // ���� ���� �߰�
        public Transform player; // �÷��̾� ���� �߰�

        [Header("�� ������")]
        public EnemyState currentState = EnemyState.Idle; // ���� ���� �߰�
        protected bool isAttackExecuted = false;

        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;

            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void Update()
        {
            HandleState();
        }

        protected void HandleState()
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    // �ƹ��͵� ���� ����
                    break;
                case EnemyState.MovingToAttack:
                    MoveToAttack();
                    break;
                case EnemyState.Attacking:
                    PerformAttack();
                    break;
                case EnemyState.Returning:
                    ReturnToInitialPosition();
                    break;
            }
        }

        protected virtual void MoveToAttack()
        {
            if (!isAttackExecuted)
            {
                transform.LookAt(MeleeCharacterControl.instance.transform);
                transform.position = Vector3.MoveTowards(transform.position, MeleeCharacterControl.instance.transform.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                currentState = EnemyState.Returning;
            }
        }

        protected virtual void PerformAttack()
        {
            // ���� ���� ����
            animator.SetFloat("Speed", 0);
            animator.SetTrigger("Trigger EnemyAttack");
            currentState = EnemyState.Returning;
        }

        protected virtual void ReturnToInitialPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);

            if (transform.position == initialPosition)
            {
                animator.SetFloat("Speed", 0);
                transform.rotation = initialRotation;
                currentState = EnemyState.Idle;
            }
        }

        public void TakeDamage(float damage)
        {
            if (maxHealth == 0 || curHealth == 0)
                return;

            animator.SetTrigger("Trigger EnemyHit");

            curHealth -= damage;

            if (curHealth <= 0)
            {
                Debug.Log("�� ���");
                Destroy(gameObject);
            }
            Debug.Log("�� �ǰ�");
        }

        public IEnumerator ExecuteAction()
        {
            yield return new WaitForSeconds(2f);
        }

        public void StartAttack()
        {
            currentState = EnemyState.MovingToAttack;
        }

        public void StopAttack()
        {
            currentState = EnemyState.Idle;
        }
    }
}