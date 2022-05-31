using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Grid
{

    [CreateAssetMenu(fileName = "GridCellSO", menuName = "ScriptableObjects/GridCellSO")]
    public class GridCellSo : ScriptableObject
    {
        // Fields
        Transform pfCell;
        GridSystem _grid;
        int _x;
        int _y;
        int _z;
        float _cellSize;
        List<Vector3Int> gridDirList;

        // Properties
        public Vector3Int Index;
        public Vector3 WorldPosition;

    }
}