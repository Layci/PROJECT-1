using UnityEngine;

namespace Project1
{
    public class MeleeEnemyControl : BaseEnemyControl
    {
        protected override void Update()
        {
            base.Update();
        }

        public void OnNotifiedAttackFinish()
        {
            if (currentState == EnemyState.Attacking)
            {
                isAttackExecuted = true;
                currentState = EnemyState.Returning;
                animator.SetFloat("Speed", 1);
                transform.LookAt(initialPosition);
            }
        }

        public void OnNotifiedSkillAttackFinish()
        {
            if (currentState == EnemyState.Attacking)
            {
                isAttackExecuted = true;
                currentState = EnemyState.Returning;
                animator.SetFloat("Speed", 1);
                transform.LookAt(initialPosition);
            }
        }
    }
}
