using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.XR;

namespace VRTRPG.Action
{
    public class WalkerActionUnit : AActionUnit
    {
        public override bool IsXR { get; protected set; }
        [SerializeField] XRUnit xRUnit;
        [SerializeField] GameObject indicator;

        void Awake()
        {
            IsXR = true;
        }

        public override void DoAction()
        {
            print("Action from walker");
            XRSystem.Instance.SelectUnit(xRUnit);
        }

        public override void EndAction()
        {
            XRSystem.Instance.DeselectUnit();
        }


        public void ActivateIndicator()
        {
            indicator.SetActive(true);
        }

        public void DeactivateIndicator()
        {
            indicator.SetActive(false);
        }
    }
}
