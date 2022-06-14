using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VRTRPG.Action;
using VRTRPG.XR;
using VRTRPG.Movement;
using VRTRPG.Grid;
using VRTRPG.Combat;

namespace VRTRPG.Chess.ActionUnit
{
    public class ActionUnitSummoner : AActionUnit
    {
        [SerializeField] XRUnit xRUnit;
        [SerializeField] GameObject hoverIndicator;
        [SerializeField] GameObject selectIndicator;
        private XRSystem xrSystem;
        private MovementSystem movementSystem;
        private CombatSystem combatSystem;

        // WIP: Remove Ienmuerator in production
        new IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            base.Start();
            actionSystem.PushActionUnit(this);
            xrSystem = XRSystem.Instance;
            movementSystem = MovementSystem.Instance;
            combatSystem = CombatSystem.Instance;
        }

        public override void DoAction()
        {
            xrSystem.SelectUnit(xRUnit);
        }

        public override void EndAction()
        {
            actionSystem.RemoveActionUnit(this);
            actionSystem.InsertActionUnit(1, this);
            actionSystem.EndActionPhase();
        }

        new public void OnHoverEnter(HoverEnterEventArgs args)
        {
            if (args.interactableObject.transform.TryGetComponent<AGridMoveable>(out AGridMoveable moveable))
            {
                ActivateHover(args.interactableObject.transform);
            }

            if (args.interactableObject.transform.TryGetComponent<ACombatable>(out ACombatable combatable))
            {
                ActivateHover(args.interactableObject.transform);
            }
        }

        void ActivateHover(Transform parent)
        {
            hoverIndicator.SetActive(true);
            hoverIndicator.transform.parent = parent;
            hoverIndicator.transform.localPosition = new Vector3(.5f, -.5f, .5f);
            hoverIndicator.transform.localScale = Vector3.one * 1.01f;
        }

        new public void OnHoverExit(HoverExitEventArgs args)
        {
            if (args.interactableObject.transform.TryGetComponent<AGridMoveable>(out AGridMoveable moveable))
            {
                DeactivateHover();
            }

            if (args.interactableObject.transform.TryGetComponent<ACombatable>(out ACombatable combatable))
            {
                DeactivateHover();
            }
        }

        void DeactivateHover()
        {
            hoverIndicator.SetActive(false);
            hoverIndicator.transform.parent = transform;
        }

        new public void OnSelectEnter(SelectEnterEventArgs args)
        {
            if (args.interactableObject.transform.TryGetComponent<AGridMoveable>(out AGridMoveable moveable))
            {
                DeactivateHover();
                ActivateSelect(args.interactableObject.transform);

                movementSystem.StartMovePhase(moveable);
            }

            if (args.interactableObject.transform.TryGetComponent<ACombatable>(out ACombatable combatable))
            {
                DeactivateHover();
                ActivateSelect(args.interactableObject.transform);

                combatSystem.StartCombatPhase(combatable);
            }
        }

        void ActivateSelect(Transform parent)
        {
            selectIndicator.SetActive(true);
            selectIndicator.transform.parent = parent;
            selectIndicator.transform.localPosition = new Vector3(.5f, -.5f, .5f);
            selectIndicator.transform.localScale = Vector3.one * 1.01f;
        }

        void DeactivateSelect()
        {
            selectIndicator.SetActive(false);
            selectIndicator.transform.parent = transform;
        }

        new public void OnSelectExit(SelectExitEventArgs args)
        {
            if (args.interactableObject.transform.TryGetComponent<MovementIndicator>(out MovementIndicator movementIndicator))
            {
                movementSystem.MoveTo(movementIndicator.transform.parent.GetComponent<AGridCell>());
                movementSystem.EndMovePhase();
                combatSystem.EndCombatPhase();
                DeactivateSelect();
                EndAction();
            }

            if (args.interactableObject.transform.TryGetComponent<CombatIndicator>(out CombatIndicator combatIndicator))
            {
                AGridCell cell = combatIndicator.transform.parent.GetComponent<AGridCell>();
                GameObject targetObject = cell.IncludedGameobjects.Find(go =>
                {
                    return go.GetComponent<ACombatable>() != null;
                });
                combatSystem.AttackUnit(targetObject.GetComponent<ACombatable>());
                combatSystem.EndCombatPhase();
                movementSystem.EndMovePhase();
                DeactivateSelect();
                EndAction();
            }
        }
    }
}
