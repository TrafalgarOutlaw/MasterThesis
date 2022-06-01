using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;

namespace VRTRPG.Grid
{
    [System.Serializable] public class RotateEvent : UnityEvent<Vector3Int> { }

    [System.Serializable] public class BoolEvent : UnityEvent<bool> { }
    [System.Serializable] public class FloatEvent : UnityEvent<float> { }

    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get { return _instance; } }


        public RotateEvent OnRotateField;
        public BoolEvent OnChangeLevel;
        public FloatEvent OnSelectedFieldChanged;
        public UnityEvent OnTryToMove;

        public UnityEvent OnPlaceField;
        public UnityEvent OnDeletField;

        // UI Events
        public UnityEvent OnInsertMode;
        public UnityEvent OnRotateMode;
        public UnityEvent OnLevelMode;
        public UnityEvent OnCameraMode;


        VrtrpgActions vrtrpgActions;

        Mouse mouse;
        Keyboard keyboard;

        Vector2 mouseScrollValue;
        private static InputManager _instance;

        bool isRotateMode;
        public bool isCameraMode;
        private bool isInsertMode = true;
        private bool isLevelMode;

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
                PlaceField();
            }
            if (mouse.rightButton.wasPressedThisFrame)
            {
                DeleteField();
            }

            if (Keyboard.current.tKey.wasPressedThisFrame)
            {
                ChangeLevel(true);
            }
            if (Keyboard.current.gKey.wasPressedThisFrame)
            {
                ChangeLevel(false);
            }


            mouseScrollValue = GetMouseScrollValue();
            if (mouseScrollValue.y != 0)
            {
                ChangeField(mouseScrollValue.y);
            }
        }

        private void ChangeField(float value)
        {
            if (isInsertMode)
            {
                OnSelectedFieldChanged.Invoke(value);
            }
        }

        private void PlaceField()
        {
            if (isInsertMode)
            {
                OnPlaceField.Invoke();
            }
        }

        private void DeleteField()
        {
            if (isInsertMode)
            {
                OnDeletField.Invoke();
            }
        }

        private void ChangeLevel(bool next)
        {
            if (isLevelMode)
            {
                OnChangeLevel.Invoke(next);
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

        public void DisableAllModes()
        {
            isCameraMode = false;
            isRotateMode = false;
            isInsertMode = false;
            isLevelMode = false;
        }

        public void EnableCameraMode()
        {
            DisableAllModes();
            isCameraMode = true;
        }

        public void CameraMode(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                DisableAllModes();
                isCameraMode = true;
                OnCameraMode.Invoke();
            }
        }

        public void InsertMode(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                DisableAllModes();
                isInsertMode = true;
                OnInsertMode.Invoke();
            }
        }

        public void RotateMode(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                DisableAllModes();
                isRotateMode = true;
                OnRotateMode.Invoke();
            }
        }

        public void LevelMode(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                DisableAllModes();
                isLevelMode = true;
                OnLevelMode.Invoke();
            }
        }

        public void Rotate(InputAction.CallbackContext context)
        {
            if (isRotateMode)
            {
                OnRotateField.Invoke(Vector3Int.FloorToInt(context.ReadValue<Vector3>()));
            }
        }
    }
}
