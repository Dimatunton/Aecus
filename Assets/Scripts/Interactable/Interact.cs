using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractSystem : MonoBehaviour
{
    public Transform cam;

    
    RaycastHit objectHit;
    public MeshRenderer interactSphere;
    [SerializeField] private LayerMask interactableMask;

    Interactable interactableObject;

    private void Update()
    {
        Physics.Raycast(cam.position, cam.forward, out RaycastHit tempHit, 2.2f, interactableMask, QueryTriggerInteraction.Collide);

        if (tempHit.collider != null)
        {
            if (tempHit.collider.TryGetComponent<Interactable>(out Interactable tempInteractable))
            {
                if (interactableObject != null)
                {
                    interactableObject.showOutline = false;
                }
                interactableObject = tempInteractable;
                interactableObject.showOutline = true;
            }
            interactSphere.sharedMaterial.color = Color.green;
        }
        else
        {
            if(interactableObject != null) 
            { 
                interactableObject.showOutline = false;
                interactableObject = null;
            }
            interactSphere.sharedMaterial.color = Color.white;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (interactableObject != null)
            {
                interactableObject.onInteract(transform);
            }
            else
            {
                print("no collider");
            }
        }
    }
}
