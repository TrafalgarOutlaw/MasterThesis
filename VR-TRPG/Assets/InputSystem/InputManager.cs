using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;
using System;

[System.Serializable]
public class RotateEvent : UnityEvent<Vector3Int>
{
}

[System.Serializable]
public class BoolEvent : UnityEvent<bool>
{
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get { return _instance; } }


    public RotateEvent OnRotateField;
    public BoolEvent OnChangeLevel;

    public UnityEvent OnPlaceField;
    public UnityEvent OnDeletField;

    VrtrpgActions vrtrpgActions;

    Mouse mouse;
    Keyboard keyboard;
    private static InputManager _instance;

    bool isRotating = true;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        _instance = this;

        vrtrpgActions = new VrtrpgActions();
        vrtrpgActions.Editor.Enable();
        OnPlaceField = new UnityEvent();
        OnDeletField = new UnityEvent();
        OnRotateField = new RotateEvent();

        vrtrpgActions.Editor.RotateField.started += Rotate;

        mouse = Mouse.current;
        keyboard = Keyboard.current;
    }


    private void Update()
    {
        if (mouse.leftButton.wasPressedThisFrame)
        {
            OnPlaceField.Invoke();
        }
        if (mouse.rightButton.wasPressedThisFrame)
        {
            OnDeletField.Invoke();
        }

        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            OnChangeLevel.Invoke(true);
        }
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            OnChangeLevel.Invoke(false);
        }
    }

    public Vector3 GetMousePosition()
    {
        return Mouse.current.position.ReadValue();
    }

    public Vector2 GetMouseScrollValue()
    {
        return mouse.scroll.ReadValue();
    }
    public void ToggleRotationMode(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRotating = !isRotating;
        }
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        if (isRotating)
        {
            OnRotateField.Invoke(Vector3Int.FloorToInt(context.ReadValue<Vector3>()));
        }
    }
}