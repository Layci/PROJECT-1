using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace ProJect1
{
    public class AnimationManager : MonoBehaviour
    {
        public void MeleeAttack()
        {
            OnTriggerTarget.instance.targetObject.SendMessage("TakeDamage", MeleeCharacterControl.instance.attackPower);
        }


        public void EnemyMeleeAttack()
        {
            //BaseCharacterControl.
        }
    }
}
