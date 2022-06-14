using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Grid;

namespace VRTRPG.Movement
{
    public abstract class AGridMoveable : MonoBehaviour
    {
        protected MovementSystem movementSystem;
        protected AGridCell CurrentCell;

        public void Start()
        {
            movementSystem = MovementSystem.Instance;
            if (!transform.parent.TryGetComponent<AGridCell>(out CurrentCell))
            {
                print("CANT FIND CURRENT CELL");
            }

        }

        // Abstract
        public abstract int walkDistance { get; protected set; }
        public abstract HashSet<AGridCell> GetAvailableCells();
        public abstract void StartMovePhase();
        public abstract void MoveTo(AGridCell cell);

        public abstract void DoMove();

    }
}
