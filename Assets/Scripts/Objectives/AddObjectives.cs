using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectives : MonoBehaviour
{
    public ObjectivesManager ObjectivesManager;
    public string[] objectList;


    private void OnEnable()
    {
        StartCoroutine(addobjs());
    }

    IEnumerator addobjs()
    {
        ObjectivesManager.clearObjectives();
        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < objectList.Length; i++)
        {
            ObjectivesManager.addObjective(i, objectList[i]);
            yield return new WaitForSeconds(.5f);
        }
    }

}
