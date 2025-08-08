using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID_Item;
    public string Name_Item;
    public int count = 1;
    private TMP_Text text_Count;

    private SpriteRenderer sprite;
    private Color normalColor;
    private Color highlightColor = Color.gray;
    private bool isHighlighted = false;



    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        normalColor = Color.white;
        text_Count = GetComponentInChildren<TMP_Text>();
        UpdateCountDisplay();
        SetHighLight(false);
    }

    public void SetHighLight(bool on)
    {
        if (sprite == null) return;

        if (on && !isHighlighted)
        {
            sprite.color = highlightColor;
            isHighlighted = true;
            SoundEffectManager.Play("Interact");
        }
        else if (!on && isHighlighted)
        {
            sprite.color = normalColor;
            isHighlighted = false;
        }
    }

    public virtual void PickUp()
    {
        SoundEffectManager.Play("PickUp");
        SetHighLight(false);
        Sprite itemIcon = GetComponent<SpriteRenderer>().sprite;
        if (UI_Notice.Instance != null)
        {
            UI_Notice.Instance.ShowNotice("Nhặt " + Name_Item, itemIcon);
        }
    }

    public virtual void UseItem()
    {
        Debug.Log("Use item " + Name_Item);
    }

    public void UpdateCountDisplay()
    {
        if (text_Count != null)
        {
            text_Count.text = count > 1 ? count.ToString() : "";
        }
    }

    public void AddToStack(int amount = 1)
    {
        count += amount;
        UpdateCountDisplay();
    }

    public int RemoveFromStack(int amount = 1)
    {
        int remove = Mathf.Min(count, amount);
        count -= remove;
        UpdateCountDisplay();
        return remove;
    }
    public GameObject CloneNewItem(int newCount = 1)
    {
        GameObject clone = Instantiate(gameObject);
        Item cloneItem = clone.GetComponent<Item>();
        cloneItem.count = newCount;
        cloneItem.UpdateCountDisplay();
        return clone;
    }
}
