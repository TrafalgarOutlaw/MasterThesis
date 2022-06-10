using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Action
{
    public class EnemyWalkerActionUnit : AActionUnit
    {
        public override bool IsXR { get; protected set; }

        void Awake()
        {
            IsXR = false;
        }

        public override void DoAction()
        {
            print("ACTION FROM ENEMY WALKER");
        }

        public override void EndAction()
        {
            print("END ACTION FROM ENEMY WALKER");
        }
    }
}
