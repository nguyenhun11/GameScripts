using System.IO;
using UnityEngine;

public class Controller_Save : MonoBehaviour
{
    private string saveLocation;
    private Controller_Inventory inventory;
    private Controller_Hotbar hotbar;

    void Start()
    {
        //define save location
        saveLocation = Path.Combine(Application.persistentDataPath, "SaveData.json");
        inventory = FindFirstObjectByType<Controller_Inventory>();
        hotbar = FindFirstObjectByType<Controller_Hotbar>();

        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            inventorySaveData = inventory.GetInventoryItems(),
            hotbarSaveData = hotbar.GetHotbarItems()
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
        }
        else
        {
            SaveGame();
        }
    }

}
