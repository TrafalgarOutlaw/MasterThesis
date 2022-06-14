using UnityEngine;
using VRTRPG.Action;
using VRTRPG.Movement;
using VRTRPG.Grid;
using System.Collections.Generic;

namespace VRTRPG.Chess.ActionUnit
{
    public class ActionUnitEnemyWalker : AActionUnit
    {
        [SerializeField] AGridMoveable moveableUnitEnemyWalker;
        MovementSystem movementSystem;
        AGridMoveable moveable;

        new void Start()
        {
            base.Start();
            actionSystem.PushActionUnit(this);
            movementSystem = MovementSystem.Instance;
            moveable = GetComponent<AGridMoveable>();
        }

        public override void DoAction()
        {
            movementSystem.StartMovePhase(moveable);
            // HashSet<AGridCell> walkableCells = moveableUnitEnemyWalker.GetMoveableCells();
            // moveableUnitEnemyWalker.DoMove();
            EndAction();
        }

        public override void EndAction()
        {
            actionSystem.RemoveActionUnit(this);
            actionSystem.PushActionUnit(this);
            actionSystem.EndActionPhase();
        }
    }
}
