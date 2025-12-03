using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractSystem : MonoBehaviour
{
    public Transform cam;

    
    RaycastHit objectHit;
    public MeshRenderer interactSphere;
    [SerializeField] private LayerMask interactableMask;

    private void Update()
    {
        Physics.Raycast(cam.position, cam.forward, out objectHit, 2.2f, interactableMask, QueryTriggerInteraction.Collide);
        if(objectHit.collider!= null)
        {
            interactSphere.sharedMaterial.color = Color.green;
        }
        else
        {
            interactSphere.sharedMaterial.color = Color.white;
        }

    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (objectHit.collider != null)
            {
                Interactable interactableObject;

                if (objectHit.collider.TryGetComponent<Interactable>(out interactableObject))
                {
                    interactableObject.onInteract(transform);
                }

            }
            else
            {
                print("no collider");
            }
        }
    }
}
