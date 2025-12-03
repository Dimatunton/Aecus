using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class doObjective : Interactable
{
    public string ObjectiveTitle;
    public UnityEvent OnDone;
    public ObjectiveHandler ObjectiveHandler;
    public override void onInteract(Transform player)
    {
        {
            ObjectiveHandler.RemoveObjective(ObjectiveTitle);
            OnDone.Invoke();
            Destroy(this);
        }
        else
        {
            Debug.LogError("No Objective like that currently");
        }
    }
}
