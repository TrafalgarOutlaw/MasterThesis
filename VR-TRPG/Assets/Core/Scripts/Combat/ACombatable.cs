using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Grid;

namespace VRTRPG.Combat
{
    public abstract class ACombatable : MonoBehaviour
    {
        protected int health = 100;
        protected int attackDistance = 1;
        protected CombatSystem combatSystem;
        protected AGridCell CurrentCell;
        public bool IsPlayerTeam { get; protected set; }

        protected void Start()
        {
            combatSystem = CombatSystem.Instance;
            if (!transform.parent.TryGetComponent<AGridCell>(out CurrentCell))
            {
                print("CANT FIND CURRENT CELL");
            }

            IsPlayerTeam = false;
        }

        public int GetHealth()
        {
            return health;
        }

        public void Damage(int damageAmount)
        {
            health -= damageAmount;
        }

        public AGridCell GetCurrentCell()
        {
            return transform.parent.GetComponent<AGridCell>();
        }

        // Abstract
        public abstract HashSet<ACombatable> GetAvailableTarget();
        public abstract void StartCombatPhase();
        public abstract void Attack(AGridCell cell);
        public abstract void DoCombat();
    }
}
