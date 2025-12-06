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

        private GameObject dragIcon;
        private RectTransform dragIconRect;
        //RectTransform rectTransform;
        Canvas canvas;
        //CanvasGroup canvasGroup;

        private void Awake()
        {
            //rectTransform = GetComponent<RectTransform>();
            canvas = FindObjectOfType<Canvas>();
            //canvasGroup = gameObject.AddComponent<CanvasGroup>();
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

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (currentData == null) return;

            // 드래그 아이콘 생성
            dragIcon = new GameObject("DragIcon");
            dragIcon.transform.SetParent(canvas.transform);

            dragIconRect = dragIcon.AddComponent<RectTransform>();
            dragIconRect.sizeDelta = icon.rectTransform.sizeDelta;

            Image img = dragIcon.AddComponent<Image>();
            img.sprite = icon.sprite;
            img.raycastTarget = false; // 드래그 아이콘은 레이캐스트 방해하지 않음
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (dragIconRect == null) return;

            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out pos
            );

            dragIconRect.anchoredPosition = pos;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Destroy(dragIcon);

            var hoveredSlot = PartyFormationWindow.Instance.GetHoveredSlot(eventData);

            if (hoveredSlot != null && hoveredSlot != this)
            {
                PartyFormationWindow.Instance.SwapSlots(this, hoveredSlot);
            }
        }
    }
}
