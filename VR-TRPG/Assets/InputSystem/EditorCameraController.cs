using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class EditorCameraController : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float sensitivity;
    [SerializeField]
    bool isLooking;
    [SerializeField]

    VrtrpgActions vrtrpgActions;
    bool isActive;

    private void Awake()
    {
        vrtrpgActions = new VrtrpgActions();
        vrtrpgActions.Camera.Enable();

        vrtrpgActions.Camera.Activate.performed += Activate;
        vrtrpgActions.Camera.Look.started += StartLooking;
        vrtrpgActions.Camera.Look.canceled += EndLooking;
    }

    private void EndLooking(InputAction.CallbackContext obj)
    {
        isLooking = false;
    }

    private void StartLooking(InputAction.CallbackContext obj)
    {
        isLooking = true;
    }

    private void Activate(InputAction.CallbackContext obj)
    {
        isActive = !isActive;
    }

    private void Update()
    {
        if (isActive)
        {
            Vector3 movementVector = vrtrpgActions.Camera.Move.ReadValue<Vector3>() * Time.deltaTime * speed;
            transform.Translate(movementVector, Space.Self);

            if (isLooking)
            {
                LookAt();
            }
        }
    }

    private void LookAt()
    {
        float mouseX = vrtrpgActions.Camera.MouseX.ReadValue<float>() * sensitivity;
        float mouseY = vrtrpgActions.Camera.MouseY.ReadValue<float>() * sensitivity;
        transform.Rotate(Vector3.up, mouseX * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.right, -mouseY * Time.deltaTime);
    }
}
