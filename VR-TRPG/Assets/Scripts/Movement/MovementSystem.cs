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
        GridSystem gridSystem;

        public void StartMovePhase()
        {
            throw new NotImplementedException();
        }

        [SerializeField] Transform walkIndicator;
        [SerializeField] Transform movementIndicatorContainer;
        List<WalkerMoveUnit> walkerList = new List<WalkerMoveUnit>();
        List<Transform> indicatorList = new List<Transform>();
        public WalkerMoveUnit debugWalker;

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
            gridSystem = GridSystem.Instance;
        }

        // internal void Subscribe(Walker walker, AGridCell cell)
        // {
        //     walkableList.Add(walker, cell);
        //     SetWalkableFields(walker);
        //     ShowWalkeableFields();
        // }

        public void StartDebug()
        {
            if (walkerList.Count == 0) return;

            debugWalker = walkerList[0];
            ShowWalkableFields(debugWalker);
        }


        public void ClearIndicator()
        {
            indicatorList.ForEach(indicator => Destroy(indicator.gameObject));
            indicatorList.Clear();
        }

        internal void AddWalker(WalkerMoveUnit walker)
        {
            walkerList.Add(walker);
        }

        public void ShowWalkableFields(WalkerMoveUnit walker)
        {
            HashSet<AGridCell> walkableCellSet = walker.GetWalkableFields();
            foreach (var cell in walkableCellSet)
            {
                Transform moveIndicator = Instantiate(walkIndicator, cell.CellTopSide, Quaternion.identity, movementIndicatorContainer);
                moveIndicator.localScale *= cell.CellSize;

                indicatorList.Add(moveIndicator);
            }
        }

        public void RemoveWalker(WalkerMoveUnit walker)
        {
            walkerList.Remove(walker);
        }

        public bool CanWalkTo(WalkerMoveUnit walker, AGridCell cell)
        {
            return walker.GetWalkableFields().Contains(cell);
        }

        public void WalkTo(WalkerMoveUnit walker, AGridCell cell)
        {
            walker.transform.position = cell.WorldPosition;
            walker.CurrentCell = cell;
            ClearIndicator();
        }

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