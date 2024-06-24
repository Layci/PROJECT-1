using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class AttackBehaviour : StateMachineBehaviour
    {

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MeeleCharacterControl characterControl = animator.transform.root.GetComponent<MeeleCharacterControl>();
            characterControl.OnNotifiedAttackFinish();
        }
    }
}
