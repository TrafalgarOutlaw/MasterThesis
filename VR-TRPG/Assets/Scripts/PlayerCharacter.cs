using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public Transform walkIndicator;
    GridManager.GridCell currentGridCell;
    int walkDistance = 4;
    List<GameObject> indicatorList = new List<GameObject>();
    GridManager gridManager;

    void Start()
    {
        gridManager = GridManager.Instance;
        currentGridCell = GridManager.Instance.GetGridCellFromPosition(transform.position);
        SetWalkableFields(currentGridCell, walkDistance);

        gridManager.playerCharacterList.Add(this);
        gridManager.OnUpdateWalkableList.AddListener(ResetWalkableFieldsList);
    }

    public void ClearWalkableFieldsList()
    {
        foreach (var indicator in indicatorList)
        {
            Destroy(indicator);
        }
        indicatorList.Clear();

    }
    public void ResetWalkableFieldsList()
    {
        ClearWalkableFieldsList();
        SetWalkableFields(currentGridCell, walkDistance);
    }

    void SetWalkableFields(GridManager.GridCell gridCell, int walkDistance)
    {
        if (walkDistance <= 0)
        {
            return;
        }
        walkDistance--;

        if (gridCell != null)
        {
            var list = gridCell.GetNeighborGridCellList();

            foreach (var cell in list)
            {
                if (cell.GetIndex() == (gridCell.GetIndex() + Vector3Int.down))
                {
                    SetWalkableFields(cell, walkDistance);
                    continue;
                }

                if (cell.GetPlacedField() == null || !cell.GetPlacedField().isWalkable)
                {
                    continue;
                }

                var test = GridManager.Instance.GetGridCellFromIndex(cell.GetIndex() + Vector3Int.up);
                if (test != null && test.GetPlacedField() != null)
                {
                    if (test != null)
                    {
                        SetWalkableFields(test, walkDistance);
                    }
                    continue;
                }

                var postiton = cell.GetWorldPosition() + (GridManager.Instance.GetCellSize() * .5f * Vector3.one);
                if (cell != currentGridCell)
                {
                    var indicator = Instantiate(walkIndicator, postiton, Quaternion.identity);
                    indicatorList.Add(indicator.gameObject);
                    SetWalkableFields(cell, walkDistance);
                }

            }
        }
    }

    private void OnDestroy()
    {
        ClearWalkableFieldsList();
        gridManager.playerCharacterList.Remove(this);
    }
}
