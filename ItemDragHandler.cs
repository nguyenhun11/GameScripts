using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    public CanvasGroup canvasGroup;

    [SerializeField] private float minDropDistance = 2f;
    [SerializeField] private float maxDropDistance = 3f;


    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        
        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();
        if (dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponent<Slot>();
            }
        }

        Slot originalSlot = originalParent.GetComponent<Slot>();
        if (dropSlot != null)
        {
            if (dropSlot.currentItem != null)
            {
                dropSlot.currentItem.transform.SetParent(originalParent.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            //Drop out of inventory
            if (!IsWithinInventory(eventData.position))
            {
                DropItem(originalSlot);
                Controller_Menu.Instance.SetActiveMenu(false);
            }
            // Return back if cant find new slot
            else
            {
                transform.SetParent(originalParent);
            }
        }
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform menuPanelRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(menuPanelRect, mousePosition); 
    }

    private void DropItem(Slot originalSlot)
    {
        originalSlot.currentItem = null;
        //Find player
        Transform playerTranform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTranform == null)
        {
            Debug.Log("Can't find player");
            return;
        }

        //Drop position
        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTranform.position + dropOffset;
        
        //Instantiate
        GameObject newItem = Instantiate(gameObject, dropPosition, Quaternion.identity);
        newItem.GetComponent<Effect_ItemBounce>()?.StartBounce();//Bounce effect

        //Destroy UI
        Destroy(gameObject);
    }

}