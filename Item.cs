using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID_Item;
    public string Name_Item;
    private SpriteRenderer sprite;
    private Color normalColor;
    [SerializeField] private Color highlightColor = Color.yellow;
    private bool isHighlighted = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        normalColor = GetComponent<SpriteRenderer>().color;
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
        Sprite itemIcon = GetComponent<SpriteRenderer>().sprite;
        if (UI_Notice.Instance != null)
        {
            UI_Notice.Instance.ShowNotice(Name_Item, itemIcon);
        }
    }

    public virtual void UseItem()
    {
        Debug.Log("Use item " + Name_Item);
    }
}
