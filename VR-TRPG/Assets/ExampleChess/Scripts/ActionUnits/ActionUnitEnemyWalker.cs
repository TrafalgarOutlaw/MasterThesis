using UnityEngine;
using VRTRPG.Action;
using VRTRPG.Movement;
using VRTRPG.Grid;
using System.Collections.Generic;
using VRTRPG.Combat;

namespace VRTRPG.Chess.ActionUnit
{
    public class ActionUnitEnemyWalker : AActionUnit
    {
        [SerializeField] AGridMoveable moveableUnitEnemyWalker;
        MovementSystem movementSystem;
        private CombatSystem combatSystem;
        AGridMoveable moveable;
        private ACombatable combatable;

        new void Start()
        {
            base.Start();
            actionSystem.PushActionUnit(this);
            movementSystem = MovementSystem.Instance;
            combatSystem = CombatSystem.Instance;
            moveable = GetComponent<AGridMoveable>();
            combatable = GetComponent<ACombatable>();
        }

        public override void DoAction()
        {
            combatSystem.StartCombatPhase(combatable);
            if (combatSystem.currentCombatable != null)
            {
                movementSystem.StartMovePhase(moveable);
            }
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
