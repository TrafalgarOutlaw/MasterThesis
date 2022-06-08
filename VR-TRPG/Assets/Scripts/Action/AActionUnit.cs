using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Action
{
    public abstract class AActionUnit : MonoBehaviour
    {
        public abstract void DoAction();

        protected ActionSystem actionSystem;
        public string actionUnitName;

        void Start()
        {
            actionSystem = ActionSystem.Instance;

            print("REGISTER FROM " + actionUnitName);
            actionSystem.RegisterAction(this);
        }

        void OnDestroy()
        {
            actionSystem.Deregister(this);
        }
    }
}
