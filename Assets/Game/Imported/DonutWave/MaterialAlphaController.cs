using UnityEngine;

[ExecuteAlways]
public class SmoothMaterialAlpha : MonoBehaviour
{
    public Renderer targetRenderer;

    [Range(0f, 1f)]
    public float targetAlpha = 1f;

    public float smoothSpeed = 5f;

    private Material mat;
    private float currentAlpha;

    void OnEnable()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        if (targetRenderer != null)
        {
            mat = targetRenderer.sharedMaterial;

            if (mat != null)
                currentAlpha = mat.color.a;
        }
    }

    void Update()
    {
        if (mat == null) return;

        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * smoothSpeed);

        Color c = mat.color;
        c.a = currentAlpha;
        mat.color = c;
    }
}