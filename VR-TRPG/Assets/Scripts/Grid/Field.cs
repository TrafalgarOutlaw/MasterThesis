using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Grid
{
    public class Field : MonoBehaviour
    {
        public int width = 1;
        public int height = 1;
        public int length = 1;

        public bool isWalkable;// = true;
        public Vector3 anchor;
        public Transform visual;
        private List<AGridCell> occupiedGridCellList;

        public void SetSize(float cellSize)
        {
            foreach (Transform child in transform)
            {
                child.localScale = Vector3.Scale(child.localScale, new Vector3(cellSize, cellSize, cellSize));
            }
        }

        public List<AGridCell> GetOccupiedGridCells()
        {
            return occupiedGridCellList;
        }

        internal void SetOccupiedGridCells(List<AGridCell> neededGridCellsList)
        {
            occupiedGridCellList = neededGridCellsList;
        }

        internal void ClearOccupiedGridCells()
        {
            foreach (AGridCell gridCell in occupiedGridCellList)
            {
                gridCell.ClearIncludedField();
            }
        }
    }
}
