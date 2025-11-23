using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractSystem : MonoBehaviour
{
    public Transform cam;

    

    RaycastHit objectHit;
    [SerializeField] private LayerMask interactableMask;
    
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Physics.Raycast(cam.position, cam.forward, out objectHit, 2.2f, interactableMask, QueryTriggerInteraction.Collide))
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
