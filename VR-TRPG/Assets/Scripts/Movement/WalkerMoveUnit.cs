using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Grid;

namespace VRTRPG.Movement
{
    public class WalkerMoveUnit : AMoveable
    {
        bool isJumpable = false;

        public override int walkDistance { get; protected set; }

        new void Start()
        {
            base.Start();
            walkDistance = 4;
        }

        public void ShowWalkableFields()
        {
            movementSystem.ClearIndicator();
            // movementSystem.ShowWalkableFields(this);
        }

        public HashSet<AGridCell> GetWalkableFields()
        {
            HashSet<AGridCell> walkableCells = new HashSet<AGridCell>();
            CurrentCell = transform.parent.GetComponent<AGridCell>();
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

            Field field = null;

            if ((cellAbove != null && !cellAbove.CanBuild())
                || gridCell.IncludedGameobjects == null
                || !gridCell.IncludedGameobjects.Exists(go =>
                {
                    if (go.TryGetComponent<Field>(out field))
                    {
                        if (field.isWalkable) return true;
                    }
                    return false;
                })
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

        public override void MoveTo(AGridCell cell)
        {
            transform.position = cell.transform.position;
            transform.parent = cell.transform;
            movementSystem.ClearIndicator();
        }

        void OnDestroy()
        {
            movementSystem.RemoveWalker(this);
        }
    }
}