using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public string ID_Chest {  get; private set; }
    public bool isOpened {  get; private set; }
    public GameObject itemPrefab;
    public Sprite openSprite;


    void Start()
    {
        ID_Chest ??= GlobalHelper.GenerateUniqueID(gameObject);
    }

    public bool CanInteract()
    {
        return !isOpened;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        OpenChest();
    }

    private void OpenChest()
    {
        SetOpen(true);
        SoundEffectManager.Play("Chest");
        if (itemPrefab!= null)
        {
            GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
            droppedItem.GetComponent<Effect_ItemBounce>().StartBounce();
        }
    }

    public void SetOpen(bool on)
    {
        if (isOpened = on)
        {
            GetComponent<SpriteRenderer>().sprite = openSprite;
        }
    }

}
