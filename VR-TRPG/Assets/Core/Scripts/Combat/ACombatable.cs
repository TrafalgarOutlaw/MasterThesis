using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRTRPG.Grid;

namespace VRTRPG.Combat
{
    public abstract class ACombatable : MonoBehaviour
    {
        public UnityEvent OnHealthChanged;

        protected int health = 100;
        protected int attackDistance = 1;
        public int damageAmount = 20;
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
            if (health <= 0) Destroy(this.gameObject);
            OnHealthChanged.Invoke();
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
