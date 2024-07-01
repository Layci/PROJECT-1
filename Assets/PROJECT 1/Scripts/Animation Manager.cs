using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace ProJect1
{
    public class AnimationManager : MonoBehaviour
    {
        public void MeeleAttackStart()
        {
            MeleeCharacterControl.instance.gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
        }

        public void EnemyMeeleAttackStart()
        {
            EnemyControl.instance.gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
        }

        public void MeeleAttackEnd()
        {
            MeleeCharacterControl.instance.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
        }

        public void EnemyMeeleAttackEnd()
        {
            EnemyControl.instance.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
        }
    }
}
