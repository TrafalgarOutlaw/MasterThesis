using System;
using UnityEngine;

namespace VRTRPG.Grid
{
    [CreateAssetMenu(fileName = "FieldSO", menuName = "ScriptableObjects/FieldSO")]
    public class FieldSO : ScriptableObject
    {
        public Field pfField;
        public string nameString;
        public int width = 1;
        public int height = 1;
        public int length = 1;
        public FieldVisual fieldVisual;
        public bool isWalkable;
        public bool isPlayer;
        public Vector3 currentWorldPosition;
        public int rotationValue = 90;

        public Vector3 GetSideLengths()
        {
            return new Vector3(width, height, length);
        }

        internal object GetCurrentWorldPosition()
        {
            return currentWorldPosition;
        }

        public int GetRotationValue()
        {
            return rotationValue;
        }
    }
}
