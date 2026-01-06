using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class PartyHPUIController : MonoBehaviour
    {
        [SerializeField] private PartyHPUISlot[] slots;

        private void Start()
        {
            Refresh();
        }

        public void Refresh()
        {
            var party = PartyFormationManager.Instance.currentParty;

            for (int i = 0; i < slots.Length; i++)
            {
                if (i < party.Count)
                {
                    slots[i].gameObject.SetActive(true);
                    slots[i].Bind(party[i]);
                }
                else
                {
                    slots[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
