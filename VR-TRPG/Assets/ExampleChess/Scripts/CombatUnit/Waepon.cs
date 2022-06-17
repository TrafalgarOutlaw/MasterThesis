using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRTRPG.Chess.CombatUnit
{
    public class Waepon : MonoBehaviour
    {
         public void OnHoverEnter(HoverEnterEventArgs args)
        {
            print("HOVER OVER WEAPON");
            transform.parent = args.interactorObject.transform;
            transform.localPosition = Vector3.zero;
        }
    }
}
