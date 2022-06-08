using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Action
{
    public class SummonerActionUnit : AActionUnit
    {
        public override void DoAction()
        {
            print("Action from Summoner");
            actionSystem.EndAction();
        }
    }
}
