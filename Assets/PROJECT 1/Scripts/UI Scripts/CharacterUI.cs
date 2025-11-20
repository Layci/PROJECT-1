using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class CharacterUI : MonoBehaviour
    {
        public Image portrait;
        public Slider hpBarSlider;
        public Text buffTurnText;

        private BaseCharacterControl owner; 

        public void Init(BaseCharacterControl character)
        {
            owner = character;

            // Portrait은 캐릭터에 따라 다를 경우 BaseCharacterControl에 sprite 넣어두고 적용
            /*if (portrait != null)
                portrait.sprite = character.unitIcon;*/

            UpdateHP();
            UpdateBuff();
        }

        public void UpdateHP()
        {
            if (hpBarSlider != null)
                hpBarSlider.value = (float)owner.curHealth / owner.maxHealth;
        }

        public void UpdateBuff()
        {
            if (buffTurnText != null)
                buffTurnText.text = owner.buffTrun.ToString();
        }
    }
}
