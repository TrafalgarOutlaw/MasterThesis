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
        public List<AActionUnit> actionUnitList = new List<AActionUnit>();
        public AActionUnit CurrentAction { get; private set; }

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

        internal void PushAction(AActionUnit actionUnit)
        {
            actionUnitList.Add(actionUnit);
            OnAddActionUnit.Invoke(actionUnit);
        }

        internal void InsertAction(int index, AActionUnit actionUnit)
        {
            if (index < actionUnitList.Count)
            {
                actionUnitList.Insert(index, actionUnit);
                OnAddActionUnit.Invoke(actionUnit);
            }
            else
            {
                PushAction(actionUnit);
            }
        }

        internal void RemoveAction(AActionUnit actionUnit)
        {
            if (actionUnitList.Remove(actionUnit))
            {
                OnDeleteActionUnit.Invoke(actionUnit);
            }
        }

        internal void SwapActionsInList(int i1, int i2)
        {
            OnActionOrderChanged.Invoke(actionUnitList[i1], actionUnitList[i2]);

            AActionUnit tmp = actionUnitList[i1];
            actionUnitList[i1] = actionUnitList[i2];
            actionUnitList[i2] = tmp;
        }

        public void StartActionPhase()
        {
            CurrentAction = actionUnitList[0];
            if (CurrentAction == null) return;

            CurrentAction.DoAction();
        }

        public void EndActionPhase()
        {
            print("END ACTION");
            actionUnitList.RemoveAll(unit => unit == null);
            StartActionPhase();
        }

        public bool StartDebug()
        {
            StartActionPhase();
            return CurrentAction != null;
            // if (actionUnitList.Count == 0
            //     || !actionUnitList[0].IsXRDebug)
            // {
            //     print("CANT DEBUG ACTIONS");
            //     return false;
            // }
            // actionUnitList[0].DoAction();
        }

        public void EndDebug()
        {
            actionUnitList[0].EndAction();
        }
    }
}
