using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.XR;
using VRTRPG.Movement;
using UnityEngine.XR.Interaction.Toolkit;
using VRTRPG.Grid;

namespace VRTRPG.Action
{
    public class WalkerActionUnit : AActionUnit
    {
        public override bool IsXRDebug { get; protected set; }
        [SerializeField] XRUnit xRUnit;
        [SerializeField] GameObject hoverIndicator;
        [SerializeField] GameObject selectionIndicator;
        [SerializeField] WalkerMoveUnit walkerMoveUnit;
        private MovementSystem movementSystem;

        void Awake()
        {
            IsXRDebug = false;
        }

        new void Start()
        {
            base.Start();
            movementSystem = MovementSystem.Instance;
        }

        public override void DoAction()
        {
            XRSystem.Instance.SelectUnit(xRUnit);
        }

        public override void EndAction()
        {
            actionSystem.CurrentAction.EndAction();
        }

        public void ActivateHoverIndicator()
        {
            hoverIndicator.SetActive(true);
        }

        public void ActivateSelectionIndicator()
        {
            selectionIndicator.SetActive(true);
            DeactivateHoverIndicator();
        }

        public void DeactivateHoverIndicator()
        {
            hoverIndicator.SetActive(false);
        }

        public void DeactivateSelectionIndicator()
        {
            hoverIndicator.SetActive(false);
            selectionIndicator.SetActive(false);
        }

        public override void SelectUnit()
        {
            // actionSystem.InsertAction(1, this);
            ActivateSelectionIndicator();
            movementSystem.ShowWalkableFields(walkerMoveUnit, (SelectExitEventArgs callback) =>
            {
                MoveToIndicator(callback.interactableObject.transform);
            });
        }

        public void MoveToIndicator(Transform transform)
        {
            if (!transform.parent.TryGetComponent<AGridCell>(out AGridCell cell)) return;
            MovementSystem.Instance.MoveTo(cell);
            DeactivateSelectionIndicator();
            EndAction();
        }
    }
}
