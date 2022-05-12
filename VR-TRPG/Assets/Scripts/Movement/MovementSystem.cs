using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.XR;

namespace VRTRPG.Movement
{
    public class MovementSystem : MonoBehaviour
    {
        public static MovementSystem Instance { get; private set; }
        List<PlayerCharacter> characterList = new List<PlayerCharacter>();
        private PlayerCharacter currentCharacter;
        private int currentCharacterIndex;
        GridManager gridManager;
        GridManager.GridCell currentGridCell;
        [SerializeField] Transform walkIndicator;
        List<GameObject> indicatorList = new List<GameObject>();
        List<GridManager.GridCell> walkableCellList = new List<GridManager.GridCell>();
        [SerializeField] MovementIndicator movementIndicator;

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
            gridManager = GridManager.Instance;
        }

        internal void TryWalk(Vector3 fieldPosition)
        {
            Debug.Log("fieldpos:" + fieldPosition);
            GridManager.GridCell targetCell = gridManager.GetGridCellFromPosition(fieldPosition);
            if (walkableCellList.Contains(targetCell))
            {
                // var targetField = targetCell.GetPlacedField().transform;
                EndMovePhase();

                currentCharacter.transform.position = fieldPosition;
                Debug.Log("Moved to: " + fieldPosition);
                // currentCharacter.transform.parent = targetField;
                StartMovePhase(currentCharacter);
            }
        }

        public void StartMovePhase(PlayerCharacter playerCharacter)
        {
            currentCharacter = playerCharacter;
            currentGridCell = gridManager.GetGridCellFromPosition(playerCharacter.transform.position);
            SetWalkableFields(currentGridCell, playerCharacter.walkDistance);

            movementIndicator.enabled = true;
        }

        public void EndMovePhase()
        {
            ClearWalkableFieldsList();
            movementIndicator.enabled = false;
        }

        void ClearWalkableFieldsList()
        {
            foreach (var indicator in indicatorList)
            {
                Destroy(indicator);
            }
            walkableCellList.Clear();
            indicatorList.Clear();
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
                        walkableCellList.Add(cell);
                        indicatorList.Add(indicator.gameObject);
                        SetWalkableFields(cell, walkDistance);
                    }

                }

            }
        }

    }
}