using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Grid
{
    public abstract class AGridCell : MonoBehaviour
    {
        protected GameObject emptyGrid;
        public abstract Vector3Int Index { get; protected set; }
        public abstract Vector3 WorldPosition { get; protected set; }
        public abstract float CellSize { get; protected set; }
        public abstract Vector3 CellCenter { get; protected set; }
        public abstract Vector3 CellTopSide { get; protected set; }
        public abstract List<Vector3Int> CellDirList { get; protected set; }
        public abstract List<GameObject> IncludedGameobjects { get; protected set; }

        public abstract void Init(GridSystem grid, int x, int y, int z, float cellSize);
        public abstract bool CanBuild();
        public abstract HashSet<AGridCell> GetNeighbor();
        public abstract void SetNeighbor(AGridCell gridCell);
        public abstract void UpdateNeighbor(AGridCell gridCell);
        public abstract void SetIncludedField(Field field);
        public abstract void RemoveIncludedObject();
        public abstract void DisableRenderer();
        public abstract void EnableRenderer();
        public abstract void DisableCollider();
        public abstract void EnableCollider();
    }
}
