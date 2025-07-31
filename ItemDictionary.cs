using NUnit.Framework;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    public Dictionary<int, GameObject> itemDictionary_ID;

    private void Awake()
    {
        itemDictionary_ID = new Dictionary<int, GameObject>();
        for(int i = 0; i< itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].ID_Item = i + 1;
            }
        }
        foreach(Item item in itemPrefabs)
        {
            itemDictionary_ID[item.ID_Item] = item.gameObject;
        }
    }

    public GameObject getItemPrefabs(int id)
    {
        itemDictionary_ID.TryGetValue(id, out GameObject prefabs);
        if (prefabs == null)
        {
            Debug.Log($"Can not find item with ID {id}");
        }
        return prefabs;
    }
}
