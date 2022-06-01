using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VRTRPG.Movement;
using VRTRPG.Grid;

namespace VRTRPG.Debugger
{
    public class Debugger : MonoBehaviour
    {
        public GameObject inputSystem;
        public GameObject fieldSystem;

        VrtrpgActions vrtrpgActions;
        private MovementSystem movementSystem;

        private Mouse mouse;
        private Keyboard keyboard;
        bool isMovementDebug = false;

        void Awake()
        {
            vrtrpgActions = new VrtrpgActions();
        }

        void Start()
        {
            movementSystem = MovementSystem.Instance;
        }

        void OnEnable()
        {
            vrtrpgActions.Debugger.Enable();
            mouse = Mouse.current;
            keyboard = Keyboard.current;
        }

        void OnDisable()
        {
            vrtrpgActions.Debugger.Disable();
        }

        public void ToggleMovementDebug(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                isMovementDebug = !isMovementDebug;
                if (isMovementDebug)
                {
                    // vrtrpgActions.Editor.Disable();
                    inputSystem.SetActive(false);
                    fieldSystem.SetActive(false);
                    movementSystem.StartDebug();
                }
                else
                {
                    inputSystem.SetActive(true);
                    fieldSystem.SetActive(true);
                    movementSystem.ClearIndicator();
                    // vrtrpgActions.Editor.Enable();
                }
            }
        }

        public void GetCellUnderMouse(InputAction.CallbackContext context)
        {

            if (context.started)
            {
                if (isMovementDebug)
                {
                    int layerMask = 1 << 6;
                    Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 999f, layerMask))
                    {
                        var cell = hit.collider.gameObject.transform.parent.GetComponent<AGridCell>();
                        Walker walker = movementSystem.debugWalker;
                        if (movementSystem.CanWalkTo(walker, cell))
                        {
                            movementSystem.WalkTo(walker, cell);
                            movementSystem.StartDebug();
                        }
                    }
                }
            }
            // OnTryToMove.Invoke();
        }
    }
    // private XRSystem _XRSystem;
    // private MovementSystem movementSystem;

    // Start is called before the first frame update
    // void Start()
    // {
    //     _XRSystem = XRSystem.Instance;
    //     movementSystem = MovementSystem.Instance;
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (Keyboard.current.pKey.wasPressedThisFrame)
    //     {
    //         if (_XRSystem.EnableNextControllerInList())
    //         {
    //             PlayerCharacter character = _XRSystem.CurrentController.GetPlayerObject()?.GetComponent<PlayerCharacter>();
    //             if (character != null)
    //             {
    //                 movementSystem.StartMovePhase(character);
    //             }
    //             else
    //             {
    //                 movementSystem.StartSelectionPhase();
    //             }
    //         }
    //     }

    //     if (Keyboard.current.oKey.wasPressedThisFrame)
    //     {
    //         if (_XRSystem.EnablePreviousControllerInList())
    //         {
    //             PlayerCharacter character = _XRSystem.CurrentController.GetPlayerObject()?.GetComponent<PlayerCharacter>();
    //             if (character != null)
    //             {
    //                 movementSystem.StartMovePhase(character);
    //             }
    //             else
    //             {
    //                 movementSystem.StartSelectionPhase();
    //             }
    //         }
    //     }
    // }
}
