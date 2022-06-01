using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Grid
{
    public class EditorUI : MonoBehaviour
    {
        [SerializeField] GameObject currenContainer;
        [SerializeField] GameObject insertContainer;
        [SerializeField] GameObject cameraContainer;
        [SerializeField] GameObject rotateContainer;
        [SerializeField] GameObject levelContainer;

        InputManager inputManager;

        // private void Start()
        // {
        //     inputManager = InputManager.Instance;
        //     inputManager.OnInsertMode.AddListener(SetInsertUI);
        //     inputManager.OnCameraMode.AddListener(SetCameraUI);
        //     inputManager.OnRotateMode.AddListener(SetRotateUI);
        //     inputManager.OnLevelMode.AddListener(SetLevelUI);
        // }

        // private void SetLevelUI()
        // {
        //     currenContainer.SetActive(false);
        //     currenContainer = levelContainer;
        //     currenContainer.SetActive(true);
        // }

        // private void SetRotateUI()
        // {
        //     currenContainer.SetActive(false);
        //     currenContainer = rotateContainer;
        //     currenContainer.SetActive(true);
        // }

        // private void SetCameraUI()
        // {
        //     currenContainer.SetActive(false);
        //     currenContainer = cameraContainer;
        //     currenContainer.SetActive(true);
        // }

        // private void SetInsertUI()
        // {
        //     currenContainer.SetActive(false);
        //     currenContainer = insertContainer;
        //     currenContainer.SetActive(true);
        // }
    }
}
