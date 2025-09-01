using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Quest;

public class Controller_Quest : MonoBehaviour
{
    #region singleton
    public static Controller_Quest Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
        questUI = FindFirstObjectByType<Controller_QuestUI>();
        Controller_Inventory.Instance.OnInventoryChanged += CheckInventoryForQuest; //explain
    }
    #endregion

    public List<QuestProgress> activeQuests = new();
    private Controller_QuestUI questUI;

    public List<string> handinQuestIDs = new();


    public void AcceptQuest(Quest quest)
    {
        if (IsQuestActive(quest.ID_Quest))
        {
            return;
        }

        activeQuests.Add(new QuestProgress(quest));
        CheckInventoryForQuest();
        questUI.UpdateQuestUI();
    }

    public bool IsQuestActive(string questID) => activeQuests.Exists(q => q.QuestID == questID);

    public void CheckInventoryForQuest()
    {
        Dictionary<int, int> itemCount = Controller_Inventory.Instance.GetItemsCount();

        foreach (QuestProgress quest in activeQuests)
        {
            foreach(QuestObjective questObjective in quest.objectives)
            {
                if (questObjective.type != ObjectiveType.CollectItem)
                {
                    continue;
                }
                if (!int.TryParse(questObjective.ID_Objective, out int itemID))//explain
                {
                    continue;
                }

                int newAmount = itemCount.TryGetValue(itemID, out int count) ? Mathf.Min(count, questObjective.requireAmount) : 0;

                if (questObjective.currentAmount != newAmount)
                {
                    questObjective.currentAmount = newAmount;
                }
            }
        }
        questUI.UpdateQuestUI();
    }

    public bool IsQuestComplete(string questID)
    {
        QuestProgress quest = activeQuests.Find(q => q.QuestID == questID);
        return quest != null && quest.objectives.TrueForAll(o => o.IsComplete);
    }

    public bool IsQuestHandedIn(string QuestID)
    {
        return handinQuestIDs.Contains(QuestID);
    }

    public void HandInQuest(string questID)
    {
        //Try remove require item
        if (!RemoveItemFromInventory(questID))
        {
            return;//Quest can not complete cause not enough item
        }

        //Remove quest
        QuestProgress quest = activeQuests.Find(q => q.QuestID ==  questID);
        if (quest != null)
        {
            handinQuestIDs.Add(questID);
            activeQuests.Remove(quest);
            questUI.UpdateQuestUI();
        }
    }

    public bool RemoveItemFromInventory(string questID)
    {
        //QuestProgress quest = activeQuests.Find(q => q.QuestID == questID);
        QuestProgress quest = activeQuests.Find(delegate (QuestProgress q) {
            return q.QuestID == questID;
        });
        if (quest == null) return false;

        Dictionary<int, int> requiredItems = new();//ID_Item, amount

        //Item require from objective
        foreach (QuestObjective objective in quest.objectives)
        {
            if (objective.type == ObjectiveType.CollectItem
                && int.TryParse(objective.ID_Objective, out int itemID))
            {
                requiredItems[itemID] = objective.requireAmount;
            }
        }

        //Item we have
        Dictionary<int, int> itemCounts = Controller_Inventory.Instance.GetItemsCount();
        foreach (var item in requiredItems)
        {
            if (itemCounts.GetValueOrDefault(item.Key) < item.Value) // Dont have enough item
            {
                return false;
            }
        }

        //Remove
        foreach(var item in requiredItems)
        {
            //Remove item by Inventory Controller
            Controller_Inventory.Instance.RemoveItemFromInventory(item.Key, item.Value);
        }
        return true;
    }

    public void LoadQuestProgress(List<QuestProgress> progress)
    {
        activeQuests = progress ?? new();
        CheckInventoryForQuest();
    }
}
