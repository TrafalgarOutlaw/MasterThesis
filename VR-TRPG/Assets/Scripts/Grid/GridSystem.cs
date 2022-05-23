using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRTRPG.Grid
{
    [System.Serializable] public class OnGridEventVector3 : UnityEvent<Vector3> { }
    [System.Serializable] public class OnGridEventQuaternion : UnityEvent<Quaternion> { }
    [System.Serializable] public class OnGridEventFieldVisual : UnityEvent<FieldVisual, float> { }
    [System.Serializable] public class OnGridEventInt : UnityEvent<int> { }

    public class GridSystem : MonoBehaviour
    {
        // This instance
        private static GridSystem _instance;
        public static GridSystem Instance { get { return _instance; } }

        // Events
        public OnGridEventVector3 OnSelectedGridCellChange;
        public OnGridEventQuaternion OnFieldRotationChange;
        public OnGridEventFieldVisual OnSelectedFieldChange;

        // Grid
        Grid<GridCell> grid;
        [SerializeField] int gridLength = 10;
        [SerializeField] int gridHeight = 3;
        [SerializeField] int gridWidth = 10;
        [SerializeField] float cellSize = 10f;
        [SerializeField] Transform pfGridCell;
        public Transform gridGameObject;
        [SerializeField] int currentYLevel = 1;

        // Placeable fields
        [SerializeField] List<FieldSO> fieldSOList;
        int fieldSOListIndex = 0;
        FieldSO currentField;

        // Previewfield
        [SerializeField] Transform previewFieldTransform;

        // Insert into gridCells
        GridCell currentGridCell;
        Quaternion currentRotationGridCell = Quaternion.identity;
        List<GridCell> currentOccupiedGridCellList;
        public bool isCurrentFieldPlaceable;
        public Vector3 anchor;

        // Level
        Transform levelObject;
        InputManager inputManager;

        void Awake()
        {
            // Set Singleton
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            _instance = this;

            // Create GridObject
            grid = new Grid<GridCell>(gridLength, gridHeight, gridWidth, cellSize, Vector3.zero, pfGridCell, (Grid<GridCell> g, int x, int y, int z, Transform emptyGridObject) => new GridCell(g, x, y, z, emptyGridObject));
            currentGridCell = grid.GetGridCell(new Vector3(0, currentYLevel, 0));

            // Set selectedField
            currentField = fieldSOList[fieldSOListIndex];
        }

        private void Start()
        {
            inputManager = InputManager.Instance;
            inputManager.OnPlaceField.AddListener(TryPlaceField);
            inputManager.OnDeletField.AddListener(TryDeleteFields);
            inputManager.OnRotateField.AddListener(RotateField);
            inputManager.OnChangeLevel.AddListener(ChangeStage);
            inputManager.OnChangeField.AddListener(ChangeField);

            anchor = GetGridCellCenter(Vector3.zero);

            SetCurrentGridCell(currentGridCell);
        }

        void Update()
        {
            GridCell selectedGridCell = GetGridCellUnderMouse();
            if (selectedGridCell != currentGridCell)
            {
                SetCurrentGridCell(selectedGridCell);
            }
        }

        void ChangeField(float value)
        {
            fieldSOListIndex = (fieldSOListIndex + (int)Mathf.Clamp(value, -1f, 1f)) % fieldSOList.Count;
            if (fieldSOListIndex < 0)
            {
                fieldSOListIndex += fieldSOList.Count;
            }
            currentField = fieldSOList[fieldSOListIndex];

            SetCurrentFieldPlaceable();
            OnSelectedFieldChange.Invoke(currentField.fieldVisual, cellSize);
        }

        void ChangeStage(bool next)
        {
            if (next)
            {
                EnableNextStage();
            }
            else
            {
                EnablePreviousStage();
            }
        }

        void EnablePreviousStage()
        {
            DisableCurrentStage(currentYLevel);
            currentYLevel--;
            if (currentYLevel < 0)
            {
                currentYLevel += gridHeight;
            }
            EnableCurrentStage(currentYLevel);
        }

        void EnableNextStage()
        {
            DisableCurrentStage(currentYLevel);
            currentYLevel = (currentYLevel + 1) % gridHeight;
            EnableCurrentStage(currentYLevel);

        }

        private void SetCurrentGridCell(GridCell selectedGridCell)
        {
            currentGridCell.EnableRenderer();
            currentGridCell = selectedGridCell;
            selectedGridCell.DisableRenderer();
            Vector3 selectedGridObjectWordlPosition = selectedGridCell.GetWorldPosition();
            anchor = GetGridCellCenter(selectedGridObjectWordlPosition);
            OnSelectedGridCellChange.Invoke(anchor);

            SetCurrentFieldPlaceable();
            OnSelectedFieldChange.Invoke(currentField.fieldVisual, cellSize);
        }

        void SetOccupiedGridCellList()
        {
            List<GridCell> occupyGridCellList = new List<GridCell>();

            Vector3Int xDirInWorld = Vector3Int.RoundToInt(previewFieldTransform.TransformDirection(Vector3.right));
            Vector3Int yDirInWorld = Vector3Int.RoundToInt(previewFieldTransform.TransformDirection(Vector3.up));
            Vector3Int zDirInWorld = Vector3Int.RoundToInt(previewFieldTransform.TransformDirection(Vector3.forward));

            for (int x = 0; x < currentField.width; x++)
            {
                for (int y = 0; y < currentField.height; y++)
                {
                    for (int z = 0; z < currentField.length; z++)
                    {
                        Vector3Int occupiedGridCellIndex = (x * xDirInWorld + -y * yDirInWorld + z * zDirInWorld) + currentGridCell.GetIndex();
                        GridCell occupiedGridCell = grid.GetGridCell(occupiedGridCellIndex);
                        if (occupiedGridCell == null || !occupiedGridCell.CanBuild())
                        {
                            currentOccupiedGridCellList = null;
                            return;
                        }
                        occupyGridCellList.Add(occupiedGridCell);
                    }
                }
            }
            currentOccupiedGridCellList = occupyGridCellList;
            return;
        }

        internal FieldVisual GetFieldVisual()
        {
            return currentField.fieldVisual;
        }

        void SetCurrentFieldPlaceable()
        {
            SetOccupiedGridCellList();
            if (currentOccupiedGridCellList != null)
            {
                isCurrentFieldPlaceable = true;
            }
            else
            {
                isCurrentFieldPlaceable = false;
            }

        }

        public void TryPlaceField()
        {
            if (isCurrentFieldPlaceable)
            {
                PlaceField(currentField.pfField);
            }
        }

        private void PlaceField(Field pfField)
        {
            var fieldTransform = Instantiate(pfField.transform, previewFieldTransform.position, currentRotationGridCell, levelObject);
            var field = fieldTransform.GetComponent<Field>();
            field.SetSize(cellSize);

            foreach (var cell in currentOccupiedGridCellList)
            {
                cell.SetField(field, currentOccupiedGridCellList);
            }
        }

        void TryDeleteFields()
        {
            if (currentGridCell.CanBuild())
            {
                return;
            }

            Destroy(currentGridCell.GetPlacedField().gameObject);
            currentGridCell.ClearField();
        }

        public void RotateField(Vector3Int dir)
        {
            Vector3Int rotation = currentField.GetRotationValue() * dir;
            Vector3 previewFieldlocalRotation = previewFieldTransform.InverseTransformVector(rotation);
            currentRotationGridCell *= Quaternion.Euler(previewFieldlocalRotation);

            OnFieldRotationChange.Invoke(currentRotationGridCell);
            // SetCurrentFieldPlaceable();
        }

        private Vector3 GetGridCellCenter(Vector3 position)
        {
            return position + new Vector3(1, -1, 1) * cellSize * .5f;
        }

        public GridCell GetGridCellFromPosition(Vector3 position)
        {
            return grid.GetGridCell(position / cellSize);

        }

        public GridCell GetGridCellFromIndex(Vector3 position)
        {
            return grid.GetGridCell(position);

        }

        private GridCell GetGridCellUnderMouse()
        {
            Vector3 worldPosition = Utils.GetMouseWorldPosition();
            if (worldPosition == -Vector3.one)
            {
                return currentGridCell;
            }
            GridCell gridObject = grid.GetGridCell(worldPosition);
            return gridObject;
        }

        void EnableCurrentStage(int currentY)
        {
            for (int x = 0; x < gridLength; x++)
            {
                for (int z = 0; z < gridWidth; z++)
                {
                    grid.GetGridCell(new Vector3(x, currentY, z)).EnableRenderer();
                }
            }
        }

        void DisableCurrentStage(int currentY)
        {
            for (int x = 0; x < gridLength; x++)
            {
                for (int z = 0; z < gridWidth; z++)
                {
                    grid.GetGridCell(new Vector3(x, currentY, z)).DisableForMouse();
                }
            }
        }

        public float GetCellSize()
        {
            return cellSize;
        }
    }
}
