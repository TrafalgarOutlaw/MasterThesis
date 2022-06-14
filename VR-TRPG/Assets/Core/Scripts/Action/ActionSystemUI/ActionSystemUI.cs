using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Action
{
    public class ActionSystemUI : MonoBehaviour
    {
        private ActionSystem actionSystem;
        [SerializeField] Transform actionSystemUIContainer;
        [SerializeField] Transform pfActionSlot;

        Dictionary<AActionUnit, Transform> actionUnitDict = new Dictionary<AActionUnit, Transform>();

        // Start is called before the first frame update
        void Start()
        {
            actionSystem = ActionSystem.Instance;

            actionSystem.OnAddActionUnit.AddListener(CreateActionSlot);
            actionSystem.OnDeleteActionUnit.AddListener(DeleteActionSlot);
            actionSystem.OnActionOrderChanged.AddListener(SwapSlots);
        }

        void CreateActionSlot(AActionUnit actionUnit)
        {
            Transform actionSlotTransform = Instantiate(pfActionSlot, actionSystemUIContainer.position, Quaternion.identity, actionSystemUIContainer);

            actionUnitDict.Add(actionUnit, actionSlotTransform);

            if (actionSlotTransform.TryGetComponent<ActionSlot>(out ActionSlot actionSlot))
            {
                DragDrop dragDrop = actionSlot.GetCurrentDragDrop();
                dragDrop.SetText(actionUnit.actionUnitName);
            }

        }

        void DeleteActionSlot(AActionUnit actionUnit)
        {
            Destroy(actionUnitDict[actionUnit].gameObject);
            actionUnitDict.Remove(actionUnit);
        }

        private void SwapSlots(AActionUnit action1, AActionUnit action2)
        {
            Transform tmp = actionUnitDict[action1];
            actionUnitDict[action1] = actionUnitDict[action2];
            actionUnitDict[action2] = tmp;
        }
    }
}
