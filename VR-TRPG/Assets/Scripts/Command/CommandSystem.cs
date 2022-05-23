using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Command
{
    public class CommandSystem : MonoBehaviour
    {
        [SerializeField] Transform commandContainer;
        [SerializeField] Transform pfCommandSlot;

        // This instance
        static CommandSystem _instance;
        public static CommandSystem Instance { get { return _instance; } }
        List<CommandUnit> commandUnitList = new List<CommandUnit>();
        Dictionary<CommandUnit, Transform> commandUnitSlotDict = new Dictionary<CommandUnit, Transform>();
        Dictionary<CommandUnit, string> commandUnitsNameDict = new Dictionary<CommandUnit, string>();

        void Start()
        {
            // Set Singleton
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            _instance = this;

            // Debug.Log("hey from command system");
        }

        void Update()
        {
            // GetAllCommandsFromQueue();
        }

        internal void SwapCommandUnits(int i1, int i2)
        {
            CommandUnit tmp = commandUnitList[i1];
            commandUnitList[i1] = commandUnitList[i2];
            commandUnitList[i2] = tmp;
            GetAllCommandUnitsFromList();
        }

        public void AddCommandUnitToList(CommandUnit commandUnit, string name)
        {
            commandUnitList.Add(commandUnit);
            Transform unitSlot = Instantiate(pfCommandSlot, commandContainer.position, Quaternion.identity, commandContainer);

            if (unitSlot.TryGetComponent<CommandSlot>(out CommandSlot commandSlot))
            {
                DragDrop dragDrop = commandSlot.GetCurrentDragDrop();
                dragDrop.SetText(name);
            }
            commandUnitSlotDict.Add(commandUnit, unitSlot);
            commandUnitsNameDict.Add(commandUnit, name);
            GetAllCommandUnitsFromList();
        }

        void GetAllCommandUnitsFromList()
        {
            Debug.Log("ALL COMMANDUNITS:");
            foreach (var command in commandUnitList)
            {
                Debug.Log("\t" + commandUnitsNameDict[command]);
            }
        }

        public void RemoveCommandUnitFromList(CommandUnit command)
        {
            Transform unitSlot = commandUnitSlotDict[command];
            commandUnitSlotDict.Remove(command);
            commandUnitsNameDict.Remove(command);
            Destroy(unitSlot.gameObject);
            commandUnitList.Remove(command);

            GetAllCommandUnitsFromList();
        }
    }

}
