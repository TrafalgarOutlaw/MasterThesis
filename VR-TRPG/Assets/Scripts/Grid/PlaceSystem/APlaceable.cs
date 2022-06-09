using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Grid
{
    public abstract class APlaceable : MonoBehaviour
    {
        protected GridSystem gridSystem;
        [SerializeField] protected int width = 1;
        [SerializeField] protected int height = 1;
        [SerializeField] protected int length = 1;

        [SerializeField] protected PlaceableVisual visual;
        protected List<AGridCell> occupiedGridCells;

        void Start()
        {
            gridSystem = GridSystem.Instance;    
        }

        public PlaceableVisual getVisual()
        {
            return visual;
        }

        public List<Vector3Int> GetNeededSpace(Transform transform, Vector3Int gridIndex)
        {
            List<Vector3Int> neededSpace = new List<Vector3Int>();
            Vector3Int xDirInWorld = Vector3Int.RoundToInt(transform.TransformDirection(Vector3.right));
            Vector3Int yDirInWorld = Vector3Int.RoundToInt(transform.TransformDirection(Vector3.up));
            Vector3Int zDirInWorld = Vector3Int.RoundToInt(transform.TransformDirection(Vector3.forward));

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < length; z++)
                    {
                        Vector3Int neededIndex = (x * xDirInWorld + -y * yDirInWorld + z * zDirInWorld) + Vector3Int.FloorToInt(transform.position / GridSystem.Instance.GetCellSize());
                        // Vector3Int neededGridCellIndex = (x * xDirInWorld + -y * yDirInWorld + z * zDirInWorld) + Vector3Int.FloorToInt(fieldTargetTransform.position);
                        neededSpace.Add(neededIndex);
                    }
                }
            }

            return neededSpace;
        }

        // Abstract
        public abstract bool IsPlaceable(List<Vector3Int> neededSpace);
        internal abstract void SetOccupiedGridCells(List<AGridCell> neededGridCells);
    }
}
