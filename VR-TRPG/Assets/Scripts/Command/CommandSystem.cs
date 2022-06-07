using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Command
{
    public class CommandSystem : MonoBehaviour
    {
        // [SerializeField] Transform commandContainer;
        // [SerializeField] Transform pfCommandSlot;

        // This instance
        static CommandSystem _instance;
        public static CommandSystem Instance { get { return _instance; } }

        // List<CommandUnit> commandUnitList = new List<CommandUnit>();
        // Dictionary<CommandUnit, Transform> commandUnitSlotDict = new Dictionary<CommandUnit, Transform>();
        // Dictionary<CommandUnit, string> commandUnitsNameDict = new Dictionary<CommandUnit, string>();

        void Start()
        {
            // Set Singleton
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            _instance = this;
        }

        //         internal void SwapCommandUnitsInList(int i1, int i2)
        //         {
        //             CommandUnit tmp = commandUnitList[i1];
        //             commandUnitList[i1] = commandUnitList[i2];
        //             commandUnitList[i2] = tmp;
        //             GetAllCommandUnitsFromList();
        //         }

        //         public void RemoveCommandUnitFromList(CommandUnit command)
        //         {
        //             Transform unitSlot = commandUnitSlotDict[command];
        //             commandUnitSlotDict.Remove(command);
        //             commandUnitsNameDict.Remove(command);
        //             Destroy(unitSlot.gameObject);
        //             commandUnitList.Remove(command);

        //             GetAllCommandUnitsFromList();
        //         }
    }

}
