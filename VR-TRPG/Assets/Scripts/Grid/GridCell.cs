
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Grid
{
    public class GridCell
    {
        Grid<GridCell> _grid;
        int _x;
        int _y;
        int _z;
        Vector3Int index;
        Transform _emptyGridObjectTransform;
        EmptyGridObject _emptyGridObject;
        Field _field;
        Vector3 positionInWorld;
        List<GridCell> occupiedGridCellList = new List<GridCell>();
        List<GridCell> neighborGridCellList = new List<GridCell>();
        List<Vector3Int> gridDirList = new List<Vector3Int>();

        public GridCell(Grid<GridCell> grid, int x, int y, int z, Transform emptyGridObject)
        {
            _grid = grid;
            _x = x;
            _y = y;
            _z = z;
            SetGridDirList();

            float cellSize = grid.GetCellSize();
            positionInWorld = new Vector3(x, y, z) * cellSize;
            index = new Vector3Int(_x, _y, _z);

            _emptyGridObjectTransform = Object.Instantiate(emptyGridObject, positionInWorld, Quaternion.identity, GridSystem.Instance.gridGameObject);
            _emptyGridObjectTransform.localScale *= cellSize;

            _emptyGridObject = _emptyGridObjectTransform.GetComponent<EmptyGridObject>();
            _emptyGridObject.SetIndex(new Vector3(x, y, z));

            UpdateNeighbors();
            if (y != 1)
            {
                DisableForMouse();
            }

        }

        void SetGridDirList()
        {
            gridDirList.Add(new Vector3Int(1, 0, 0));
            gridDirList.Add(new Vector3Int(-1, 0, 0));
            gridDirList.Add(new Vector3Int(0, 1, 0));
            gridDirList.Add(new Vector3Int(0, -1, 0));
            gridDirList.Add(new Vector3Int(0, 0, 1));
            gridDirList.Add(new Vector3Int(0, 0, -1));
        }

        private void UpdateNeighbors()
        {
            foreach (var dir in gridDirList)
            {
                GridCell neighborGridCell = _grid.GetGridCell(new Vector3Int(_x - dir.x, _y - dir.y, _z - dir.z));
                if (neighborGridCell != null)
                {
                    SetNeighbor(this, neighborGridCell);
                }
            }
        }

        private void SetNeighbor(GridCell gridCell, GridCell neighborGridCell)
        {
            gridCell.neighborGridCellList.Add(neighborGridCell);
            neighborGridCell.neighborGridCellList.Add(gridCell);
        }

        public Field GetPlacedField()
        {
            return _field;
        }

        public Vector3Int GetIndex()
        {
            return new Vector3Int(_x, _y, _z);
        }

        public void SetField(Field field, List<GridCell> occupyGridCellList)
        {
            _field = field;
            foreach (var cell in occupyGridCellList)
            {
                if (cell != this)
                {
                    occupiedGridCellList.Add(cell);
                }
            }
        }

        public void ClearField()
        {
            _field = null;
            ClearOccupiedGridCellList();
        }

        void ClearOccupiedGridCellList()
        {
            foreach (var cell in occupiedGridCellList)
            {
                cell._field = null;
                cell.occupiedGridCellList = new List<GridCell>();
            }
            occupiedGridCellList = new List<GridCell>();
        }

        public bool CanBuild()
        {
            return _field == null;
        }

        internal void DisableForMouse()
        {
            _emptyGridObject.DisableForMouse();
        }

        internal void DisableRenderer()
        {
            _emptyGridObject.DisableRenderer();
        }

        internal void EnableRenderer()
        {
            _emptyGridObject.EnableRenderer();
        }

        public Vector3 GetWorldPosition()
        {
            return positionInWorld;
        }

        public List<GridCell> GetNeighborGridCellList()
        {
            return neighborGridCellList;
        }
    }
}