using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VRTRPG.Action
{
    public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        Canvas canvas;
        RectTransform rectTransform;
        CanvasGroup canvasGroup;
        [SerializeField] TextMeshProUGUI text;

        void Awake()
        {
            canvas = transform.root.GetComponent<Canvas>();
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = .5f;
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            rectTransform.anchoredPosition = Vector2.zero;
        }

        internal void ChangeSlot(Transform slot)
        {
            transform.parent = slot;
            rectTransform.anchoredPosition = Vector2.zero;
            ActionSlot actionSlot = slot.GetComponent<ActionSlot>();
            actionSlot.SetDragDrop(this);
        }

        public void SetText(string name)
        {
            text.text = name;
        }
    }
}
