using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRTRPG.Grid
{
    // [System.Serializable] public class OnGridEventQuaternion : UnityEvent<Quaternion> { }
    // [System.Serializable] public class OnGridEventFieldVisual : UnityEvent<FieldVisual, float> { }
    // [System.Serializable] public class OnGridEventInt : UnityEvent<int> { }


    public class FieldSystem : MonoBehaviour
    {
        // This instance
        private static FieldSystem _instance;
        public static FieldSystem Instance { get { return _instance; } }

        // Events
        // public OnGridEventQuaternion OnFieldRotationChange;
        // public OnGridEventFieldVisual OnSelectedFieldChange;

        // Grid
        GridSystem grid;

        // Placeable fields
        [SerializeField] List<Field> fieldList;
        int fieldListIndex = 0;
        Field selectedField;

        // Previewfield
        Transform fieldTargetTransform;

        [SerializeField] PreviewField previewField;

        // Insert into gridCells

        Quaternion currentRotationGridCell = Quaternion.identity;
        // List<GridCell> currentOccupiedGridCellList;
        // public bool isCurrentFieldPlaceable;

        // Level
        [SerializeField] Transform fieldsContainer;
        InputManager inputManager;
        private float _cellSize;
        public AGridCell selectedGridCell;

        void Awake()
        {
            // Set Singleton
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            _instance = this;
        }

        void Start()
        {
            inputManager = InputManager.Instance;
            grid = GridSystem.Instance;
            _cellSize = grid.GetCellSize();

            // inputManager.OnRotateField.AddListener(RotateField);

            selectedGridCell = grid.GetGridCell(Vector3.zero);

            selectedField = fieldList[fieldListIndex];
            previewField.InitPreviewField(selectedField.visual, _cellSize, selectedField.visual.position);
            SetPreviewLayer(previewField.previewFieldTransform.gameObject);

            inputManager.OnPlaceField.AddListener(TryPlaceField);
            inputManager.OnSelectedFieldChanged.AddListener(ChangeField);
            inputManager.OnDeletField.AddListener(TryDeleteFields);
            grid.OnSelectedGridCellChange.AddListener(ChangeFieldTargetPosition);
            // grid.OnSelectedGridCellChange.AddListener(UpdatePreviewFieldPosition);

        }

        void ChangeFieldTargetPosition(Vector3 cellWorldPosition, AGridCell gridCell)
        {
            previewField.UpdatePreviewFieldPosition(cellWorldPosition);
            SetPreviewLayer(previewField.previewFieldTransform.gameObject);

            selectedGridCell = gridCell;
        }

        void ChangeField(float value)
        {
            fieldListIndex = (fieldListIndex + (int)Mathf.Clamp(value, -1f, 1f)) % fieldList.Count;
            if (fieldListIndex < 0)
            {
                fieldListIndex += fieldList.Count;
            }
            selectedField = fieldList[fieldListIndex];
            previewField.ChangePreviewField(selectedField.visual);
            SetPreviewLayer(previewField.previewFieldTransform.gameObject);

            // Debug.Log("IsFieldPlaceable(): " + IsFieldPlaceable());
            // OnSelectedFieldChange.Invoke(selectedField.visual, grid.GetCellSize());
        }

        public List<Vector3Int> GetNeededGridCellsIndexList()
        {
            List<Vector3Int> neededGridCellsIndexList = new List<Vector3Int>();
            Vector3Int xDirInWorld = Vector3Int.RoundToInt(previewField.previewFieldTransform.TransformDirection(Vector3.right));
            Vector3Int yDirInWorld = Vector3Int.RoundToInt(previewField.previewFieldTransform.TransformDirection(Vector3.up));
            Vector3Int zDirInWorld = Vector3Int.RoundToInt(previewField.previewFieldTransform.TransformDirection(Vector3.forward));
            // Vector3Int xDirInWorld = Vector3Int.RoundToInt(fieldTargetTransform.TransformDirection(Vector3.right));
            // Vector3Int yDirInWorld = Vector3Int.RoundToInt(fieldTargetTransform.TransformDirection(Vector3.up));
            // Vector3Int zDirInWorld = Vector3Int.RoundToInt(fieldTargetTransform.TransformDirection(Vector3.forward));

            for (int x = 0; x < selectedField.width; x++)
            {
                for (int y = 0; y < selectedField.height; y++)
                {
                    for (int z = 0; z < selectedField.length; z++)
                    {
                        Vector3Int neededGridCellIndex = (x * xDirInWorld + -y * yDirInWorld + z * zDirInWorld) + Vector3Int.FloorToInt(previewField.previewFieldTransform.position / _cellSize);
                        // Vector3Int neededGridCellIndex = (x * xDirInWorld + -y * yDirInWorld + z * zDirInWorld) + Vector3Int.FloorToInt(fieldTargetTransform.position);
                        neededGridCellsIndexList.Add(neededGridCellIndex);
                    }
                }
            }
            return neededGridCellsIndexList;
        }

        public List<AGridCell> GetNeededGridCellsList(List<Vector3Int> neededGridCellsIndexList)
        {
            List<AGridCell> neededGridCellsList = new List<AGridCell>();

            foreach (Vector3Int index in neededGridCellsIndexList)
            {
                neededGridCellsList.Add(grid.GetGridCell(index));
            }

            return neededGridCellsList;
        }

        public bool IsFieldPlaceable(List<Vector3Int> neededGridCellsIndexList)
        {
            return neededGridCellsIndexList.TrueForAll(index =>
            {
                AGridCell gridCell = grid.GetGridCell(index);
                return gridCell != null && (selectedField.isCharacter ? gridCell.IncludedField != null && gridCell.IncludedField.isWalkable : gridCell.CanBuild());
            });
        }

        public void TryPlaceField()
        {
            List<Vector3Int> neededGridCellsIndexList = GetNeededGridCellsIndexList();
            if (!IsFieldPlaceable(neededGridCellsIndexList)) return;

            List<AGridCell> neededGridCellsList = GetNeededGridCellsList(neededGridCellsIndexList);
            PlaceField(neededGridCellsList);
        }

        public void PlaceField(List<AGridCell> neededGridCellsList)
        {
            var fieldTransform = Instantiate(selectedField.transform, previewField.previewFieldTransform.position, Quaternion.identity, fieldsContainer);
            var field = fieldTransform.GetComponent<Field>();
            field.SetSize(_cellSize);
            field.SetOccupiedGridCells(neededGridCellsList);


            if (!field.isCharacter)
            {
                foreach (AGridCell gridCell in neededGridCellsList)
                {
                    gridCell.SetIncludedField(field);
                }

            }
        }

        void TryDeleteFields()
        {
            if (!selectedGridCell.CanBuild())
            {
                Field field = selectedGridCell.IncludedField;
                selectedGridCell.IncludedField.ClearOccupiedGridCells();
                Destroy(field.gameObject);
                SetPreviewLayer(previewField.previewFieldTransform.gameObject);
            }

        }

        // public void RotateField(Vector3Int dir)
        // {
        //     Vector3Int rotation = selectedField.GetRotationValue() * dir;
        //     Vector3 previewFieldlocalRotation = previewFieldTransform.InverseTransformVector(rotation);
        //     currentRotationGridCell *= Quaternion.Euler(previewFieldlocalRotation);

        //     OnFieldRotationChange.Invoke(currentRotationGridCell);
        //     // SetCurrentFieldPlaceable();
        // }



        //     public GridCell GetGridCellFromPosition(Vector3 position)
        //     {
        //         return grid.GetGridCell(position / cellSize);

        //     }

        //     public GridCell GetGridCellFromIndex(Vector3 position)
        //     {
        //         return grid.GetGridCell(position);

        //     }

        //     public float GetCellSize()
        //     {
        //         return cellSize;
        //     }

        void SetPreviewLayer(GameObject targetGameObject)
        {
            List<Vector3Int> neededGridCellsIndice = GetNeededGridCellsIndexList();
            if (IsFieldPlaceable(neededGridCellsIndice)) { SetLayerRecusrive(targetGameObject, 11); }
            else { SetLayerRecusrive(targetGameObject, 12); }
        }

        void SetLayerRecusrive(GameObject targetGameObject, int layer)
        {
            targetGameObject.layer = layer;
            foreach (Transform child in targetGameObject.transform)
            {
                SetLayerRecusrive(child.gameObject, layer);
            }
        }
    }
}
