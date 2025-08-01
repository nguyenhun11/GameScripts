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
            interactionIcon.SetActive(true);
            SoundEffectManager.Play("Interact");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) 
            && interactable == interactableInRange)
        {
            interactableInRange = null;
            interactionIcon.SetActive(false);
        }
    }
}
