using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerArea : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    public string Tag = "";

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Tag != null || Tag != "")
        {
            if (Tag == other.tag)
            {
                onTriggerEnter.Invoke();
                Destroy(this);
            }
        }
        else
        {
            onTriggerEnter.Invoke();
            Destroy(this);
        }
    }
}
