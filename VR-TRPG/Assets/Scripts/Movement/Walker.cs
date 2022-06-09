using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Grid;

namespace VRTRPG.Movement
{
    public class Walker : MonoBehaviour
    {
        private MovementSystem movementSystem;
        [SerializeField] Field currentField;
        public AGridCell CurrentCell { get; internal set; }
        public int walkDistance = 4;
        public int fallDistance = 2;
        bool isJumpable = false;

        void Start()
        {
            movementSystem = MovementSystem.Instance;
            movementSystem.AddWalker(this);

            //currentField = PlaceSystem.Instance.currentGridCell.IncludedGameobjects;
            CurrentCell = currentField.GetOccupiedGridCells()[0];
            transform.parent = currentField.transform;
        }

        public HashSet<AGridCell> GetWalkableFields()
        {
            HashSet<AGridCell> walkableCells = new HashSet<AGridCell>();
            foreach (var cell in CurrentCell.GetNeighbor())
            {
                GetWalkableFieldsRecursive(cell, walkDistance).ForEach(value =>
                {
                    walkableCells.Add(value);
                });
            }
            return walkableCells;
        }

        private List<AGridCell> GetWalkableFieldsRecursive(AGridCell gridCell, int walkDistance)
        {
            List<AGridCell> walkableCells = new List<AGridCell>();
            if (walkDistance <= 0) return walkableCells;

            AGridCell cellAbove = GridSystem.Instance.GetGridCell(gridCell.Index + Vector3Int.up);
            if ((cellAbove != null && !cellAbove.CanBuild())
                || gridCell.IncludedGameobjects == null
                //|| !gridCell.IncludedGameobjects.isWalkable
                || gridCell.Index.y != CurrentCell.Index.y
                || gridCell == CurrentCell)
            {
                return walkableCells;
            }
            walkableCells.Add(gridCell);
            foreach (var cell in gridCell.GetNeighbor())
            {
                walkableCells.AddRange(GetWalkableFieldsRecursive(cell, walkDistance - 1));
            }
            return walkableCells;
        }

        void OnDestroy()
        {
            movementSystem.RemoveWalker(this);
        }
    }
}