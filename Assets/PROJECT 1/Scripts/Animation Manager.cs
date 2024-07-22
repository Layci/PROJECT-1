using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace ProJect1
{
    public class AnimationManager : MonoBehaviour
    {
        GameObject hitObject;

        public void MeleeAttack()
        {
            OnTriggerTarget.instance.targetObject.SendMessage("TakeDamage", MeleeCharacterControl.instance.attackPower);
        }

        public void EnemyMeleeAttack()
        {
            OnTriggerTarget.instance.targetObject.SendMessage("TakeDamage", EnemyControl.instance.enemyAttackPower);
        }
    }
}
