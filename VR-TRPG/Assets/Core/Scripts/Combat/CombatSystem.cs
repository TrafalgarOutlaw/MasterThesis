using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Grid;

namespace VRTRPG.Combat
{
    public class CombatSystem : MonoBehaviour
    {
        [SerializeField] Transform pfTargetIndicator;
        List<Transform> indicatorList = new List<Transform>();
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
            ClearIndicators();

            combatable.DoCombat();
        }

        public void EndCombatPhase()
        {
            CurrentCombatable = null;
            ClearIndicators();
        }

        public void AttackUnit(ACombatable target)
        {
            print(CurrentCombatable.name + " attacks " + target.name);
            target.Damage(CurrentCombatable.damageAmount);
        }

        private void ClearIndicators()
        {
            indicatorList.ForEach(indicator => Destroy(indicator.gameObject));
            indicatorList.Clear();
        }

        public void ShowAvailableTargets()
        {
            List<ACombatable> availableTargets = new List<ACombatable>(CurrentCombatable.GetAvailableTarget());

            availableTargets.ForEach(target =>
            {
                AGridCell cell = target.GetCurrentCell();
                Transform targetIndicator = Instantiate(pfTargetIndicator, cell.CellTopSide, Quaternion.identity, cell.transform);
                indicatorList.Add(targetIndicator);
            });
        }
    }
}