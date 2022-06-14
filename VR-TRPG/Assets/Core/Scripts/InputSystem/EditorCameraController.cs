using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;


namespace VRTRPG.Input
{
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

            vrtrpgActions.Camera.Activate.performed += Toggle;
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

        public void Toggle(InputAction.CallbackContext obj)
        {
            isActive = !isActive;
        }

        public void Enable()
        {
            isActive = true;
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
}
