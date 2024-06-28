using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ProJect1
{
    public class MeleeAttackBehaviour : StateMachineBehaviour
    {

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MeleeCharacterControl characterControl = animator.transform.root.GetComponent<MeleeCharacterControl>();
            characterControl.OnNotifiedAttackFinish();
            characterControl.OnNotifiedSkillAttackFinish();
        }
    }
}
