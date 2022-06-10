using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Movement;
using VRTRPG.XR;

namespace VRTRPG.Action
{
    public class SummonerActionUnit : AActionUnit
    {
        public override bool IsXR { get; protected set; }
        [SerializeField] XRUnit xRUnit;

        void Awake()
        {
            IsXR = true;
        }


        public override void DoAction()
        {
            // MovementSystem.Instance.StartMovePhase();
            print("Action from Summoner");
            XRSystem.Instance.SelectUnit(xRUnit);
            // actionSystem.EndAction();
        }

        public override void EndAction()
        {
            XRSystem.Instance.DeselectUnit();
        }

        public void Test()
        {
            print("OVER INTERACTABLE");
        }
    }
}
