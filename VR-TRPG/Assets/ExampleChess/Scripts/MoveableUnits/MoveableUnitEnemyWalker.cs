using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Grid;
using VRTRPG.Place;
using VRTRPG.Movement;

namespace VRTRPG.Chess.MoveableUnit
{
    public class MoveableUnitEnemyWalker : AGridMoveable
    {
        public override int walkDistance { get; protected set; }

        new void Start()
        {
            base.Start();
            walkDistance = 2;
        }

        public override HashSet<AGridCell> GetAvailableCells()
        {
            HashSet<AGridCell> walkableCells = new HashSet<AGridCell>();
            foreach (var cell in CurrentCell.GetNeighbor())
            {
                GetMoveableCellsRecursive(cell, walkDistance).ForEach(value =>
                {
                    walkableCells.Add(value);
                });
            }
            return walkableCells;
        }

        private List<AGridCell> GetMoveableCellsRecursive(AGridCell gridCell, int walkDistance)
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
                || gridCell.IncludedGameobjects.Exists(go =>
                {
                    if (go.GetComponent<AGridMoveable>() != null)
                    {
                        return true;
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
                walkableCells.AddRange(GetMoveableCellsRecursive(cell, walkDistance - 1));
            }
            return walkableCells;
        }

        public override void MoveTo(AGridCell cell)
        {
            CurrentCell.IncludedGameobjects.Remove(gameObject);
            transform.position = cell.transform.position;
            transform.parent = cell.transform;
            CurrentCell = transform.parent.GetComponent<AGridCell>();
            CurrentCell.IncludedGameobjects.Add(gameObject);
        }

        public override void DoMove()
        {
            var walkableFields = GetAvailableCells();
            int index = Random.Range(0, walkableFields.Count);
            AGridCell[] array = new AGridCell[walkableFields.Count];
            walkableFields.CopyTo(array);
            var randomCell = array[index];
            MoveTo(randomCell);
        }

        public override void StartMovePhase()
        {
            throw new System.NotImplementedException();
        }
    }
}
