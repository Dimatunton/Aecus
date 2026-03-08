using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectives : MonoBehaviour
{
    public ObjectivesManager ObjectivesManager;
    public string[] objectList;

    private void OnEnable()
    {
        ObjectivesManager.clearObjectives();

        StartCoroutine(addobjs());
    }

    IEnumerator addobjs()
    {
        for (int i = 0; i < objectList.Length; i++)
        {
            ObjectivesManager.addObjective(i, objectList[i]);
            yield return new WaitForSeconds(1f);
        }
    }

}
