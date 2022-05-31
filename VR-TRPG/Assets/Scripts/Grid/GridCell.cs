
using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Grid
{
    public class GridCell : AGridCell
    {
        GridSystem _grid;
        [SerializeField] Transform emptyGridObject;
        [SerializeField] MeshRenderer emptyGridMeshRenderer;
        [SerializeField] BoxCollider emptyGridBoxCollider;

        // Properties
        public override Vector3Int Index { get; protected set; }
        public override Vector3 WorldPosition { get; protected set; }
        public override float CellSize { get; protected set; }
        public override Vector3 CellCenter { get; protected set; }
        public override List<Vector3Int> CellDirList { get; protected set; }
        public override HashSet<AGridCell> NeighborCellSet { get; protected set; }
        public override Field IncludedField { get; protected set; }


        private void Awake()
        {
            CellDirList = new List<Vector3Int>();
            NeighborCellSet = new HashSet<AGridCell>();
        }

        public override void Init(GridSystem grid, int x, int y, int z, float cellSize)
        {
            _grid = grid;
            CellSize = cellSize;

            Index = new Vector3Int(x, y, z);
            WorldPosition = CellSize * (Vector3)Index;
            transform.position = WorldPosition;
            CellCenter = WorldPosition + new Vector3(.5f, -.5f, .5f) * cellSize;

            InitCellDirList();
            transform.localScale *= CellSize;
        }

        void InitCellDirList()
        {
            CellDirList.Add(Vector3Int.right);
            CellDirList.Add(Vector3Int.left);
            CellDirList.Add(Vector3Int.up);
            CellDirList.Add(Vector3Int.down);
            CellDirList.Add(Vector3Int.forward);
            CellDirList.Add(Vector3Int.back);
        }

        // private void UpdateNeighbors()
        // {
        //     foreach (var dir in CellDirList)
        //     {
        //         IGridCell neighborGridCell = _grid.GetGridCell(new Vector3Int(_x - dir.x, _y - dir.y, _z - dir.z));
        //         if (neighborGridCell != null)
        //         {
        //             SetNeighbor(this, neighborGridCell);
        //         }
        //     }
        // }

        public override bool CanBuild()
        {
            return IncludedField == null;
        }

        public override void SetNeighbor(AGridCell gridCell)
        {
            NeighborCellSet.Add(gridCell);
            gridCell.NeighborCellSet.Add(gridCell);
        }
        // public void SetNeighbor(GridCell gridCell, GridCell neighborGridCell)
        // {
        //     gridCell.NeighborCellSet.Add(neighborGridCell);
        //     neighborGridCell.NeighborCellSet.Add(gridCell);
        // }

        public override void SetIncludedField(Field field)
        {
            IncludedField = field;
        }

        public override void ClearIncludedField()
        {
            IncludedField = null;
        }

        // void ClearOccupiedGridCellList()
        // {
        //     foreach (var cell in occupiedGridCellList)
        //     {
        //         cell._field = null;
        //         cell.occupiedGridCellList = new List<GridCell>();
        //     }
        //     occupiedGridCellList = new List<GridCell>();
        // }

        public override void DisableRenderer()
        {
            emptyGridMeshRenderer.enabled = false;
        }

        public override void EnableRenderer()
        {
            emptyGridMeshRenderer.enabled = true;
        }

        public override void DisableCollider()
        {
            emptyGridBoxCollider.enabled = false;
            DisableRenderer();
        }

        public override void EnableCollider()
        {
            emptyGridBoxCollider.enabled = true;
            EnableRenderer();
        }

    }
}