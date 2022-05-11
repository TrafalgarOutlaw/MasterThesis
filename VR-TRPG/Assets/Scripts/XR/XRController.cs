using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.XR
{
    public class XRController : MonoBehaviour
    {
        [SerializeField] Transform anchorTransform;
        XRSystem _XRSystem;

        void Start()
        {
            _XRSystem = XRSystem.Instance;

            _XRSystem.AddController(this);
        }

        private void OnDestroy()
        {
            _XRSystem.RemoveController(this);
        }

        public Transform GetXRAnchor()
        {
            return anchorTransform;
        }
    }
}