using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Grid;

namespace VRTRPG.Place
{
    public class PlaceableCharacter : APlaceable
    {
        public override bool IsPlaceable(List<Vector3Int> neededSpace)
        {
            return neededSpace.TrueForAll(index =>
            {
                AGridCell gridCell = GridSystem.Instance.GetGridCell(index);

                // Needs a grid
                if (gridCell == null) return false;

                // Needs a field on grid
                Field field = null;
                gridCell.IncludedGameobjects.ForEach(go =>
                {
                    field = go.GetComponent<Field>();
                });
                if (field == null) return false;

                return field.isWalkable;
            });
        }
    }
}
