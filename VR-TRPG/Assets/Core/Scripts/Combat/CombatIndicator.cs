using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Combat
{
    public class CombatIndicator : MonoBehaviour
    {
        Material startMaterial;
        [SerializeField] Material hoverMaterial;
        [SerializeField] MeshRenderer meshRenderer;

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
