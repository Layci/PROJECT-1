using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class MainPlayerAnimationManager : MonoBehaviour
    {
        public void PlayerAttack()
        {
            MainSenceEnemy mainSenceEnemy = PlayerCombat.instance.currentTarget.GetComponent<MainSenceEnemy>();
            if (mainSenceEnemy != null)
            {
                Debug.Log(mainSenceEnemy.name);
                mainSenceEnemy.OnPlayerEnterBattle();
            }
        }

        public void PlayerAttackEnd()
        {
            MainPlayerControl.instance.isAttacking = false;
        }
    }
}
