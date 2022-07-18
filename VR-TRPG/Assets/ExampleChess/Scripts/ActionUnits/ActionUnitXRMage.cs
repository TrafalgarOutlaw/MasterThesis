using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VRTRPG.Action;
using VRTRPG.Combat;
using VRTRPG.Grid;
using VRTRPG.Movement;
using VRTRPG.XR;

namespace VRTRPG.Chess.ActionUnit
{
    public class ActionUnitXRMage : AActionUnit
    {
        [SerializeField] XRUnit xRUnit;
        private XRSystem xrSystem;
        private MovementSystem movementSystem;
        private CombatSystem combatSystem;

        new void Start()
        {
            base.Start();
            xrSystem = XRSystem.Instance;
            movementSystem = MovementSystem.Instance;
            combatSystem = CombatSystem.Instance;
        }

        public override void DoAction()
        {
            throw new System.NotImplementedException();
        }

        public override void EndAction()
        {
            throw new System.NotImplementedException();
        }

        new public void OnSelectExit(SelectExitEventArgs args)
        {
            if (args.interactableObject.transform.TryGetComponent<MovementIndicator>(out MovementIndicator movementIndicator))
            {
                transform.parent = movementIndicator.transform.parent;
                transform.localPosition= Vector3.zero; 
                movementSystem.EndMovePhase();
                combatSystem.EndCombatPhase();
                actionSystem.CurrentAction.EndAction();
            }

            if (args.interactableObject.transform.TryGetComponent<CombatIndicator>(out CombatIndicator combatIndicator))
            {
                AGridCell cell = combatIndicator.transform.parent.GetComponent<AGridCell>();
                GameObject targetObject = cell.IncludedGameobjects.Find(go =>
                {
                    return go.GetComponent<ACombatable>() != null;
                });
                combatSystem.DoCombat(targetObject.GetComponent<ACombatable>());
                //movementSystem.EndMovePhase();
                //combatSystem.EndCombatPhase();
                //actionSystem.CurrentAction.EndAction();
            }
        }

        public override XRUnit GetXRUnit()
        {
            return xRUnit;
        }
    }
}
