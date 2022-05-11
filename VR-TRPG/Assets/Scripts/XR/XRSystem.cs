using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.XR
{
    public class XRSystem : MonoBehaviour
    {
        public static XRSystem Instance { get; private set; }
        public bool IsControllerPlaced { get; private set; }
        List<XRController> controllerList = new List<XRController>();
        private XRController currentController;
        private int currentControllerIndex;
        Camera camera;

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
            camera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public List<XRController> GetControllerList()
        {
            return controllerList;
        }

        public void AddController(XRController controller)
        {
            controllerList.Add(controller);
            IsControllerPlaced = true;
        }

        public void RemoveController(XRController controller)
        {
            controllerList.Remove(controller);
            if (controllerList.Count <= 0)
            {
                IsControllerPlaced = false;
            }
        }

        internal void DoAction()
        {
            EnableControllerIndex(0);
        }

        private void EnableControllerIndex(int i)
        {
            camera.transform.position = controllerList[i].GetXRAnchor().position;
            currentController = controllerList[i];
            currentControllerIndex = i;
        }

        public void EnableNextControllerInList()
        {
            if (currentControllerIndex + 1 < controllerList.Count)
            {
                EnableControllerIndex(currentControllerIndex + 1);
            }
        }

        public void EnablePreviousControllerInList()
        {
            if (currentControllerIndex - 1 >= 0)
            {
                EnableControllerIndex(currentControllerIndex - 1);
            }
        }
    }
}