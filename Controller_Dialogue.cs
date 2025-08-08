using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Dialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image avt;

    public Transform choicesContainter;
    public GameObject choiceButtonPrefab;

    #region singleton
    public static Controller_Dialogue Instance;
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
    }
    #endregion

    public void ShowDialogueUI(bool on)
    {
        dialoguePanel.SetActive(on);
    }

    public void SetNPCInfor(string name_NPC, Sprite avt_NPC)
    {
        nameText.text = name_NPC;
        avt.sprite = avt_NPC;
    }

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void EndDialogue()
    {
        //FindAnyObjectByType<NPC>().EndDialogue();
    }

    public void ClearChoices()
    {
        foreach(Transform child in choicesContainter)
        {
            Destroy(child.gameObject);
        }
    }

    public void CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject choiceButton = Instantiate(choiceButtonPrefab, choicesContainter);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);
    }
}
