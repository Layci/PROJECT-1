using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace ProJect1
{
    public class MeeleCharacterControl : MonoBehaviour
    {
        public static MeeleCharacterControl instance;

        Animator animator;
        public GameObject targetPosition;
        public GameObject resetPosition;

        public float moveSpeed = 0f;
        public bool attackMove = false;
        public bool attacking = false;

        private bool isAttackExecuted = false;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            instance = this;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }

        private void Update()
        {
            // Q�� �������� �������� �ƴ϶��
            if (Input.GetKeyDown(KeyCode.Q) && !attacking)
            {
                attackMove = true;
                attacking = true;
                animator.SetFloat("Speed", 1);
                isAttackExecuted = false;
            }

            // �����Ϸ� �̵����̶��
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
                if (transform.position == targetPosition.transform.position)
                {
                    animator.SetFloat("Speed", 0);
                    attackMove = false;

                    animator.SetTrigger("Trigger Attack");
                    attacking = false;
                }
            }

            // �÷��̾ �����ִ� ��ġ�� �̵������� ĳ������ �����̼ǰ� ���������� �����ϰ� �ִϸ��̼� Speed:0
            if (transform.position == resetPosition.transform.position)
            {
                animator.SetFloat("Speed", 0);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        // ���� �ִϸ����Ͱ� ������ ������ ����
        public void OnNotifiedAttackFinish()
        {
            // To do : ���⿡ ���Դٴ� ���� �ִϸ����Ϳ��� ���� ����� �������� �˷��־��ٴ� ��.
            isAttackExecuted = true;
            attackMove = true;
            animator.SetFloat("Speed", 1);
            transform.LookAt(resetPosition.transform);

            
            //Vector3 dir = (transform.position - resetPosition.transform.position).nor;
            //transform.forward = dir;
        }

        private void Attack()
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = true;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}
