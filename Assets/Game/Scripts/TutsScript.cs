using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.XR;

public class TutsScript : MonoBehaviour
{
    public UnityEvent onDisable;

    float timer = 0f;

    private void OnEnable()
    {
        timer = 0f;
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        onDisable?.Invoke();
    }

    void Update()
    {
        // Wait 3 seconds (unscaled time)
        if (timer < 3f)
        {
            timer += Time.unscaledDeltaTime;
            return;
        }

        // Check XR controller buttons only
        foreach (var device in InputSystem.devices)
        {
            if (device is XRController controller)
            {
                foreach (var control in controller.allControls)
                {
                    // Only detect actual buttons (ignores joystick automatically)
                    if (control is ButtonControl button)
                    {
                        if (button.wasPressedThisFrame)
                        {
                            Debug.Log("Pressed: " + button.name);

                            gameObject.SetActive(false);
                            return;
                        }
                    }
                }
            }
        }
    }
}