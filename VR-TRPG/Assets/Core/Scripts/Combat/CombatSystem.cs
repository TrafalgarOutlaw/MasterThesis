using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Grid;

namespace VRTRPG.Combat
{
    public class CombatSystem : MonoBehaviour
    {
        public static CombatSystem Instance { get; private set; }
        GridSystem gridSystem;
        private ACombatable CurrentCombatable;

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

        public void StartCombatPhase(ACombatable combatable)
        {
            if (combatable != null && CurrentCombatable == combatable) return;

            CurrentCombatable = combatable;
            HashSet<ACombatable> availableTargets = combatable.GetAvailableTarget();
        }
    }
}
