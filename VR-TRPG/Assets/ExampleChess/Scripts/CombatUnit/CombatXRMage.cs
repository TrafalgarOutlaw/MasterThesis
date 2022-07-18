using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Combat;
using VRTRPG.Grid;

namespace VRTRPG.Chess.CombatUnit
{
    public class CombatXRMage : ACombatable
    {
        [SerializeField] GameObject weapon;
        new void Start()
        {
            base.Start();
            IsPlayerTeam = true;
        }

        public override void StartCombatPhase()
        {
            combatSystem.StartCombatPhase(this);
        }

        public override HashSet<ACombatable> GetAvailableTarget()
        {
            HashSet<ACombatable> availableTargets = new HashSet<ACombatable>();

            foreach (var cell in CurrentCell.GetNeighbor())
            {
                GetAvailableTargetRecursive(cell, attackDistance).ForEach(value =>
                {
                    availableTargets.Add(value);
                });
            }

            return availableTargets;
        }

        List<ACombatable> GetAvailableTargetRecursive(AGridCell gridCell, int attackDistance)
        {
            List<ACombatable> availableTargets = new List<ACombatable>();

            ACombatable combatable = null;

            if (attackDistance <= 0) return availableTargets;
            if (gridCell == CurrentCell) return availableTargets;

            gridCell.IncludedGameobjects.ForEach(go =>
            {
                combatable = go.GetComponent<ACombatable>();
                if (combatable != null && !combatable.IsPlayerTeam)
                {
                    availableTargets.Add(combatable);
                }
            });

            foreach (var cell in gridCell.GetNeighbor())
            {
                availableTargets.AddRange(GetAvailableTargetRecursive(cell, attackDistance - 1));
            }
            return availableTargets;
        }

        public override void StartCombat()
        {
            CurrentCell = transform.parent.GetComponent<AGridCell>();
            combatSystem.ShowAvailableTargets();
        }

        public override void DoCombat(ACombatable target)
        {
            //target.Damage(damageAmount);
            weapon.SetActive(true);
            //print("ACTIVATE WEAPON");
        }
    }
}
