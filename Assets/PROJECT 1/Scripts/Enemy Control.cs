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

        public float maxHealth; // 최대 체력
        public float curHealth; // 현재 체력
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
            // T를 눌렀을때 공격중이 아니라면
            if (Input.GetKeyDown(KeyCode.T) && !enemyAttacking && !enemySkillAttacking)
            {
                enemyAttackMove = true;
                enemyAttacking = true;
                animator.SetFloat("EnemySpeed", 1);
                isAttackExecuted = false;
                enemyAttackPower = 30f;
            }

            // 공격하러 이동중이라면 (기본공격)
            if (enemyAttackMove)
            {
                // 공격이 안끝났으면 타겟의 위치로 이동
                if (!isAttackExecuted)
                {
                    transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition.transform.position, moveSpeed * Time.deltaTime);
                }
                // 공격이 끝났으면 원래있던 위치로 이동
                else
                {
                    transform.position = Vector3.MoveTowards(gameObject.transform.position, resetPosition.transform.position, moveSpeed * Time.deltaTime);
                }

                // 공격하러 이동하는 중에, 목표 지점에 도착을 했는지 검사해서 => 도착했다면 애니메이션 Speed:0, 공격 액션 수행
                if (transform.position == targetPosition.transform.position && enemyAttackMove)
                {
                    animator.SetFloat("EnemySpeed", 0);
                    enemyAttackMove = false;
                    animator.SetTrigger("Trigger EnemyAttack");
                }
            }

            // 플레이어가 원래있던 위치로 이동했으면 캐릭터의 로테이션값 정방향으로 변경하고 애니메이션 Speed:0
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

        // 공격 애니메이터가 끝나고 나가면 실행
        public void OnNotifiedAttackFinish()
        {
            if (enemyAttacking)
            {
                // To do : 여기에 들어왔다는 뜻은 애니메이터에서 공격 모션이 끝났음을 알려주었다는 것.
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
    }
}
