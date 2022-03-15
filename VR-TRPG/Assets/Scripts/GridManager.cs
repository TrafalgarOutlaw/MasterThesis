using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance;

    public static GridManager Instance { get { return _instance; } }

    [SerializeField]
    int gridWidth = 10;
    [SerializeField]
    int gridHeight = 10;
    [SerializeField]
    float cellSize = 10f;

    [SerializeField]
    List<FieldSO> fieldSOList;
    FieldSO fieldSO;
    FieldSO startFieldSO;
    public Transform pfFreeGrid;

    Grid<GridObject> _grid;
    FieldSO.Dir dir = FieldSO.Dir.Down;

    int fieldSOListIndex = 0;

    [SerializeField]
    Transform level;

    public event EventHandler OnSelectedChanged;
    Mouse mouse;

    bool _playerIsPlaced = false;
    public Transform emptyGrids;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }

        _instance = this;
        _grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z));

        fieldSO = fieldSOList[fieldSOListIndex];
        mouse = Mouse.current;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector2Int rotationOffset = fieldSO.GetRotationOffset(dir);
                Vector3 freeGridPosition = _grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * _grid.GetCellSize();
                Transform freeGrid = Instantiate(pfFreeGrid, freeGridPosition, Quaternion.Euler(0, fieldSO.GetRotationAngle(dir), 0), emptyGrids);
                freeGrid.localScale = new Vector3(cellSize, cellSize, cellSize);
            }
        }
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
                Transform builtTransform;
                if (fieldSO.isPlayer)
                {
                    if (_playerIsPlaced)
                    {
                        Debug.Log("Player is already placed in level");
                        return;
                    }

                    _playerIsPlaced = true;
                    builtTransform = StartField.Instance.transform;
                    builtTransform.GetComponent<StartField>()?.SetSize(cellSize);
                    builtTransform.parent = level;
                    builtTransform.transform.position = placedFieldWorldPosition;

                    StartField.Instance.OnStartPlaced();
                    startFieldSO = fieldSO;
                    fieldSOList.RemoveAt(fieldSOListIndex);
                    fieldSOListIndex = 0;
                    fieldSO = fieldSOList[fieldSOListIndex];
                    RefreshSelectedObjectType();
                }
                else
                {
                    builtTransform = Instantiate(fieldSO.prefab, placedFieldWorldPosition, Quaternion.Euler(0, fieldSO.GetRotationAngle(dir), 0), level);
                    builtTransform.GetComponent<Field>()?.SetSize(cellSize);
                }
                gridObject.SetField(builtTransform);
            }
        }

        if (mouse.rightButton.wasPressedThisFrame)
        {
            GridObject gridObject = _grid.GetGridObject(Utils.GetMouseWorldPosition());
            if (gridObject != null && gridObject.GetField() != null)
            {
                Transform field = gridObject.GetField();
                if (field.CompareTag("Player"))
                {
                    _playerIsPlaced = false;
                    StartField.Instance.OnStartRemoved();
                    fieldSOList.Add(startFieldSO);
                    fieldSOListIndex = fieldSOList.Count - 1;
                    fieldSO = fieldSOList[fieldSOListIndex];
                    RefreshSelectedObjectType();
                }
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

    public bool IsPlayerPlaced()
    {
        return _playerIsPlaced;
    }

    public class GridObject
    {
        Grid<GridObject> _grid;
        int _x;
        int _z;
        Transform _transform;

        public GridObject(Grid<GridObject> grid, int x, int z)
        {
            _grid = grid;
            _x = x;
            _z = z;
        }

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
            if (!_transform.CompareTag("Player"))
            {
                Destroy(_transform.gameObject);
            }

            _transform = null;
        }

        public bool CanBuild()
        {
            return _transform == null;
        }

        public override string ToString()
        {
            return _x + ", " + _z + "\n" + _transform;
        }
    }
}
