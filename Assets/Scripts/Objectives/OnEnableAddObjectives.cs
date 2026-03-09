using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableAddObjectives : MonoBehaviour
{
    public UnityEvent onEnable;

    public ObjectivesManager ObjectivesManager;
    public string[] objectList;


    private void OnEnable()
    {
        onEnable.Invoke();
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
