using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class EnemyAttackBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MeleeEnemyControl enemyControl = animator.transform.root.GetComponent<MeleeEnemyControl>();
            enemyControl.OnNotifiedAttackFinish();
            enemyControl.OnNotifiedSkillAttackFinish();
        }
    }
}
