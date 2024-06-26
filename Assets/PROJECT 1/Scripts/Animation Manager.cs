using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class AnimationManager : MonoBehaviour
    {
        public void MeeleAttackStart()
        {
            MeeleCharacterControl.instance.gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
        }

        public void MeeleAttackEnd()
        {
            MeeleCharacterControl.instance.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
        }
    }
}
