using UnityEngine;

public class TextureAnimator : MonoBehaviour
{
    public Texture[] frames;       // Drag textures here in Inspector
    public float frameRate = 10f;  // Frames per second

    private Renderer rend;
    private int currentFrame;
    private float timer;

    void Start()
    {
        rend = GetComponent<Renderer>();

        if (frames.Length > 0)
        {
            rend.material.mainTexture = frames[0];
        }
    }

    void Update()
    {
        if (frames.Length == 0) return;

        timer += Time.deltaTime;

        if (timer >= 1f / frameRate)
        {
            timer = 0f;

            currentFrame++;
            if (currentFrame >= frames.Length)
                currentFrame = 0;

            rend.material.mainTexture = frames[currentFrame];
        }
    }
}