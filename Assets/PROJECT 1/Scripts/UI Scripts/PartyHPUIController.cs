using ProJect1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class PartyHPUIController : MonoBehaviour
    {
        [SerializeField] private PartyHPUISlot[] slots;
        public static PartyHPUIController Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Refresh();
        }

        private void OnEnable()
        {
             // 3. 구독 신청: "파티 바뀌면 내 Refresh 함수를 실행해줘"
             if (PartyFormationManager.Instance != null)
             {
                 PartyFormationManager.Instance.OnPartyCompositionChanged += Refresh;
             }
        }

        private void OnDisable()
        {
             // 4. 구독 해지: 객체가 없어질 때는 알림을 해지해야 에러가 안 납니다.
             if (PartyFormationManager.Instance != null)
             {
                 PartyFormationManager.Instance.OnPartyCompositionChanged -= Refresh;
             }
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
