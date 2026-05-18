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
        }

        private void OnEnable()
        {
            if (PartyFormationManager.Instance != null)
            {
                PartyFormationManager.Instance.OnPartyCompositionChanged += RefreshUI;
                RefreshUI(); // 창이 열릴 때 초기 데이터 로드
            }
        }

        private void OnDisable()
        {
            if (PartyFormationManager.Instance != null)
            {
                PartyFormationManager.Instance.OnPartyCompositionChanged -= RefreshUI;
            }
        }

        // 캐릭터 아이콘을 클릭했을 때 빈 슬롯에 자동 배치
        public void AddToFirstEmptySlot(PartyMemberData data)
        {
            // 중복 체크 활성화 + 이미 선택된 캐릭터라면 추가하지 않음
            if (PartyFormationManager.Instance.preventDuplicate &&
                PartyFormationManager.Instance.IsCharacterAlreadySelected(data))
            {
                Debug.Log($"중복 캐릭터 {data.characterName} 편성 시도됨.");
                return;
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].IsEmpty())
                {
                    // SetSlot에서 이벤트를 발생시키므로 RefreshUI를 직접 호출할 필요 없음
                    PartyFormationManager.Instance.SetSlot(i, data);
                    return;
                }
            }
        }

        // 슬롯 제거
        public void RemoveSlot(int index)
        {
            // RemoveSlot에서 이벤트를 발생시키므로 RefreshUI를 직접 호출할 필요 없음
            PartyFormationManager.Instance.RemoveSlot(index);
        }

        // 드래그앤드롭으로 슬롯 교환
        public void SwapSlots(PartySlotUI a, PartySlotUI b)
        {
            var temp = a.currentData;

            // SetSlot이 이벤트를 발생시키지만, 두 번 연속 발생하므로 
            // 효율을 위해 Manager 내부 데이터를 직접 수정하고 마지막에 한번만 호출하는 것도 방법이나
            // 일단은 단순하게 유지
            PartyFormationManager.Instance.SetSlot(a.slotIndex, b.currentData);
            PartyFormationManager.Instance.SetSlot(b.slotIndex, temp);
        }

        // ����� �� � ���� ���� �ִ��� Ȯ��
        // 마우스 포인트가 어떤 슬롯 위에 있는지 확인
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
