using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class AnimationManager : MonoBehaviour
    {
        public void MeeleAttackStart()
        {
            MeleeCharacterControl.instance.gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
            EnemyControl.instance.gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
        }

        public void MeeleAttackEnd()
        {
            MeleeCharacterControl.instance.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
            EnemyControl.instance.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
        }
    }
}
