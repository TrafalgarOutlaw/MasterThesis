using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Grid
{
    public class FieldVisual : MonoBehaviour
    {
        [SerializeField] Transform visual;
        [SerializeField] BoxCollider gridObjectCollider;
        List<Vector3> occupiedGridObjects;

        public void SetSize(float cellSize)
        {
            visual.localScale = Vector3.Scale(visual.localScale, new Vector3(cellSize, cellSize, cellSize));
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("COLLIDED");
            if (other.gameObject.layer == 6 || other.gameObject.layer == 7)
            {
                Debug.Log("ADD TRIGGER");
                // occupiedGridObjects.Add(other.GetComponentInParent<EmptyGridObject>().index);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("TRIGGER EXIT");
            if (other.gameObject.layer == 6 || other.gameObject.layer == 7)
            {
                occupiedGridObjects.Remove(other.GetComponentInParent<EmptyGridObject>().index);
            }
        }
    }
}
