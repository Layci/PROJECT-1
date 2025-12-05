using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProJect1
{
    public class PartySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public int slotIndex;
        public Image icon;
        public Text nameText;

        public PartyMemberData currentData;

        RectTransform rectTransform;
        Canvas canvas;
        CanvasGroup canvasGroup;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = FindObjectOfType<Canvas>();
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        public void SetData(PartyMemberData data)
        {
            currentData = data;

            if (data == null)
            {
                icon.enabled = false;
                nameText.text = "";
                return;
            }

            icon.enabled = true;
            icon.sprite = data.icon;
            nameText.text = data.characterName;
        }

        public bool IsEmpty() => currentData == null;

        // 슬롯 클릭 시 제거
        public void OnPointerClick(PointerEventData eventData)
        {
            if (currentData != null)
            {
                PartyFormationWindow.Instance.RemoveSlot(slotIndex);
            }
        }

        // 드래그 시작
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (currentData == null) return;

            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }

        // 드래그 중
        public void OnDrag(PointerEventData eventData)
        {
            if (currentData == null) return;

            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        // 드래그 종료 → 다른 슬롯에 Dropped 확인
        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

            PartySlotUI hoveredSlot = PartyFormationWindow.Instance.GetHoveredSlot(eventData);

            // 슬롯과 슬롯 교환
            if (hoveredSlot != null && hoveredSlot != this)
            {
                PartyFormationWindow.Instance.SwapSlots(this, hoveredSlot);
            }

            // 원래 위치로 돌아가기
            PartyFormationWindow.Instance.RefreshUI();
        }
    }
}
