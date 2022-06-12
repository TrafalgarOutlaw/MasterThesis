using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Grid;

namespace VRTRPG.Movement
{
    public abstract class AMoveable : MonoBehaviour
    {
        protected MovementSystem movementSystem;
        protected AGridCell CurrentCell;

        public void Start()
        {
            movementSystem = MovementSystem.Instance;
            CurrentCell = transform.parent.GetComponent<AGridCell>();
        }

        // Abstract
        public abstract int walkDistance { get; protected set; }
        public abstract void MoveTo(AGridCell cell);
    }
}
