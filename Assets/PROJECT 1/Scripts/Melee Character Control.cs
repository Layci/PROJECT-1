using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace ProJect1
{
    public class MeleeCharacterControl : MonoBehaviour
    {
        public static MeleeCharacterControl instance;

        Animator animator;
        public GameObject targetPosition;
        public GameObject resetPosition;

        public float moveSpeed = 0f;
        public float attackPower = 30f;
        public bool attackMove = false;
        public bool attacking = false;
        public bool skillAttackMove = false;
        public bool skillAttacking = false;

        private bool isAttackExecuted = false;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            instance = this;
        }

        private void Update()
        {
            // Q�� �������� �������� �ƴ϶��
            if (Input.GetKeyDown(KeyCode.Q) && !attacking && !skillAttacking)
            {
                attackMove = true;
                attacking = true;
                animator.SetFloat("Speed", 1);
                isAttackExecuted = false;
                attackPower = 30f;
            }

            // E�� �������� �������� �ƴ϶��
            if (Input.GetKeyDown(KeyCode.E) && !skillAttacking && !attacking)
            {
                skillAttackMove = true;
                skillAttacking = true;
                animator.SetFloat("Speed", 1);
                isAttackExecuted = false;
                attackPower = 15f;
            }
            
            // �����Ϸ� �̵����̶�� (�⺻����)
            if (attackMove)
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
                if (transform.position == targetPosition.transform.position && attackMove)
                {
                    animator.SetFloat("Speed", 0);
                    attackMove = false;
                    animator.SetTrigger("Trigger Attack");
                }
            }

            // �����Ϸ� �̵����̶�� (��ų����)
            if (skillAttackMove)
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

                // �����Ϸ� �̵��ϴ� �߿�, ��ǥ ������ ������ �ߴ��� �˻��ؼ� => �����ߴٸ� �ִϸ��̼� Speed:0, ��ų���� �׼� ����
                if (transform.position == targetPosition.transform.position && skillAttackMove)
                {
                    animator.SetFloat("Speed", 0);
                    skillAttackMove = false;
                    animator.SetTrigger("Trigger SkillAttack");
                }
            }

            // �÷��̾ �����ִ� ��ġ�� �̵������� ĳ������ �����̼ǰ� ���������� �����ϰ� �ִϸ��̼� Speed:0
            if (transform.position == resetPosition.transform.position)
            {
                animator.SetFloat("Speed", 0);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                attacking = false;
                skillAttacking = false;
                attackMove = false;
                skillAttackMove = false;
            }
        }

        // ���� �ִϸ����Ͱ� ������ ������ ����
        public void OnNotifiedAttackFinish()
        {
            if (attacking)
            {
                // To do : ���⿡ ���Դٴ� ���� �ִϸ����Ϳ��� ���� ����� �������� �˷��־��ٴ� ��.
                isAttackExecuted = true;
                attackMove = true;
                animator.SetFloat("Speed", 1);
                transform.LookAt(resetPosition.transform);

                //Vector3 dir = (transform.position - resetPosition.transform.position).nor;
                //transform.forward = dir;
            }
        }

        // ��ų���� �ִϸ����Ͱ� ������ ������ ����
        public void OnNotifiedSkillAttackFinish()
        {
            if (skillAttacking)
            {
                // To do : ���⿡ ���Դٴ� ���� �ִϸ����Ϳ��� ��ų���� ����� �������� �˷��־��ٴ� ��.
                isAttackExecuted = true;
                skillAttackMove = true;
                animator.SetFloat("Speed", 1);
                transform.LookAt(resetPosition.transform);
            }
        }

    }
}
