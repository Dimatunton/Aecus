using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneObjective : MonoBehaviour
{
    public ObjectivesManager ObjectivesManager;
    public string[] objectList;

    private void doneObjective()
    {
        for (int i = 0; i < objectList.Length; i++)
        {
            ObjectivesManager.doneObjective(objectList[i]);
        }
    }
}
