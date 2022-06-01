using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Command;

namespace VRTRPG.XR
{
    public class XRController : MonoBehaviour, CommandUnit
    {
        // public string name;
        [SerializeField] Transform anchorTransform;
        [SerializeField] bool isSpectator;
        XRSystem _XRSystem;

        void Start()
        {
            _XRSystem = XRSystem.Instance;
            _XRSystem.AddController(this);

            name += "\nXR";
            CommandSystem.Instance.AddCommandUnitToList(this, name);
        }

        private void OnDestroy()
        {
            CommandSystem.Instance.RemoveCommandUnitFromList(this);
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