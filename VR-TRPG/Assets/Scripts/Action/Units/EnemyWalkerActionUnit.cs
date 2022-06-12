using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Movement;

namespace VRTRPG.Action
{
    public class EnemyWalkerActionUnit : AActionUnit
    {
        [SerializeField] EnemyWalkerMoveUnit enemyMoveable;
        public override bool IsXRDebug { get; protected set; }

        void Awake()
        {
            IsXRDebug = false;
        }

        new void Start()
        {
            base.Start();
            actionSystem.PushAction(this);
        }

        public override void DoAction()
        {
            print("ACTION FROM ENEMY WALKER");
            enemyMoveable.DoRandomMove();
            EndAction();
        }

        public override void EndAction()
        {
            actionSystem.RemoveAction(this);
            actionSystem.PushAction(this);
            actionSystem.EndActionPhase();
        }

        public override void SelectUnit()
        {
            actionSystem.PushAction(this);
        }
    }
}
