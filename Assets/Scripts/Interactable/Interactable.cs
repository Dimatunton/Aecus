using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    QuickOutline outline;

    public bool showOutline = false;

    Coroutine showOnScan = null;


    public abstract void onInteract(Transform player);

    protected virtual void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        if(!TryGetComponent<QuickOutline>(out outline))
        {
            Debug.Log("No outline",gameObject);
        }

        //use for overiding start
        //    protected override void Start()
        //        {
        //            base.Start(); // runs Interactable.Start()
        //            Debug.Log("Door Start");
        //        }

    }

    protected virtual void Update()
    {
        if ((outline != null))
        {
            if (showOutline)
            {
                if(outline.OutlineWidth < 1f)
                {
                    outline.OutlineWidth += Time.deltaTime;
                }
            }
            else
            {
                if (outline.OutlineWidth > 0f)
                {
                    outline.OutlineWidth -= Time.deltaTime;
                }
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

    public void ScanShow()
    {
        if(outline != null)
        {
            if (showOnScan != null)
            {
                StopCoroutine(showOnScan);
                showOnScan = StartCoroutine(ShowOnScan());
            }
            else
            {
                showOnScan = StartCoroutine(ShowOnScan());
            }
        }
    }

    IEnumerator ShowOnScan()
    {
        showOutline = true;
        yield return new WaitForSeconds(4f);
        showOutline = false;
    }

}
