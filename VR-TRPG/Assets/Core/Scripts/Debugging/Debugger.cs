using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VRTRPG.Movement;
using VRTRPG.Grid;
using VRTRPG.XR;
using VRTRPG.Action;
using VRTRPG.Place;
using VRTRPG.Input;

namespace VRTRPG.Debugger
{
    public class Debugger : MonoBehaviour
    {
        public GameObject inputSystem;
        public GameObject placeSystem;

        VrtrpgActions vrtrpgActions;
        private MovementSystem movementSystem;
        private XRSystem xrSystem;
        private ActionSystem actionSystem;
        private Mouse mouse;
        private Keyboard keyboard;
        bool isMovementDebug = false;
        bool isXRDebug = false;
        bool isActionDebug = false;
        public GameObject editorCamera;


        void Awake()
        {
            vrtrpgActions = new VrtrpgActions();
        }

        void Start()
        {
            movementSystem = MovementSystem.Instance;
            xrSystem = XRSystem.Instance;
            actionSystem = ActionSystem.Instance;
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


        public void ToggleXRDebug(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                isXRDebug = !isXRDebug;
                if (isXRDebug)
                {
                    if (!xrSystem.StartDebug()) { isXRDebug = false; return; }
                    // vrtrpgActions.Editor.Disable();
                    inputSystem.SetActive(false);
                    placeSystem.SetActive(false);
                    editorCamera.SetActive(false);
                }
                else
                {
                    inputSystem.SetActive(true);
                    placeSystem.SetActive(true);
                    // movementSystem.ClearIndicator();
                    xrSystem.EndDebug();
                    editorCamera.SetActive(true);
                    // vrtrpgActions.Editor.Enable();
                }
            }
        }

        public void ToggleActionDebug(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                isActionDebug = !isActionDebug;
                if (isActionDebug)
                {
                    if (!actionSystem.StartDebug()) { isActionDebug = false; return; }
                    inputSystem.SetActive(false);
                    placeSystem.SetActive(false);
                    placeSystem.GetComponent<PlaceSystem>().currentVisual.gameObject.SetActive(false);
                    editorCamera.SetActive(false);
                }
                else
                {
                    inputSystem.SetActive(true);
                    placeSystem.SetActive(true);
                    placeSystem.GetComponent<PlaceSystem>().currentVisual.gameObject.SetActive(true);
                    // movementSystem.ClearIndicator();
                    editorCamera.SetActive(true);
                    // vrtrpgActions.Editor.Enable();
                }
            }
        }

        // public void GetCellUnderMouse(InputAction.CallbackContext context)
        // {
        //     if (context.started)
        //     {
        //         if (isMovementDebug)
        //         {
        //             int layerMask = 1 << 6;
        //             Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        //             RaycastHit hit;
        //             if (Physics.Raycast(ray, out hit, 999f, layerMask))
        //             {
        //                 var cell = hit.collider.gameObject.transform.parent.GetComponent<AGridCell>();
        //                 WalkerMoveUnit walker = movementSystem.debugWalker;
        //                 if (movementSystem.CanWalkTo(walker, cell))
        //                 {
        //                     // movementSystem.WalkTo(walker, cell);
        //                     movementSystem.StartDebug();
        //                 }
        //             }
        //         }
        //     }
        //     // OnTryToMove.Invoke();
        // }
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
