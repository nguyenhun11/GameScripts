using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID_Item;
    public string Name_Item;

    private SpriteRenderer sprite;
    private Color normalColor;
    private Color highlightColor = Color.gray;
    private bool isHighlighted = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        normalColor = Color.white;
        SetHighlight(false);
    }

    public void SetHighlight(bool on)
    {
        if (sprite == null) return;

        if (on && !isHighlighted)
        {
            sprite.color = highlightColor;
            isHighlighted = true;
        }
        else if (!on && isHighlighted)
        {
            sprite.color = normalColor;
            isHighlighted = false;
        }
    }

    public virtual void PickUp()
    {
        SetHighlight(false);
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
}
