using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Grid
{
    public class PlaceableField : APlaceable
    {
        public override bool IsPlaceable(List<Vector3Int> neededSpace)
        {
            return neededSpace.TrueForAll(index =>
            {
                AGridCell gridCell = GridSystem.Instance.GetGridCell(index);

                // Needs an empty grid
                return (gridCell != null && gridCell.CanBuild());
            });
        }

        internal override void SetOccupiedGridCells(List<AGridCell> neededGridCells)
        {
            occupiedGridCells = neededGridCells;

            neededGridCells.ForEach(cell =>
            {
                cell.IncludedGameobjects.Add(this.gameObject);
            });
        }
    }
}
