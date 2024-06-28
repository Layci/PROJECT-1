using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class EnemyControl : MonoBehaviour
    {
        public static EnemyControl instance;

        Animator animator;
        public Slider hpBarSlider;
        public GameObject targetPosition;
        public GameObject resetPosition;

        public float maxHealth; // �ִ� ü��
        public float curHealth; // ���� ü��
        public float moveSpeed = 0f;
        public float enemyAttackPower = 30f;
        public bool enemyAttackMove = false;
        public bool enemyAttacking = false;
        public bool enemySkillAttackMove = false;
        public bool enemySkillAttacking = false;

        private bool isAttackExecuted = false;


        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            instance = this;
        }

        private void Update()
        {
            // T�� �������� �������� �ƴ϶��
            if (Input.GetKeyDown(KeyCode.T) && !enemyAttacking && !enemySkillAttacking)
            {
                enemyAttackMove = true;
                enemyAttacking = true;
                animator.SetFloat("EnemySpeed", 1);
                isAttackExecuted = false;
                enemyAttackPower = 30f;
            }

            // �����Ϸ� �̵����̶�� (�⺻����)
            if (enemyAttackMove)
            {
                // ������ �ȳ������� Ÿ���� ��ġ�� �̵�
                if (!isAttackExecuted)
                {
                    transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition.transform.position, moveSpeed * Time.deltaTime);
                }
                // ������ �������� �����ִ� ��ġ�� �̵�
                else
                {
                    transform.position = Vector3.MoveTowards(gameObject.transform.position, resetPosition.transform.position, moveSpeed * Time.deltaTime);
                }

                // �����Ϸ� �̵��ϴ� �߿�, ��ǥ ������ ������ �ߴ��� �˻��ؼ� => �����ߴٸ� �ִϸ��̼� Speed:0, ���� �׼� ����
                if (transform.position == targetPosition.transform.position && enemyAttackMove)
                {
                    animator.SetFloat("EnemySpeed", 0);
                    enemyAttackMove = false;
                    animator.SetTrigger("Trigger EnemyAttack");
                }
            }

            // �÷��̾ �����ִ� ��ġ�� �̵������� ĳ������ �����̼ǰ� ���������� �����ϰ� �ִϸ��̼� Speed:0
            if (transform.position == resetPosition.transform.position)
            {
                animator.SetFloat("EnemySpeed", 0);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                enemyAttacking = false;
                enemySkillAttacking = false;
                enemyAttackMove = false;
                enemySkillAttackMove = false;
            }
        }

        // ���� �ִϸ����Ͱ� ������ ������ ����
        public void OnNotifiedAttackFinish()
        {
            if (enemyAttacking)
            {
                // To do : ���⿡ ���Դٴ� ���� �ִϸ����Ϳ��� ���� ����� �������� �˷��־��ٴ� ��.
                isAttackExecuted = true;
                enemyAttackMove = true;
                animator.SetFloat("EnemySpeed", 1);
                transform.LookAt(resetPosition.transform);

                //Vector3 dir = (transform.position - resetPosition.transform.position).nor;
                //transform.forward = dir;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "MeleeWeapon":
                    animator.SetTrigger("Trigger EnemyHit");
                    TakeDamage(MeleeCharacterControl.instance.attackPower);
                    break;
            }
        }

        // ü�� ����
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

            curHealth -= damage;

            // ü�� ����
            CheckHP();

            // ü���� 0���϶��
            if (curHealth <= 0)
            {
                Debug.Log("����");
            }
            Debug.Log("hit");
        }
    }
}
