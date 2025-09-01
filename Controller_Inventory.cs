using NUnit.Framework;
using System;
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
    Dictionary<int, int> itemsCountCache = new();
    public event Action OnInventoryChanged; //event to notify quest system


    #region singleton
    public static Controller_Inventory Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    void Start()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>();
        RebuildItemCounts();
    }

    public void RebuildItemCounts()
    {
        itemsCountCache.Clear();
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                if (item != null)
                {
                    itemsCountCache[item.ID_Item] = itemsCountCache.GetValueOrDefault(item.ID_Item, 0) + item.count;//Explain
                }
            }
        }
        OnInventoryChanged?.Invoke();
    }

    public Dictionary<int, int> GetItemsCount() => itemsCountCache;


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
                    slot_Index = slotTransform.GetSiblingIndex(),
                    count = item.count
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
        for (int i = 0; i < slotCount; i++)
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
                    
                    Item itemComponent = item.GetComponent<Item>();
                    if (itemComponent != null && data.count > 1)
                    {
                        itemComponent.count = data.count;
                        itemComponent.UpdateCountDisplay();
                    }

                    slot.currentItem = item;
                }
            }
        }
        RebuildItemCounts();
    }


    public bool AddItem(GameObject item)
    {
        Item itemToAdd = item.GetComponent<Item>();
        if (itemToAdd == null)
        {
            return false;
        }
        //Check if already has this item
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                Item slotItem = slot.currentItem.GetComponent<Item>();
                if (slotItem != null && slotItem.ID_Item == itemToAdd.ID_Item)
                {
                    //Add count this item
                    slotItem.AddToStack();
                    RebuildItemCounts();
                    return true;
                }
            }
        }

        //Look for empty slot
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(item, slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition= Vector2.zero;
                slot.currentItem = newItem;
                RebuildItemCounts();
                return true;
            }
        }
        Debug.Log("Inventory is full!!");
        return false;
    }

    public void RemoveItemFromInventory(int itemID, int amount)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            if (amount <= 0) break;
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot?.currentItem?.GetComponent<Item>() is Item item && item.ID_Item == itemID)
            {
                int removed = Mathf.Min(amount, item.count);
                item.RemoveFromStack(removed);

                amount -= removed; // amount left
                if (item.count == 0)
                {
                    Destroy(slot.currentItem);
                    slot.currentItem = null;
                }
            }
        }
        RebuildItemCounts();
    }
    
}
