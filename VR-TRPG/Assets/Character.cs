using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Transform walkIndicator;
    GridManager.GridCell currentGridCell;
    int walkDistance = 4;

    void Start()
    {
        currentGridCell = GridManager.Instance.GetGridCellFromPosition(transform.position);
        SetWalkableFields(currentGridCell, walkDistance);
    }

    void SetWalkableFields(GridManager.GridCell gridCell, int walkDistance)
    {
        if (walkDistance <= 0)
        {
            Debug.Log("out of distance: " + gridCell.GetIndex());
            return;
        }
        walkDistance--;
        if (gridCell != null)
        {
            Debug.Log("current: " + gridCell.GetIndex());
            var list = gridCell.GetNeighborGridCellList();

            foreach (var cell in list)
            {
                if (cell.GetIndex() == (gridCell.GetIndex() + Vector3Int.down))
                {
                    Debug.Log("under current field: " + cell.GetIndex());
                    SetWalkableFields(cell, walkDistance);
                    continue;
                }

                if (cell.GetPlacedField() == null || !cell.GetPlacedField().isWalkable)
                {
                    Debug.Log("not walkable or null: " + cell.GetIndex());
                    SetWalkableFields(cell, walkDistance);
                    continue;
                }

                var test = GridManager.Instance.GetGridCellFromIndex(cell.GetIndex() + Vector3Int.up);
                if (test != null && test.GetPlacedField() != null)
                {
                    Debug.Log("Top is blocked: " + test.GetIndex());
                    if (test != null)
                    {
                        SetWalkableFields(test, walkDistance);
                    }
                    continue;
                }
                // Debug.Log("place for: " + cell.GetIndex());0

                Debug.Log("PLCE: " + cell.GetIndex());
                var postiton = cell.GetWorldPosition() + (GridManager.Instance.GetCellSize() * .5f * Vector3.one);
                if (cell != currentGridCell)
                {
                    Instantiate(walkIndicator, postiton, Quaternion.identity);
                    SetWalkableFields(cell, walkDistance);
                }

            }
        }

    }
}
