using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected virtual void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        //use for overiding start
        //    protected override void Start()
        //        {
        //            base.Start(); // runs Interactable.Start()
        //            Debug.Log("Door Start");
        //        }

    }
    public abstract void onInteract(Transform player);

}
