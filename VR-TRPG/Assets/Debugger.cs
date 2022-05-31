using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VRTRPG.Movement;
using VRTRPG.XR;

public class Debugger : MonoBehaviour
{
    private XRSystem _XRSystem;
    private MovementSystem movementSystem;

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
