using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class PartyHPUISlot : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Image icon;
        [SerializeField] private Slider hpSlider;
        [SerializeField] private Text hpText;

        private PartyMemberData data;

        public void Bind(PartyMemberData member)
        {
            data = member;

            icon.sprite = member.icon;

            hpSlider.maxValue = member.maxHP;
            hpSlider.minValue = 0;

            Refresh();
        }

        public void Refresh()
        {
            if (data == null) return;

            hpSlider.value = data.currentHP;

            // HP 표시
            hpText.text = data.currentHP.ToString();
        }

        private void Update()
        {
            // 전투 → 메인씬 복귀 시 자동 반영
            Refresh();
        }
    }
}
