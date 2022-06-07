using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Action
{
    public class Character : MonoBehaviour, IActionable
    {
        private ActionSystem actionSystem;
        private string name = "Walker";

        void Start()
        {
            actionSystem = ActionSystem.Instance;
            RegisterAction();
        }

        void OnDestroy()
        {
            DeregisterAction();
        }

        public void RegisterAction()
        {
            actionSystem.RegisterAction(this);
        }

        public void DoAction()
        {

        }

        public void DeregisterAction()
        {
            actionSystem.Deregister(this);
        }

        public void SetActionName(string name)
        {
            this.name = name;
        }

        public string GetActionName()
        {
            return name;
        }
    }
}
