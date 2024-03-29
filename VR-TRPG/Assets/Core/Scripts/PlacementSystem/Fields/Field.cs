using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Grid;

namespace VRTRPG.Place
{
    public class Field : MonoBehaviour
    {
        public int width = 1;
        public int height = 1;
        public int length = 1;

        public bool isWalkable;// = true;
        public bool isCharacter;
        public Vector3 anchor;
        public Transform visual;
        private List<AGridCell> occupiedGridCellList;

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
                gridCell.RemoveAllIncludedObjects();
            }
        }
    }
}
