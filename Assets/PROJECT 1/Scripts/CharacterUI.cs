using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class CharacterUI : MonoBehaviour
    {
        public Image hpBar;

        public CharacterBase linkedCharacter;

        private void Start()
        {
            linkedCharacter.onDamageCallback += RefreshHPBar; // Delegate에 Chain(체인)을 건다.
            linkedCharacter.onDamagedAction += RefreshHPBar; // Delegate에 Chin(체인)을 건다
        }

        public void RefreshHPBar(float currentHp, float maxHp)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
}
