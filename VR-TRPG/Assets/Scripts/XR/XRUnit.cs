using UnityEngine;
using Unity.XR.CoreUtils;

namespace VRTRPG.XR
{
    public class XRUnit : MonoBehaviour
    {
        // public string name;
        XRSystem xrSystem;
        public XROrigin xrOrigin;
        [SerializeField] GameObject visual;

        void Start()
        {
            xrSystem = XRSystem.Instance;

            xrSystem.AddUnit(this);
        }

        private void OnDestroy()
        {
            xrSystem.RemoveUnit(this);
            // CommandSystem.Instance.RemoveCommandUnitFromList(this);
            // _XRSystem.RemoveController(this);
        }

        public void EnableVisual()
        {
            visual.SetActive(true);
        }

        public void DisableVisual()
        {
            visual.SetActive(false);
        }
    }
}