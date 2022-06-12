using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Action
{
    public abstract class AActionUnit : MonoBehaviour
    {

        protected ActionSystem actionSystem;
        public string actionUnitName;

        public void Start()
        {
            print("START FROM:  " + name);
            actionSystem = ActionSystem.Instance;
        }

        void OnDestroy()
        {
            actionSystem.RemoveAction(this);
        }

        // Abstract
        public abstract void SelectUnit();
        public abstract void DoAction();
        public abstract void EndAction();
        public abstract bool IsXRDebug { get; protected set; }
    }
}
