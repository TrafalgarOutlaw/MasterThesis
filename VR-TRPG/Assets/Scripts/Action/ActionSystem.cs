using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRTRPG.Action
{
    [System.Serializable] public class ActionUnitEvent : UnityEvent<AActionUnit> { }
    [System.Serializable] public class ActionOrderEvent : UnityEvent<AActionUnit, AActionUnit> { }
    public class ActionSystem : MonoBehaviour
    {
        public ActionUnitEvent OnAddActionUnit;
        public ActionUnitEvent OnDeleteActionUnit;
        public ActionOrderEvent OnActionOrderChanged;

        public static ActionSystem Instance { get; private set; }
        List<AActionUnit> actionUnitList = new List<AActionUnit>();
        private int namePostfix = 0;

        private enum State
        {
            PlayerTurn,
            Waiting,
        }

        void Awake()
        {
            // Set Singleton
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            Instance = this;
        }

        internal void RegisterAction(AActionUnit actionUnit)
        {
            actionUnitList.Add(actionUnit);

            OnAddActionUnit.Invoke(actionUnit);
        }

        internal void Deregister(AActionUnit actionUnit)
        {
            actionUnitList.Remove(actionUnit);

            OnDeleteActionUnit.Invoke(actionUnit);
        }

        internal void SwapActionsInList(int i1, int i2)
        {
            OnActionOrderChanged.Invoke(actionUnitList[i1], actionUnitList[i2]);

            AActionUnit tmp = actionUnitList[i1];
            actionUnitList[i1] = actionUnitList[i2];
            actionUnitList[i2] = tmp;
        }

        public void DoNextAction()
        {
            if (actionUnitList.Count == 0) return;

            actionUnitList[0].DoAction();
        }

        public void EndAction()
        {
            OnDeleteActionUnit.Invoke(actionUnitList[0]);
            actionUnitList.RemoveAt(0);
        }
    }
}
