using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectives : MonoBehaviour
{
    public string[] objectList;


    public void addObjectives()
    {
        StartCoroutine(addobjs());
    }

    IEnumerator addobjs()
    {
        ObjectivesManager.instance.clearObjectives();
        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < objectList.Length; i++)
        {
            ObjectivesManager.instance.addObjective(i, objectList[i]);
            yield return new WaitForSeconds(.5f);
        }
    }

}
