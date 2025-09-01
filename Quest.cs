using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Quest;
[CreateAssetMenu(menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    public string ID_Quest;
    public string questName;
    public string description;
    public List<QuestObjective> objectives;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(ID_Quest))
        {
            ID_Quest = questName + Guid.NewGuid().ToString();
        }
    }

    public enum ObjectiveType
    {
        CollectItem,
        Defeat,
        ReachLocation,
        TalkToNPC
    }

    [System.Serializable]
    public class QuestObjective
    {
        public string ID_Objective; // ID item and IT quest
        public string discription;
        public ObjectiveType type;
        public int requireAmount;
        public int currentAmount;

        public bool IsComplete => currentAmount >= requireAmount;
    }
}

[System.Serializable]
public class QuestProgress
{
    public Quest quest;
    public List<QuestObjective> objectives;

    public QuestProgress(Quest quest)
    {
        this.quest = quest;
        objectives = new List<QuestObjective>();

        //deep copy avoid modifying original
        foreach (var obj in quest.objectives)
        {
            objectives.Add(new QuestObjective
            {
                ID_Objective = obj.ID_Objective,
                discription = obj.discription,
                type = obj.type,
                requireAmount = obj.requireAmount,
                currentAmount = obj.currentAmount
            });
        }
    }

    public bool IsComplete => objectives.TrueForAll(o => o.IsComplete);

    public string QuestID => quest.ID_Quest;

}