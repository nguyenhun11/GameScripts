using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller_Hotbar : MonoBehaviour
{
    public GameObject hotbarPanel;
    public GameObject slotPrefab;
    public int slotCount = 10; //From 1 - 0 on keyboard
    private ItemDictionary itemDictionary;
    private Key[] hotbarKeys;

    private void Awake()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>();
        hotbarKeys = new Key[slotCount];
        for(int i = 0; i < slotCount; i++)
        {
            hotbarKeys[i] = (i < 9) ? (Key)((int)Key.Digit1 + i) : Key.Digit0;
        }
    }

    void Update()
    {
        //Check key pressed
        for(int i = 0; i < slotCount; i++)
        {
            if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)
            {
                UseItemInSlot(i);
            }
        }
    }

    private void UseItemInSlot(int index)
    {
        Slot slot = hotbarPanel.transform.GetChild(index).GetComponent<Slot>();
        if (slot != null && slot.currentItem != null)
        {
            Item item = slot.currentItem.GetComponent<Item>();
            item.UseItem();
        }
    }

    public List<SlotItemSaveData> GetHotbarItems()
    {
        List<SlotItemSaveData> hotbarSaveDatas = new List<SlotItemSaveData>();
        foreach (Transform slotTransform in hotbarPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                hotbarSaveDatas.Add(new SlotItemSaveData
                {
                    ID_Item = item.ID_Item,
                    Name_Item = item.Name_Item,
                    slot_Index = slotTransform.GetSiblingIndex()
                });
            }
        }
        return hotbarSaveDatas;
    }

    public void SetHotbarItem(List<SlotItemSaveData> inventorySaveDatas)
    {
        //Clear
        foreach (Transform slotTransform in hotbarPanel.transform)
        {
            Destroy(slotTransform.gameObject);
        }
        //Create new slot
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, hotbarPanel.transform);
        }

        foreach (SlotItemSaveData data in inventorySaveDatas)
        {
            if (data.slot_Index < slotCount)
            {
                Slot slot = hotbarPanel.transform.GetChild(data.slot_Index).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.getItemPrefabs(data.ID_Item);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
        }
    }
}
