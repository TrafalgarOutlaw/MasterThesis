using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VRTRPG.XR;

namespace VRTRPG.Action
{
    public abstract class AActionUnit : MonoBehaviour
    {
        protected ActionSystem actionSystem;
        public string actionUnitName;

        public void Start()
        {
            actionSystem = ActionSystem.Instance;
        }

        void OnDestroy()
        {
            actionSystem.RemoveActionUnit(this);
        }

        public void OnHoverEnter(HoverEnterEventArgs args) { return; }
        public void OnHoverExit(HoverExitEventArgs args) { return; }
        public void OnSelectEnter(SelectEnterEventArgs args) { return; }
        public void OnSelectExit(SelectExitEventArgs args) { return; }

        // Abstract
        public abstract void DoAction();
        public abstract void EndAction();
        public abstract XRUnit GetXRUnit();
    }
}
