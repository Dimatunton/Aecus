using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaActivateTimeline : MonoBehaviour
{
    public UnityEvent onTimelineActivate;
    public GameObject timeline;

    private void OnTriggerEnter(Collider other)
    {
        onTimelineActivate.Invoke();
        if (timeline == null)
        {
            Debug.Log("NO TIMELINE");
        }
        else
        {
            timeline.SetActive(true);
        }
    }
}
