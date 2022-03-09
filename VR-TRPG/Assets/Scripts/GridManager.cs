using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField]
    int gridWidth = 10;
    [SerializeField]
    int gridHeight = 10;
    [SerializeField]
    float cellSize = 10f;

    [SerializeField]
    List<FieldSO> fieldSOList;
    FieldSO fieldSO;

    Grid<GridObject> _grid;
    FieldSO.Dir dir = FieldSO.Dir.Down;

    int fieldSOListIndex = 0;

    public event EventHandler OnSelectedChanged;
    Mouse mouse;

    void Awake()
    {
        Instance = this;
        _grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z));
        fieldSO = fieldSOList[fieldSOListIndex];
        mouse = Mouse.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (mouse.leftButton.wasPressedThisFrame)
        {
            _grid.GetXZ(Utils.GetMouseWorldPosition(), out int x, out int z);
            GridObject gridObject = _grid.GetGridObject(x, z);
            if (gridObject != null && gridObject.CanBuild())
            {
                Vector2Int rotationOffset = fieldSO.GetRotationOffset(dir);
                Vector3 placedFieldWorldPosition = _grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * _grid.GetCellSize();
                Transform builtTransform = Instantiate(fieldSO.prefab, placedFieldWorldPosition, Quaternion.Euler(0, fieldSO.GetRotationAngle(dir), 0));
                gridObject.SetField(builtTransform);
                builtTransform.GetComponent<Field>()?.SetSize(cellSize);
            }
        }

        if (mouse.rightButton.wasPressedThisFrame)
        {
            GridObject gridObject = _grid.GetGridObject(Utils.GetMouseWorldPosition());
            if (gridObject != null && gridObject.GetField() != null)
            {
                gridObject.ClearField();
            }
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            dir = FieldSO.GetNextDir(dir);
        }

        if (mouse.scroll.ReadValue().y != 0)
        {
            fieldSOListIndex = (fieldSOListIndex + (int)Mathf.Clamp(mouse.scroll.ReadValue().y, -1f, 1f)) % fieldSOList.Count;
            if (fieldSOListIndex < 0)
            {
                fieldSOListIndex += fieldSOList.Count;
            }
            fieldSO = fieldSOList[fieldSOListIndex];
            RefreshSelectedObjectType();
        }

    }

    void RefreshSelectedObjectType()
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = Utils.GetMouseWorldPosition();
        _grid.GetXZ(mousePosition, out int x, out int z);

        if (fieldSO != null)
        {
            Vector2Int rotationOffset = fieldSO.GetRotationOffset(dir);
            Vector3 placedFieldWorldPosition = _grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * _grid.GetCellSize();
            return placedFieldWorldPosition;
        }
        else
        {
            return mousePosition;
        }
    }

    public Quaternion GetPlacedFieldRotation()
    {
        if (fieldSO != null)
        {
            return Quaternion.Euler(0, fieldSO.GetRotationAngle(dir), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public FieldSO GetPlacedFieldSO()
    {
        return fieldSO;
    }

    public class GridObject
    {
        Grid<GridObject> _grid;
        int _x;
        int _z;
        Transform _transform;

        public void SetField(Transform transform)
        {
            _transform = transform;
            _grid.TriggerGridObjectChanged(_x, _z);
        }

        public Transform GetField()
        {
            return _transform;
        }

        public void ClearField()
        {
            Destroy(_transform.gameObject);
            _transform = null;
        }

        public bool CanBuild()
        {
            return _transform == null;
        }

        public GridObject(Grid<GridObject> grid, int x, int z)
        {
            _grid = grid;
            _x = x;
            _z = z;
        }

        public override string ToString()
        {
            return _x + ", " + _z + "\n" + _transform;
        }
    }
}
