using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VRTRPG.Command
{
    public class CommandSlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] Transform currentDragDropTransform;
        [SerializeField] DragDrop currentDragDrop;

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                if (eventData.pointerDrag.TryGetComponent<DragDrop>(out DragDrop dragDrop))
                {
                    SwapDragDrop(dragDrop);
                }
            }
        }

        private void SwapDragDrop(DragDrop dragDrop)
        {
            CommandSystem.Instance.SwapCommandUnitsInList(dragDrop.transform.parent.GetSiblingIndex(), transform.GetSiblingIndex());
            currentDragDrop.ChangeSlot(dragDrop.transform.parent);
            dragDrop.ChangeSlot(transform);
        }

        internal void SetDragDrop(DragDrop dragDrop)
        {
            currentDragDrop = dragDrop;
            currentDragDropTransform = dragDrop.transform;
        }

        public DragDrop GetCurrentDragDrop()
        {
            return currentDragDrop;
        }
    }
}
