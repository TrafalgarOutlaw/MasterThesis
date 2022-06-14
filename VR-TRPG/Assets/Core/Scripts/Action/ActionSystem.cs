using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRTRPG.Action
{
    [System.Serializable] public class ActionUnitEvent : UnityEvent<AActionUnit> { }
    [System.Serializable] public class ActionOrderEvent : UnityEvent<AActionUnit, AActionUnit> { }
    public class ActionSystem : MonoBehaviour
    {
        [HideInInspector] public ActionUnitEvent OnAddActionUnit;
        [HideInInspector] public ActionUnitEvent OnDeleteActionUnit;
        [HideInInspector] public ActionOrderEvent OnActionOrderChanged;

        public static ActionSystem Instance { get; private set; }
        public List<AActionUnit> actionUnitList = new List<AActionUnit>();
        public AActionUnit CurrentAction { get; private set; }

        void Awake()
        {
            // Set Singleton
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            Instance = this;
        }

        public void SetCurrentActionUnit(AActionUnit actionUnit)
        {
            CurrentAction = actionUnit;
        }

        public void PushActionUnit(AActionUnit actionUnit)
        {
            actionUnitList.Add(actionUnit);
            OnAddActionUnit.Invoke(actionUnit);
        }

        public void InsertActionUnit(int index, AActionUnit actionUnit)
        {
            if (index < actionUnitList.Count)
            {
                actionUnitList.Insert(index, actionUnit);
                OnAddActionUnit.Invoke(actionUnit);
            }
            else
            {
                PushActionUnit(actionUnit);
            }
        }

        public void RemoveActionUnit(AActionUnit actionUnit)
        {
            if (actionUnitList.Remove(actionUnit))
            {
                OnDeleteActionUnit.Invoke(actionUnit);
            }
        }

        internal void SwapActionUnits(int i1, int i2)
        {
            OnActionOrderChanged.Invoke(actionUnitList[i1], actionUnitList[i2]);

            AActionUnit tmp = actionUnitList[i1];
            actionUnitList[i1] = actionUnitList[i2];
            actionUnitList[i2] = tmp;
        }

        public void StartActionPhase()
        {
            if (actionUnitList.Count == 0) return;
            CurrentAction = actionUnitList[0];

            CurrentAction.DoAction();
        }

        public void EndActionPhase()
        {
            actionUnitList.RemoveAll(unit => unit == null);
            StartActionPhase();
        }

        public bool StartDebug()
        {
            StartActionPhase();
            return CurrentAction != null;
        }
    }
}
