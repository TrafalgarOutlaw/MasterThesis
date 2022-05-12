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
        public XRController CurrentController { get; private set; }
        private int currentControllerIndex;
        new Camera camera;

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

        public void EnableControllerIndex(int i)
        {
            camera.transform.position = controllerList[i].GetAnchorPosition();
            CurrentController = controllerList[i];
            currentControllerIndex = i;

            camera.transform.parent = CurrentController.transform;
        }

        public bool EnableNextControllerInList()
        {
            if (currentControllerIndex + 1 < controllerList.Count)
            {
                EnableControllerIndex(currentControllerIndex + 1);
                return true;
            }
            return false;
        }

        public bool EnablePreviousControllerInList()
        {
            if (currentControllerIndex - 1 >= 0)
            {
                EnableControllerIndex(currentControllerIndex - 1);
                return true;
            }
            return false;
        }
    }
}