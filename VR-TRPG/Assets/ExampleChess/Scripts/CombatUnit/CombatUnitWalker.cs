using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Combat;
using VRTRPG.Grid;

namespace VRTRPG.Chess.CombatUnit
{
    public class CombatUnitWalker : ACombatable
    {

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
            CurrentCell = transform.parent.GetComponent<AGridCell>();


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
            return null;
        }

        public override void Attack(VRTRPG.Grid.AGridCell cell)
        {
            throw new System.NotImplementedException();
        }
    }
}
