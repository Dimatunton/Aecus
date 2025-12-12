using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    QuickOutline outline;

    public bool showOutline = false;
    protected virtual void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        if(!TryGetComponent<QuickOutline>(out outline))
        {
            print("no outline");
        }

        //use for overiding start
        //    protected override void Start()
        //        {
        //            base.Start(); // runs Interactable.Start()
        //            Debug.Log("Door Start");
        //        }

    }
    public abstract void onInteract(Transform player);

    protected virtual void Update()
    {

        if ((outline != null))
        {
            if (showOutline)
            {
                outline.enabled = true;
            }
            else
            {
                outline.enabled = false;
            }
        }
    }

    private void OnDisable()
    {
        showOutline = false;
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}
