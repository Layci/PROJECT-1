using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class CharacterIconUI : MonoBehaviour
    {
        public PartyMemberData data;
        public Button button;

        private void Start()
        {
            button.onClick.AddListener(() =>
            {
                PartyFormationWindow.Instance.AddToFirstEmptySlot(data);
            });
        }
    }
}
