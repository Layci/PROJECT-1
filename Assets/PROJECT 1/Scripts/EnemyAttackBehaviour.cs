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
            EnemyControl enemyControl = animator.transform.root.GetComponent<EnemyControl>();
            enemyControl.OnNotifiedAttackFinish();
        }
    }
}
