using System.Collections.Generic;
using UnityEngine;

public class SetChildMaterial : MonoBehaviour
{
    [Tooltip("The sonar reveal shader name.")]
    public string sonarShaderName = "MadeByProfessorOakie/SimpleSonarShaderReveal";

    [Tooltip("Optional: Set a custom hidden color.")]
    public Color hiddenColor = Color.black;

    [Tooltip("Optional: Set a custom ring color.")]
    public Color ringColor = Color.white;

    [Tooltip("Optional: Set a custom ring width.")]
    public float ringWidth = 0.1f;

    [Tooltip("Optional: Set a custom ring speed.")]
    public float ringSpeed = 1f;

    [Tooltip("Optional: Set a custom ring intensity.")]
    public float ringIntensity = 1f;

    void OnEnable()
    {
        Shader sonarShader = Shader.Find(sonarShaderName);
        if (sonarShader == null)
        {
            Debug.LogError("Sonar shader not found: " + sonarShaderName);
            return;
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);

        foreach (Renderer rend in renderers)
        {
            if (rend.sharedMaterial == null)
                continue;

            Material newMat = new Material(sonarShader);

            if (rend.sharedMaterial.HasProperty("_MainTex"))
                newMat.SetTexture("_MainTex", rend.sharedMaterial.GetTexture("_MainTex"));
            if (rend.sharedMaterial.HasProperty("_Color"))
                newMat.SetColor("_Color", rend.sharedMaterial.GetColor("_Color"));

            newMat.SetColor("_HiddenColor", hiddenColor);
            newMat.SetColor("_RingColor", ringColor);
            newMat.SetFloat("_RingWidth", ringWidth);
            newMat.SetFloat("_RingSpeed", ringSpeed);
            newMat.SetFloat("_RingIntensity", ringIntensity);

            rend.material = newMat;
        }
    }
}

[RequireComponent(typeof(Renderer))]
public class SonarMultiRingController : MonoBehaviour
{
    public string sonarOriginsProperty = "_SonarOrigins";
    public string sonarCountProperty = "_SonarCount";
    public int maxRings = 20;

    private Renderer rend;
    private List<Vector4> rings = new List<Vector4>();

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Remove expired rings (optional, based on your effect duration)
        // For demo, keep all rings

        // Update shader arrays
        if (rend != null && rend.material != null)
        {
            Vector4[] arr = new Vector4[maxRings];
            int count = Mathf.Min(rings.Count, maxRings);
            for (int i = 0; i < count; i++)
                arr[i] = rings[i];
            rend.material.SetVectorArray(sonarOriginsProperty, arr);
            rend.material.SetInt(sonarCountProperty, count);
        }
    }

    public void TriggerSonar()
    {
        if (rings.Count < maxRings)
        {
            Vector3 origin = transform.position;
            float startTime = Time.time;
            rings.Add(new Vector4(origin.x, origin.y, origin.z, startTime));
        }
    }
}