using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Overlays;
using UnityEngine;

public class Controller_Save : MonoBehaviour
{
    private string saveLocation;
    private Controller_Inventory inventory;
    private Controller_Hotbar hotbar;
    private Chest[] chests;

    void Start()
    {
        InitializeComponent();
        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            inventorySaveData = inventory.GetInventoryItems(),
            hotbarSaveData = hotbar.GetHotbarItems(),
            chestSaveData = GetChestsState()
        };
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;
            inventory.SetInventoryItem(saveData.inventorySaveData);
            hotbar.SetHotbarItem(saveData.hotbarSaveData);
            LoadChestsState(saveData.chestSaveData);
        }
        else
        {
            SaveGame();
            inventory.SetInventoryItem(new List<SlotItemSaveData>());
            hotbar.SetHotbarItem(new List<SlotItemSaveData>());

        }
    }

    private void InitializeComponent()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "SaveData.json");
        inventory = FindFirstObjectByType<Controller_Inventory>();
        hotbar = FindFirstObjectByType<Controller_Hotbar>();
        chests = FindObjectsByType<Chest>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    private List<ChestSaveData> GetChestsState()
    {
        List<ChestSaveData> chestsState = new List<ChestSaveData>();
        foreach (Chest chest in chests)
        {
            ChestSaveData chestSaveData = new ChestSaveData
            {
                ID_Chest = chest.ID_Chest,
                isOpen = chest.isOpened
            };
            chestsState.Add(chestSaveData);
        }
        return chestsState;
    }

    private void LoadChestsState(List<ChestSaveData> data)
    {
        foreach(Chest chest in chests)
        {
            ChestSaveData chestSaveData = data.FirstOrDefault(c => c.ID_Chest == chest.ID_Chest);
            if (chestSaveData != null)
            {
                chest.SetOpen(chestSaveData.isOpen);
            }
        }
    }
}



