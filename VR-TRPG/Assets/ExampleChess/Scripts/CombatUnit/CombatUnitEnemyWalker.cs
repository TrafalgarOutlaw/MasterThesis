using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Combat;
using VRTRPG.Grid;

namespace VRTRPG.Chess.CombatUnit
{
    public class CombatUnitEnemyWalker : ACombatable
    {

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
                if (combatable != null && combatable.IsPlayerTeam)
                {
                    availableTargets.Add(combatable);
                }
            });

            foreach (var cel in gridCell.GetNeighbor())
            {
                availableTargets.AddRange(GetAvailableTargetRecursive(gridCell, attackDistance - 1));
            }
            return availableTargets;
        }

        public override void StartCombat()
        {
            CurrentCell = transform.parent.GetComponent<AGridCell>();
            List<ACombatable> availableTargets = new List<ACombatable>(GetAvailableTarget());
            if (availableTargets.Count == 0)
            {
                return;
            }
            DoCombat(availableTargets[0]);
        }
        public override void DoCombat(ACombatable target)
        {
            target.Damage(damageAmount);
            combatSystem.EndCombatPhase();
        }
    }
}
