using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Transform originalParent;
    public CanvasGroup canvasGroup;
    private Controller_Inventory inventory;

    [SerializeField] private float minDropDistance = 2f;
    [SerializeField] private float maxDropDistance = 3f;


    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventory = Controller_Inventory.Instance;
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
                Item draggedItem = GetComponent<Item>();
                Item targetItem = dropSlot.currentItem.GetComponent<Item>();

                //if the same item, add to stack
                if (draggedItem.ID_Item == targetItem.ID_Item)
                {
                    targetItem.AddToStack(draggedItem.count);
                    originalSlot.currentItem = null;
                    Destroy(gameObject);
                }
                //if dropSlot has difference item, swap two items
                else
                {
                    dropSlot.currentItem.transform.SetParent(originalParent.transform);
                    originalSlot.currentItem = dropSlot.currentItem;
                    dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    transform.SetParent(dropSlot.transform);
                    dropSlot.currentItem = gameObject;
                    GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                }
            }
            else
            {
                //Put item to this slot
                originalSlot.currentItem = null;
                transform.SetParent(dropSlot.transform);
                dropSlot.currentItem = gameObject;
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
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
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        }
    }

    private bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform menuPanelRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(menuPanelRect, mousePosition); 
    }

    private void DropItem(Slot originalSlot)
    {
        Item item = GetComponent<Item>();
        int count = item.count;
        if (count > 1)
        {
            item.RemoveFromStack();
            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            count = 1;
        }
        else
        {
            originalSlot.currentItem = null;
        }

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
        Item droppedItem = newItem.GetComponent<Item>();
        droppedItem.count = 1;
        newItem.GetComponent<Effect_ItemBounce>()?.StartBounce();//Bounce effect

        //Destroy UI
        if (count <= 1 && originalSlot.currentItem == null)
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            SplitStack();
        }
    }

    private void SplitStack()
    {
        Item item = GetComponent<Item>();
        if (item == null || item.count <= 1)
        {
            return;
        }
        int splitAmount = item.count / 2;
        if (splitAmount <= 0) return;

        item.RemoveFromStack(splitAmount);

        GameObject newItem = item.CloneNewItem(splitAmount);

        if (inventory == null || newItem == null)
        {
            return;
        }

        foreach (Transform slotTransform in inventory.inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                slot.currentItem = newItem;
                newItem.transform.SetParent(slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                return;
            }
        }
        //No empty slot, return to stack
        item.AddToStack(splitAmount);
        Destroy(newItem);
    }
}