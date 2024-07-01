using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class ButtonManager : MonoBehaviour
    {
        public void OnClickAttackBtn()
        {
            MeleeCharacterControl.instance.OnClickAttackBtn();
        }

        public void OnClickSkillAttackBtn()
        {
            MeleeCharacterControl.instance.OnClickSkillAttackBtn();
        }
    }
}
