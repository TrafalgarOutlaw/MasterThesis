using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using VRTRPG.Grid;

namespace VRTRPG.Movement
{
    public class MovementIndicator : MonoBehaviour
    {
        Material startMaterial;
        [SerializeField] Material hoverMaterial;
        [SerializeField] MeshRenderer meshRenderer;
        [SerializeField] XRSimpleInteractable xRSimpleInteractable;

        void Start()
        {
            startMaterial = meshRenderer.material;
        }

        public void SetHoverMaterial()
        {
            meshRenderer.material = hoverMaterial;
        }

        public void SetStartMaterial()
        {
            meshRenderer.material = startMaterial;
        }
    }
}
