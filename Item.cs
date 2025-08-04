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
}
