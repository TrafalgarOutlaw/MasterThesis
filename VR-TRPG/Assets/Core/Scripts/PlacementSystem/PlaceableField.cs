using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Grid;

namespace VRTRPG.Place
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
    }
}
