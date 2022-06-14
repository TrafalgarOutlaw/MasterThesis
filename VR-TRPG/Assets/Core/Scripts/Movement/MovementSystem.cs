using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using VRTRPG.Grid;

namespace VRTRPG.Movement
{
    public class MovementSystem : MonoBehaviour
    {
        public static MovementSystem Instance { get; private set; }
        GridSystem gridSystem;
        [SerializeField] Transform pfMovementIndicator;
        List<Transform> indicatorList = new List<Transform>();
        private AGridMoveable currentMoveable;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
        }

        void Start()
        {
            gridSystem = GridSystem.Instance;
        }

        public void StartMovePhase(AGridMoveable moveable)
        {
            if (currentMoveable != null && currentMoveable == moveable) return;
            currentMoveable = moveable;
            ClearIndicators();
            moveable.DoMove();
        }

        public void EndMovePhase()
        {
            currentMoveable = null;
            ClearIndicators();
        }

        public AGridMoveable GetCurrentMoveable()
        {
            return currentMoveable;
        }

        void ClearIndicators()
        {
            indicatorList.ForEach(indicator => Destroy(indicator.gameObject));
            indicatorList.Clear();
        }

        public void MoveTo(AGridCell cell)
        {
            currentMoveable.MoveTo(cell);
        }

        public void ShowWalkableCells()
        {
            HashSet<AGridCell> walkableCellSet = currentMoveable.GetAvailableCells();
            foreach (var cell in walkableCellSet)
            {
                Transform moveIndicator = Instantiate(pfMovementIndicator, cell.CellTopSide, Quaternion.identity, cell.transform);
                indicatorList.Add(moveIndicator);
            }
        }
    }
}