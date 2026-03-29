using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HelpCustomerObjectives : MonoBehaviour
{

    public bool checkFirstAid = false;
    public bool checkWaterGlass = false;

    public UnityEvent onObjectiveDone;

    public void enableFirstAid()
    {
        checkFirstAid = true;
    }

    public void enableCheckWaterGlass()
    {
        checkWaterGlass = true;
    }

    void Update()
    {
        if (checkFirstAid && checkFirstAid)
        {
            onObjectiveDone.Invoke();
            Debug.Log("Done!");
            Destroy(this);
        }
    }
}
