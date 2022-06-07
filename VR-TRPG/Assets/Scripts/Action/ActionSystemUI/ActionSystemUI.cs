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

        Dictionary<IActionable, Transform> actionDict = new Dictionary<IActionable, Transform>();

        // Start is called before the first frame update
        void Start()
        {
            actionSystem = ActionSystem.Instance;
            print(actionSystem);

            actionSystem.OnActionListChanged.AddListener(UpdateSlots);
            actionSystem.OnActionOrderChanged.AddListener(SwapSlots);
        }

        void UpdateSlots(IActionable action)
        {
            if (actionDict.ContainsKey(action))
            {
                DeleteActionSlot(action);
            }
            else
            {
                CreateActionSlot(action);
            }
        }

        void CreateActionSlot(IActionable action)
        {
            Transform actionSlotTransform = Instantiate(pfActionSlot, actionSystemUIContainer.position, Quaternion.identity, actionSystemUIContainer);

            actionDict.Add(action, actionSlotTransform);

            if (actionSlotTransform.TryGetComponent<ActionSlot>(out ActionSlot actionSlot))
            {
                DragDrop dragDrop = actionSlot.GetCurrentDragDrop();
                dragDrop.SetText(action.GetActionName());
            }
        }

        private void SwapSlots(IActionable action1, IActionable action2)
        {
            Transform tmp = actionDict[action1];
            actionDict[action1] = actionDict[action2];
            actionDict[action2] = tmp;
        }

        void DeleteActionSlot(IActionable action)
        {
            Destroy(actionDict[action].gameObject);
            actionDict.Remove(action);
        }
    }
}
