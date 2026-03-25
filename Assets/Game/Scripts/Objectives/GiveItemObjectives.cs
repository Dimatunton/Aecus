using UnityEngine;
using UnityEngine.Events;

public class GiveItemObjectives : MonoBehaviour
{

    public string QuestItem = "";

    public UnityEvent onDone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == QuestItem)
        {
            onDone.Invoke();
            Destroy(other.gameObject);
            Destroy(this);
        }
    }
}
