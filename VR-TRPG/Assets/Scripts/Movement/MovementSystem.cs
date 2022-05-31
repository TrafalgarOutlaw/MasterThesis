using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Grid;

namespace VRTRPG.Movement
{
    public class MovementSystem : MonoBehaviour
    {
        public static MovementSystem Instance { get; private set; }
        List<PlayerCharacter> characterList = new List<PlayerCharacter>();
        private PlayerCharacter currentCharacter;
        private int currentCharacterIndex;
        GridSystem gridManager;
        GridCell currentGridCell;
        [SerializeField] Transform walkIndicator;
        List<GameObject> indicatorList = new List<GameObject>();
        List<GridCell> walkableCellList = new List<GridCell>();
        [SerializeField] MovementIndicator movementIndicator;
        [SerializeField] SelectionIndicator selectionIndicator;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
        }

        void Start()
        {
            gridManager = GridSystem.Instance;
        }

        // internal void TryWalk(Vector3 fieldPosition)
        // {
        //     GridCell targetCell = gridManager.GetGridCellFromPosition(fieldPosition);
        //     if (walkableCellList.Contains(targetCell))
        //     {
        //         currentCharacter.transform.position = fieldPosition;
        //         StartMovePhase(currentCharacter);
        //     }
        // }

        // public void StartSelectionPhase()
        // {
        //     EndSelectionPhase();
        //     EndMovePhase();
        //     selectionIndicator.enabled = true;
        // }

        // public void EndSelectionPhase()
        // {
        //     selectionIndicator.enabled = false;
        // }

        // public void StartMovePhase(PlayerCharacter playerCharacter)
        // {
        //     EndSelectionPhase();
        //     EndMovePhase();
        //     currentCharacter = playerCharacter;
        //     currentGridCell = gridManager.GetGridCellFromPosition(playerCharacter.transform.position);
        //     SetWalkableFields(currentGridCell, playerCharacter.walkDistance);

        //     movementIndicator.enabled = true;
        // }

        // public void EndMovePhase()
        // {
        //     ClearWalkableFieldsList();
        //     movementIndicator.enabled = false;
        // }

        // void ClearWalkableFieldsList()
        // {
        //     foreach (var indicator in indicatorList)
        //     {
        //         Destroy(indicator);
        //     }
        //     walkableCellList.Clear();
        //     indicatorList.Clear();
        // }

        // void SetWalkableFields(GridCell gridCell, int walkDistance)
        // {
        //     if (walkDistance <= 0)
        //     {
        //         return;
        //     }
        //     walkDistance--;

        //     if (gridCell != null)
        //     {
        //         var list = gridCell.GetNeighborGridCellList();

        //         foreach (var cell in list)
        //         {
        //             if (cell.GetIndex() == (gridCell.GetIndex() + Vector3Int.down))
        //             {
        //                 SetWalkableFields(cell, walkDistance);
        //                 continue;
        //             }

        //             if (cell.GetPlacedField() == null || !cell.GetPlacedField().isWalkable)
        //             {
        //                 continue;
        //             }

        //             var test = GridSystem.Instance.GetGridCellFromIndex(cell.GetIndex() + Vector3Int.up);
        //             if (test != null && test.GetIncludedField() != null)
        //             {
        //                 if (test != null)
        //                 {
        //                     SetWalkableFields(test, walkDistance);
        //                 }
        //                 continue;
        //             }

        //             var postiton = cell.GetWorldPosition() + (GridSystem.Instance.GetCellSize() * .5f * Vector3.one);
        //             if (cell != currentGridCell)
        //             {
        //                 var indicator = Instantiate(walkIndicator, postiton, Quaternion.identity);
        //                 walkableCellList.Add(cell);
        //                 indicatorList.Add(indicator.gameObject);
        //                 SetWalkableFields(cell, walkDistance);
        //             }

        //         }

        //     }
        // }

    }
}