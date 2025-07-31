using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private Controller_Inventory inventory;
    private bool CanPickItem;
    private Item itemToPick;

    void Start()
    {
        inventory = FindFirstObjectByType<Controller_Inventory>();
    }

    void Update()
    {
        if (CanPickItem)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                bool canAddItem = inventory.AddItem(itemToPick.gameObject);
                if (canAddItem)
                {
                    itemToPick.PickUp();
                    Destroy(itemToPick.gameObject);
                }
                else
                {
                    UI_Notice.Instance.ShowNotice("Túi đã đầy", null);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                item.SetHighlight(true);
                itemToPick = item;
            }
            CanPickItem = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                item.SetHighlight(false);
                itemToPick = null;
            }
            CanPickItem = false;
        }
    }
}
