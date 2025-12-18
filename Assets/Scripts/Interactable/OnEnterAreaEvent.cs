using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaActivateTimeline : MonoBehaviour
{
    public UnityEvent onTimelineActivate;

    private void OnTriggerEnter(Collider other)
    {
        onTimelineActivate.Invoke();
    }
}
