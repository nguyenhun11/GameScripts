using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Controller_QuestUI : MonoBehaviour
{
    public Transform questListContent;
    public GameObject questEntryPrefab;
    public GameObject objectiveTextPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        //Destroy existing quest entry
        foreach(Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }

        //Build new entry
        foreach (var quest in Controller_Quest.Instance.activeQuests)
        {
            GameObject entry = Instantiate(questEntryPrefab, questListContent);
            TMP_Text questNameText = entry.transform.Find("QuestName").GetComponent<TMP_Text>();
            Transform objectiveList = entry.transform.Find("ObjectiveList");

            questNameText.text = quest.quest.questName;
            foreach (var objective in quest.objectives)
            {
                GameObject objectiveTextGO = Instantiate(objectiveTextPrefab, objectiveList);
                TMP_Text objText = objectiveTextGO.GetComponent<TMP_Text>();
                objText.text = $"{objective.discription} ({objective.currentAmount}/{objective.requireAmount})";
            }
        }
    }

}
