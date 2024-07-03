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
            linkedCharacter.onDamageCallback += RefreshHPBar; // Delegate�� Chain(ü��)�� �Ǵ�.
            linkedCharacter.onDamagedAction += RefreshHPBar; // Delegate�� Chin(ü��)�� �Ǵ�
        }

        public void RefreshHPBar(float currentHp, float maxHp)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
}
