using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.XR
{
    public class XRController : MonoBehaviour
    {
        [SerializeField] Transform anchorTransform;
        [SerializeField] bool isSpectator;
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

        public Vector3 GetAnchorPosition()
        {
            return anchorTransform.position;
        }

        public GameObject GetPlayerObject()
        {
            if (isSpectator)
            {
                return null;
            }
            return transform.parent.gameObject;
        }
    }
}