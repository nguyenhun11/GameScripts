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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                itemToPick.SetHighLight(false);
                bool canAddItem = inventory.AddItem(itemToPick.gameObject);
                if (canAddItem)
                {
                    itemToPick.PickUp();
                    Destroy(itemToPick.gameObject);
                }
                else
                {
                    Sprite icon = GetComponent<SpriteRenderer>().sprite;
                    UI_Notice.Instance.ShowNotice("Túi đã đầy", icon);
                    itemToPick = null;
                    CanPickItem = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            if (itemToPick == null)
            {
                Item item = collision.GetComponent<Item>();
                if (item != null)
                {
                    item.SetHighLight(true);
                    itemToPick = item;
                }
                CanPickItem = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                item.SetHighLight(false);
                itemToPick = null;
            }
            CanPickItem = false;
        }
    }
}
