using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Controller_Inventory : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefaps;
    private ItemDictionary itemDictionary;

    void Start()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>();
    }

    public List<SlotItemSaveData> GetInventoryItems()
    {
        List<SlotItemSaveData> inventorySaveDatas = new List<SlotItemSaveData>();
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                inventorySaveDatas.Add(new SlotItemSaveData { 
                    ID_Item = item.ID_Item, 
                    Name_Item = item.Name_Item,
                    slot_Index = slotTransform.GetSiblingIndex()
                });
            }
        }
        return inventorySaveDatas;
    }

    public void SetInventoryItem(List<SlotItemSaveData> inventorySaveDatas)
    {
        //Clear
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Destroy(slotTransform.gameObject);
        }
        //Create new slot
        for(int i = 0; i< slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        foreach(SlotItemSaveData data in inventorySaveDatas)
        {
            if (data.slot_Index < slotCount)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slot_Index).GetComponent<Slot>();
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


    public bool AddItem(GameObject item)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(item, slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition= Vector2.zero;
                slot.currentItem = newItem;
                return true;
            }
        }
        Debug.Log("Inventory is full!!");
        return false;
    }
    
}
