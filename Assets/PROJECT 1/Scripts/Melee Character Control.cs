using UnityEngine;
using Project1;

namespace ProJect1
{
    public class MeleeCharacterControl : BaseCharacterControl
    {
        public static MeleeCharacterControl instance;

        protected override void Awake()
        {
            base.Awake();

            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected override void HandleAttackInput()
        {
            if (Input.GetKeyDown(KeyCode.Q) && !attacking && !skillAttacking)
            {
                attackMove = true;
                attacking = true;
                animator.SetFloat("Speed", 1);
                isAttackExecuted = false;
                attackPower = 30f;
                SkillPointManager.instance.SkillPointUp();
            }

            if (SkillPointManager.instance.curSkillPoint > 0)
            {
                if (Input.GetKeyDown(KeyCode.E) && !skillAttacking && !attacking)
                {
                    skillAttackMove = true;
                    skillAttacking = true;
                    animator.SetFloat("Speed", 1);
                    isAttackExecuted = false;
                    attackPower = 15f;
                    SkillPointManager.instance.UseSkillPoint();
                }
            }
        }

        protected override void HandleMovement()
        {
            if (attackMove)
            {
                if (!isAttackExecuted)
                {
                    transform.LookAt(targetPosition.transform);
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition.transform.position, moveSpeed * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
                }

                if (transform.position == targetPosition.transform.position && attackMove)
                {
                    animator.SetFloat("Speed", 0);
                    attackMove = false;
                    animator.SetTrigger("Trigger Attack");
                }
            }

            if (skillAttackMove)
            {
                if (!isAttackExecuted)
                {
                    transform.LookAt(targetPosition.transform);
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition.transform.position, moveSpeed * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
                }

                if (transform.position == targetPosition.transform.position && skillAttackMove)
                {
                    animator.SetFloat("Speed", 0);
                    skillAttackMove = false;
                    animator.SetTrigger("Trigger SkillAttack");
                }
            }

            if (transform.position == initialPosition)
            {
                animator.SetFloat("Speed", 0);
                transform.rotation = initialRotation;
                attacking = false;
                skillAttacking = false;
                attackMove = false;
                skillAttackMove = false;
            }
        }

        public void OnNotifiedAttackFinish()
        {
            if (attacking)
            {
                isAttackExecuted = true;
                attackMove = true;
                animator.SetFloat("Speed", 1);
                transform.LookAt(initialPosition);
            }
        }

        public void OnNotifiedSkillAttackFinish()
        {
            if (skillAttacking)
            {
                isAttackExecuted = true;
                skillAttackMove = true;
                animator.SetFloat("Speed", 1);
                transform.LookAt(initialPosition);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("EnemyMeleeWeapon"))
            {
                animator.SetTrigger("Trigger Hit");
                TakeDamage(BaseEnemyControl.instance.enemyAttackPower);
            }
        }

        public void OnClickAttackBtn()
        {
            if (!attacking && !skillAttacking)
            {
                attackMove = true;
                attacking = true;
                animator.SetFloat("Speed", 1);
                isAttackExecuted = false;
                attackPower = 30f;
                SkillPointManager.instance.SkillPointUp();
            }
        }

        public void OnClickSkillAttackBtn()
        {
            if (!attacking && !skillAttacking)
            {
                skillAttackMove = true;
                skillAttacking = true;
                animator.SetFloat("Speed", 1);
                isAttackExecuted = false;
                attackPower = 15f;
                SkillPointManager.instance.UseSkillPoint();
            }
        }
    }
}