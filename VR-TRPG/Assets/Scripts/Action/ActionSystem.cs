using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRTRPG.Action
{
    [System.Serializable] public class ActionListEvent : UnityEvent<IActionable> { }
    [System.Serializable] public class ActionOrderEvent : UnityEvent<IActionable, IActionable> { }
    public class ActionSystem : MonoBehaviour
    {
        public ActionListEvent OnActionListChanged;
        public ActionOrderEvent OnActionOrderChanged;

        public static ActionSystem Instance { get; private set; }
        List<IActionable> actionList = new List<IActionable>();
        private int namePostfix = 0;

        void Awake()
        {
            // Set Singleton
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            Instance = this;
        }

        internal void RegisterAction(IActionable action)
        {
            actionList.Add(action);
            action.SetActionName(action.GetActionName() + namePostfix.ToString());
            namePostfix++;

            OnActionListChanged.Invoke(action);
        }

        internal void Deregister(IActionable action)
        {
            actionList.Remove(action);

            OnActionListChanged.Invoke(action);
        }

        internal void SwapActionsInList(int i1, int i2)
        {
            OnActionOrderChanged.Invoke(actionList[i1], actionList[i2]);

            IActionable tmp = actionList[i1];
            actionList[i1] = actionList[i2];
            actionList[i2] = tmp;
        }
    }
}
