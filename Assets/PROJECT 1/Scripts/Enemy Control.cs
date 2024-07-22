using System;
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
        public GameObject targetResetPosition;
        public GameObject targetPosition;
        public GameObject resetPosition;

        [Header("ĳ���� ����")]
        public float maxHealth; // �ִ� ü��
        public float curHealth; // ���� ü��
        public float moveSpeed = 0f;
        public float enemyAttackPower = 30f;
        public float unitSpeed = 95;

        [Header("ĳ���� ������")]
        public bool enemyAttackMove = false;
        public bool enemyAttacking = false;
        public bool enemySkillAttackMove = false;
        public bool enemySkillAttacking = false;
        private bool actionCompleted;
        private bool isAttackExecuted = false;


        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();

            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            // T�� �������� �������� �ƴ϶��
            if (Input.GetKeyDown(KeyCode.T) && !enemyAttacking && !enemySkillAttacking)
            {
                enemyAttackMove = true;
                enemyAttacking = true;
                animator.SetFloat("Speed", 1);
                isAttackExecuted = false;
                enemyAttackPower = 30f;
            }
            // E�� �������� �������� �ƴ϶��
            if (Input.GetKeyDown(KeyCode.Y) && !enemySkillAttacking && !enemyAttacking)
            {
                enemySkillAttackMove = true;
                enemySkillAttacking = true;
                animator.SetFloat("Speed", 1);
                isAttackExecuted = false;
                enemyAttackPower = 15f;
            }

            // �����Ϸ� �̵����̶�� (�⺻����)
            if (enemyAttackMove)
            {
                // ������ �ȳ������� Ÿ���� ��ġ�� �̵�
                if (!isAttackExecuted)
                {
                    transform.LookAt(targetResetPosition.transform);
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
                    animator.SetFloat("Speed", 0);
                    enemyAttackMove = false;
                    animator.SetTrigger("Trigger EnemyAttack");
                }
            }

            // �����Ϸ� �̵����̶�� (��ų����)
            if (enemySkillAttackMove)
            {
                // ������ �ȳ������� Ÿ���� ��ġ�� �̵�
                if (!isAttackExecuted)
                {
                    transform.LookAt(targetResetPosition.transform);
                    transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition.transform.position, moveSpeed * Time.deltaTime);
                }
                // ������ �������� �����ִ� ��ġ�� �̵�
                else
                {
                    transform.position = Vector3.MoveTowards(gameObject.transform.position, resetPosition.transform.position, moveSpeed * Time.deltaTime);
                }

                // �����Ϸ� �̵��ϴ� �߿�, ��ǥ ������ ������ �ߴ��� �˻��ؼ� => �����ߴٸ� �ִϸ��̼� Speed:0, ��ų���� �׼� ����
                if (transform.position == targetPosition.transform.position && enemySkillAttackMove)
                {
                    animator.SetFloat("Speed", 0);
                    enemySkillAttackMove = false;
                    animator.SetTrigger("Trigger EnemySkillAttack");
                }
            }

            // �÷��̾ �����ִ� ��ġ�� �̵������� ĳ������ �����̼ǰ� ���������� �����ϰ� �ִϸ��̼� Speed:0
            if (transform.position == resetPosition.transform.position)
            {
                animator.SetFloat("Speed", 0);
                transform.rotation = Quaternion.Euler(0, -180, 0);
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
                animator.SetFloat("Speed", 1);
                transform.LookAt(resetPosition.transform);

                //Vector3 dir = (transform.position - resetPosition.transform.position).nor;
                //transform.forward = dir;
            }
        }

        // ��ų���� �ִϸ����Ͱ� ������ ������ ����
        public void OnNotifiedSkillAttackFinish()
        {
            if (enemySkillAttacking)
            {
                // To do : ���⿡ ���Դٴ� ���� �ִϸ����Ϳ��� ��ų���� ����� �������� �˷��־��ٴ� ��.
                isAttackExecuted = true;
                enemySkillAttackMove = true;
                animator.SetFloat("Speed", 1);
                transform.LookAt(resetPosition.transform);
            }
        }

        // ��뿡�� ���� ���
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

            animator.SetTrigger("Trigger EnemyHit");

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

        public IEnumerator ExecuteAction()
        {
            actionCompleted = false;
            Debug.Log("EnemyControl action started.");

            // ���⿡ EnemyControl�� ���� ������ �ڵ带 �ۼ��մϴ�.
            // ��: 1�� ���� ���
            yield return new WaitForSeconds(1f);

            Debug.Log("EnemyControl action completed.");
            actionCompleted = true;
        }
    }
}
