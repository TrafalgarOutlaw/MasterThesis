using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRTRPG.XR
{
    public class XRSystem : MonoBehaviour
    {
        public static XRSystem Instance { get; private set; }
        public bool IsControllerPlaced { get; private set; }
        List<XRUnit> controllerList = new List<XRUnit>();
        public XRUnit CurrentController { get; private set; }
        private int currentControllerIndex;


        [SerializeField] Transform pfSummoner;
        Transform spectator;
        bool isSummoner = false;

        //_________________________________

        List<XRUnit> xrUnitList = new List<XRUnit>();
        private XRUnit debugXRUnit;
        public LocomotionSystem locosys;

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
        }

        // Update is called once per frame
        void Update()
        {

        }

        internal void AddUnit(XRUnit unit)
        {
            xrUnitList.Add(unit);
        }

        internal void RemoveUnit(XRUnit unit)
        {
            xrUnitList.Remove(unit);
        }

        public void SelectUnit(XRUnit unit)
        {
            unit.xrOrigin.gameObject.SetActive(true);
            debugXRUnit.DisableVisual();
            locosys.xrOrigin = unit.xrOrigin;
        }

        private void DeselectUnit()
        {
            debugXRUnit.xrOrigin.gameObject.SetActive(false);
            debugXRUnit.EnableVisual();
            debugXRUnit = null;
        }

        public bool StartDebug()
        {
            if (xrUnitList.Count == 0) { print("NO XRUnits"); return false; }
            debugXRUnit = xrUnitList[0];
            SelectUnit(debugXRUnit);
            return true;
        }

        public void EndDebug()
        {
            DeselectUnit();
        }

        public void SetSummoner()
        {
            if (isSummoner)
            {
                DestroyImmediate(spectator.gameObject);
                isSummoner = false;
            }
            else
            {
                spectator = Instantiate(pfSummoner, new Vector3(-100, 0, -100), Quaternion.identity);
                isSummoner = true;
            }
        }
    }
}