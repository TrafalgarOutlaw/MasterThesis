using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Action
{
    public interface IActionable
    {
        void RegisterAction();
        void DeregisterAction();
        void DoAction();
        void SetActionName(string name);
        string GetActionName();
    }
}
