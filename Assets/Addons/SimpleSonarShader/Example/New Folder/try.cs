using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class trty : MonoBehaviour
{
    [Tooltip("The sonar reveal shader property name for origin.")]
    public string sonarOriginProperty = "_SonarOrigin";

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void OnEnable()
    {
        TriggerSonar();
    }

    /// <summary>
    /// Triggers the sonar effect by updating the _SonarOrigin property.
    /// Call this method whenever you want to start a new sonar ring.
    /// </summary>
    public void TriggerSonar()
    {
        if (rend != null && rend.material != null && rend.material.HasProperty(sonarOriginProperty))
        {
            Vector3 origin = transform.position;
            float startTime = Time.time;
            rend.material.SetVector(sonarOriginProperty, new Vector4(origin.x, origin.y, origin.z, startTime));
        }
    }
}

