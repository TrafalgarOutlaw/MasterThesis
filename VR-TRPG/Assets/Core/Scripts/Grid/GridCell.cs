
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
        public override Vector3 CellTopSide { get; protected set; }
        public override List<Vector3Int> CellDirList { get; protected set; }

        public HashSet<AGridCell> NeighborCellSet { get; private set; }
        public override List<GameObject> IncludedGameobjects { get; protected set; }


        private void Awake()
        {
            CellDirList = new List<Vector3Int>();
            NeighborCellSet = new HashSet<AGridCell>();
            IncludedGameobjects = new List<GameObject>();
        }

        public override void Init(GridSystem grid, int x, int y, int z, float cellSize)
        {
            _grid = grid;
            CellSize = cellSize;

            Index = new Vector3Int(x, y, z);
            WorldPosition = CellSize * (Vector3)Index;
            transform.position = WorldPosition;
            CellCenter = WorldPosition + new Vector3(.5f, -.5f, .5f) * cellSize;
            CellTopSide = WorldPosition + new Vector3(.5f, 0, .5f) * cellSize;

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

        public override bool CanBuild()
        {
            return IncludedGameobjects.Count == 0;
        }

        public override void SetNeighbor(AGridCell gridCell)
        {
            NeighborCellSet.Add(gridCell);
            gridCell.UpdateNeighbor(this);
        }

        public override void UpdateNeighbor(AGridCell gridCell)
        {
            NeighborCellSet.Add(gridCell);
        }

        public override HashSet<AGridCell> GetNeighbor()
        {
            return NeighborCellSet;
        }
        // public void SetNeighbor(GridCell gridCell, GridCell neighborGridCell)
        // {
        //     gridCell.NeighborCellSet.Add(neighborGridCell);
        //     neighborGridCell.NeighborCellSet.Add(gridCell);
        // }

        public override void RemoveAllIncludedObjects()
        {
            IncludedGameobjects.ForEach(go =>
            {
                Destroy(go);
            });
            IncludedGameobjects.Clear();
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

        public override void RemoveIncludedObject(GameObject go)
        {
            if (!IncludedGameobjects.Remove(go))
            {
                print("COULD NOT REMOVE OBJECT :" + go.name);
                print(name);
                foreach (var item in IncludedGameobjects)
                {
                    print(item);
                }
            }
        }
    }
}