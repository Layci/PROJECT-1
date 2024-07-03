using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class MeleeCharacterControl : MonoBehaviour
    {
        public static MeleeCharacterControl instance;

        Animator animator;

        public Slider hpBarSlider;
        public GameObject targetPosition;
        public GameObject resetPosition;

        [Header("캐릭터 정보")]
        public float maxHealth; // 최대 체력
        public float curHealth; // 현재 체력
        public float moveSpeed = 0f;
        public float attackPower = 30f;

        [Header("캐릭터 움직임")]
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
            // Q를 눌렀을때 공격중이 아니라면
            if (Input.GetKeyDown(KeyCode.Q) && !attacking && !skillAttacking)
            {
                attackMove = true;
                attacking = true;
                animator.SetFloat("Speed", 1);
                isAttackExecuted = false;
                attackPower = 30f;
                SkillPointManager.instance.SkillPointUp();
            }

            // 스킬포인트가 남아있을 경우
            if (SkillPointManager.instance.curSkillPoint > 0)
            {
                // E를 눌렀을때 공격중이 아니라면
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
            
            // 공격하러 이동중이라면 (기본공격)
            if (attackMove)
            {
                // 공격이 안끝났으면 타겟의 위치로 이동
                if (!isAttackExecuted)
                {
                    transform.LookAt(targetPosition.transform);
                    transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition.transform.position, moveSpeed * Time.deltaTime);
                }
                // 공격이 끝났으면 원래있던 위치로 이동
                else
                {
                    transform.position = Vector3.MoveTowards(gameObject.transform.position, resetPosition.transform.position, moveSpeed * Time.deltaTime);
                }

                // 공격하러 이동하는 중에, 목표 지점에 도착을 했는지 검사해서 => 도착했다면 애니메이션 Speed:0, 공격 액션 수행
                if (transform.position == targetPosition.transform.position && attackMove)
                {
                    animator.SetFloat("Speed", 0);
                    attackMove = false;
                    animator.SetTrigger("Trigger Attack");
                }
            }

            // 공격하러 이동중이라면 (스킬공격)
            if (skillAttackMove)
            {
                // 공격이 안끝났으면 타겟의 위치로 이동
                if (!isAttackExecuted)
                {
                    transform.LookAt(targetPosition.transform);
                    transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition.transform.position, moveSpeed * Time.deltaTime);
                }
                // 공격이 끝났으면 원래있던 위치로 이동
                else
                {
                    transform.position = Vector3.MoveTowards(gameObject.transform.position, resetPosition.transform.position, moveSpeed * Time.deltaTime);
                }

                // 공격하러 이동하는 중에, 목표 지점에 도착을 했는지 검사해서 => 도착했다면 애니메이션 Speed:0, 스킬공격 액션 수행
                if (transform.position == targetPosition.transform.position && skillAttackMove)
                {
                    animator.SetFloat("Speed", 0);
                    skillAttackMove = false;
                    animator.SetTrigger("Trigger SkillAttack");
                }
            }

            // 플레이어가 원래있던 위치로 이동했으면 캐릭터의 로테이션값 정방향으로 변경하고 애니메이션 Speed:0
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

        // 공격 애니메이터가 끝나고 나가면 실행
        public void OnNotifiedAttackFinish()
        {
            if (attacking)
            {
                // To do : 여기에 들어왔다는 뜻은 애니메이터에서 공격 모션이 끝났음을 알려주었다는 것.
                isAttackExecuted = true;
                attackMove = true;
                animator.SetFloat("Speed", 1);
                transform.LookAt(resetPosition.transform);

                //Vector3 dir = (transform.position - resetPosition.transform.position).nor;
                //transform.forward = dir;
            }
        }

        // 스킬공격 애니메이터가 끝나고 나가면 실행
        public void OnNotifiedSkillAttackFinish()
        {
            if (skillAttacking)
            {
                // To do : 여기에 들어왔다는 뜻은 애니메이터에서 스킬공격 모션이 끝났음을 알려주었다는 것.
                isAttackExecuted = true;
                skillAttackMove = true;
                animator.SetFloat("Speed", 1);
                transform.LookAt(resetPosition.transform);
            }
        }

        // 상대에게 맞을 경우
        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "EnemyMeleeWeapon":
                    animator.SetTrigger("Trigger Hit");
                    TakeDamage(EnemyControl.instance.enemyAttackPower);
                    break;
            }
        }

        // 체력 갱신
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

            // 체력 갱신
            CheckHP();

            // 체력이 0이하라면
            if (curHealth <= 0)
            {
                Debug.Log("죽음");
            }
            Debug.Log("hit");
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
            }
        }
        
    }
}
