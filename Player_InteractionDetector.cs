using UnityEngine;
using UnityEngine.InputSystem;

public class Player_InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null;
    [SerializeField] private GameObject interactionIcon;

    void Start()
    {
        interactionIcon.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactableInRange?.Interact();
            if (!interactableInRange.CanInteract())
            {
                interactionIcon.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) 
            && interactable.CanInteract())
        {
            interactableInRange = interactable;
            interactableInRange.SetHighLight(true);
            interactionIcon.SetActive(true);
            SoundEffectManager.Play("Interact");
            if (collision.TryGetComponent(out NPC_WaypointMover mover)){
                mover.StopMove();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) 
            && interactable == interactableInRange)
        {
            interactableInRange.SetHighLight(false); 
            interactableInRange = null;
            interactionIcon.SetActive(false);
            if (collision.TryGetComponent(out NPC_WaypointMover mover))
            {
                mover.StopMove(false);
            }
        }
    }
}
