using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Action
{
    public abstract class AActionUnit : MonoBehaviour
    {

        protected ActionSystem actionSystem;
        public string actionUnitName;

        void Start()
        {
            actionSystem = ActionSystem.Instance;
            actionSystem.RegisterAction(this);
        }

        void OnDestroy()
        {
            actionSystem.Deregister(this);
        }

        // Abstract
        public abstract void DoAction();
        public abstract void EndAction();
        public abstract bool IsXR { get; protected set; }
    }
}
