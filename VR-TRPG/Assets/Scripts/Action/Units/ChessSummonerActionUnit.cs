using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.XR;

namespace VRTRPG.Action
{
    public class ChessSummonerActionUnit : AActionUnit
    {
        public override bool IsXRDebug { get; protected set; }
        [SerializeField] XRUnit xRUnit;
        private XRSystem xrSystem;

        void Awake()
        {
            IsXRDebug = true;
        }

        new void Start()
        {
            print("START FROM CHESS SUMMONER");
            base.Start();
            xrSystem = XRSystem.Instance;
            actionSystem.PushAction(this);
        }

        public override void DoAction()
        {
            // MovementSystem.Instance.StartMovePhase();
            print("Action from Summoner");
            xrSystem.SelectUnit(xRUnit);
            // actionSystem.EndAction();
        }

        public override void EndAction()
        {
            // xrSystem.DeselectUnit();
            actionSystem.RemoveAction(this);
            actionSystem.InsertAction(1, this);
            actionSystem.EndActionPhase();
        }

        public override void SelectUnit()
        {
            actionSystem.InsertAction(1, this);
        }
    }
}
