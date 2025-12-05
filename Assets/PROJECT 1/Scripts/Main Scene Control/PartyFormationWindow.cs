using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProJect1
{
    public class PartyFormationWindow : MonoBehaviour
    {
        public static PartyFormationWindow Instance;

        public PartySlotUI[] slots;

        private void Awake()
        {
            Instance = this;
            RefreshUI();
        }

        // 캐릭터 아이콘 누르면 가장 먼저 비어있는 슬롯에 자동 배치
        public void AddToFirstEmptySlot(PartyMemberData data)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].IsEmpty())
                {
                    slots[i].SetData(data);
                    PartyFormationManager.Instance.SetSlot(i, data);
                    return;
                }
            }
        }

        // 슬롯 제거
        public void RemoveSlot(int index)
        {
            slots[index].SetData(null);
            PartyFormationManager.Instance.RemoveSlot(index);
            RefreshUI();
        }

        // 드래그앤드롭 슬롯 교환
        public void SwapSlots(PartySlotUI a, PartySlotUI b)
        {
            var temp = a.currentData;
            a.SetData(b.currentData);
            b.SetData(temp);

            PartyFormationManager.Instance.SetSlot(a.slotIndex, a.currentData);
            PartyFormationManager.Instance.SetSlot(b.slotIndex, b.currentData);
        }

        // 드롭할 때 어떤 슬롯 위에 있는지 확인
        public PartySlotUI GetHoveredSlot(PointerEventData eventData)
        {
            foreach (var slot in slots)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(
                    slot.GetComponent<RectTransform>(),
                    eventData.position))
                {
                    return slot;
                }
            }
            return null;
        }

        // UI 전체 갱신
        public void RefreshUI()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < PartyFormationManager.Instance.currentParty.Count)
                    slots[i].SetData(PartyFormationManager.Instance.currentParty[i]);
                else
                    slots[i].SetData(null);
            }
        }
    }
}
