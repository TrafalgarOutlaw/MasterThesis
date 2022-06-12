using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Grid;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using UnityEngine.Events;
// using Xr
// using Unity.X

namespace VRTRPG.Movement
{
    public class WalkIndicator : MonoBehaviour
    {
        [SerializeField] MeshRenderer debugRenderer;
        [SerializeField] Material selectionMaterial;
        [SerializeField] XRSimpleInteractable xRSimpleInteractable;
        Material hoverMaterial;

        void Start()
        {
            hoverMaterial = debugRenderer.material;
            // actionSystem = ActionSystem.Instance;
        }

        public void SetSelection()
        {
            debugRenderer.material = selectionMaterial;
        }

        public void SetHover()
        {
            debugRenderer.material = hoverMaterial;
        }

        public void SetSelectCallback(UnityAction<SelectExitEventArgs> callback)
        {
            xRSimpleInteractable.lastSelectExited.AddListener(callback);
        }
    }
}
