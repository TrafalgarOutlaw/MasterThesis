using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class RotateEvent : UnityEvent<Vector3Int>
{
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get { return _instance; } }


    public RotateEvent OnRotateField;

    public UnityEvent OnPlaceField;
    public UnityEvent OnDeletField;

    VrtrpgActions vrtrpgActions;

    Mouse mouse;
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