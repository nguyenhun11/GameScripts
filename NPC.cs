using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPC_Dialogue dialogueData;
    private Controller_Dialogue dialogueController;

    public GameObject chatIcon;
    public int dialogueIndex;
    public bool isTyping, isDialogueActive;

    private bool isShowChoices;
    private bool isSelectChoicesByKey;
    private bool[] choiceSelectedIndex;

    private enum QuestState
    {
        NotStarted,
        InProgress,
        Completed
    };
    private QuestState questState = QuestState.NotStarted;

    private void Start()
    {
        chatIcon.SetActive(false);
        dialogueController = Controller_Dialogue.Instance;
    }

    private void Update()
    {
        if (isShowChoices)
        {
            if (!isSelectChoicesByKey && Input.GetKeyDown(KeyCode.Space))
            {
                SelectNextOption();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow)
                || Input.GetKeyDown(KeyCode.RightArrow))
            {
                SelectNextOption();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)
                || Input.GetKeyDown(KeyCode.UpArrow))
            {
                SelectPreviousOption();
            }
        }
    }


    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData ==  null || (Controller_Pause.isGamePaused && !isDialogueActive))
        {
            return;
        }
        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        //Sync with quest data
        SyncQuestState();

        //Set dialogue line base on quest state
        if (questState == QuestState.NotStarted)
        {
            dialogueIndex = 0; //Start at first
        }
        else if (questState == QuestState.InProgress)
        {
            dialogueIndex = dialogueData.questInProgressIndex;
        }
        else if (questState == QuestState.Completed)
        {
            dialogueIndex = dialogueData.questCompletedIndex;
        }


        isDialogueActive = true;
        dialogueController.SetNPCInfor(dialogueData.name_NPC, dialogueData.avt_NPC);
        dialogueController.ShowDialogueUI(true);
        Controller_Pause.SetPause(true);

        DisplayCurrentLine();
    }

    private void SyncQuestState()
    {
        if (dialogueData.quest == null) return;

        string questID = dialogueData.quest.ID_Quest;

        if (Controller_Quest.Instance.IsQuestComplete(questID)
            || Controller_Quest.Instance.IsQuestHandedIn(questID))
        {
            questState = QuestState.Completed;
        }

        else if (Controller_Quest.Instance.IsQuestActive(questID))
        {
            questState = QuestState.InProgress;
        }
        else
        {
            questState = QuestState.NotStarted;
        }
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueController.SetDialogueText("");
        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueController.SetDialogueText(dialogueController.dialogueText.text += letter);
            SoundEffectManager.PlayVoice(dialogueData.voiceSound, dialogueData.voicePitch);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }
        CheckAndShowChoices();
        isTyping = false;
        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex] && !isShowChoices )// Tự động chuyển
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    private void NextLine()
    {
        //Clear choices
        ClearChoices();
        if (isTyping)
        {
            //Skip typing animation, show full text
            StopAllCoroutines();
            dialogueController.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            CheckAndShowChoices();
            isTyping = false;
        }

        //Check endDialogue
        else if (dialogueIndex < dialogueData.dialogueLines.Length
            && dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }

        //If show choice, if dont select choice, dont show next line
        else if (isShowChoices)
        {
            if (!isSelectChoicesByKey)
            {
                SelectNextOption();
            }
            else
            {
                ChooseOptionSelected();
            }
                return;
        }
        //If has next line, show next line
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            DisplayCurrentLine();
        }        
    }

    private void ClearChoices()
    {
        dialogueController.ClearChoices();
        isShowChoices = false;
        choiceSelectedIndex = new bool[0];
    }

    private void CheckAndShowChoices()
    {
        foreach (DialogueChoice choice in dialogueData.choices)
        {
            if (choice.dialogueIndex == dialogueIndex)
            {
                DisplayChoices(choice);
                return;
            }
        }
    }

    private void DisplayChoices(DialogueChoice choice)
    {
        //Create choices button
        for(int i = 0; i <  choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            bool giveQuest = choice.giveQuest[i];
            dialogueController.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex, giveQuest));
        }

        isShowChoices = true;
        isSelectChoicesByKey = false;
        choiceSelectedIndex = new bool[choice.choices.Length];
    }

    private void ChooseOption(int nextIndex, bool giveQuest)
    {
        if (giveQuest)
        {
            Controller_Quest.Instance.AcceptQuest(dialogueData.quest);
            questState = QuestState.InProgress;
        }
        dialogueIndex = nextIndex;
        dialogueController.ClearChoices();
        DisplayCurrentLine();
    }

    private void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    public void EndDialogue()
    {
        if (questState == QuestState.Completed
            && !Controller_Quest.Instance.IsQuestHandedIn(dialogueData.quest.ID_Quest))
        {
            //Handle quest completion
            HandleQuestCompletion(dialogueData.quest);
        }

        StopAllCoroutines();
        isDialogueActive = false;
        dialogueController.SetDialogueText("");
        dialogueController.ShowDialogueUI(false);
        Controller_Pause.SetPause(false);
    }

    private void HandleQuestCompletion(Quest quest)
    {
        Controller_Quest.Instance.HandInQuest(quest.ID_Quest);
    }

    private void SetChatIcon(bool on)
    {
        chatIcon.SetActive(on);
    }
    
    public void SetHighLight(bool on)
    {
        SetChatIcon(on);
    }

    private void SelectOption(int index)
    {
        if (!isShowChoices) return;
        isSelectChoicesByKey = true;
        for(int i = 0; i < choiceSelectedIndex.Length; i++)
        {
            choiceSelectedIndex[i] = false;
        }
        ChoicesContainer choicesContainer =  dialogueController.choicesContainter.GetComponent<ChoicesContainer>();
        choicesContainer.SelectChoice(index);
        choiceSelectedIndex[index] = true;
    }

    private void SelectNextOption()
    {
        if (!isShowChoices) return;
        for(int i = 0; i < choiceSelectedIndex.Length; i++)
        {
            if (choiceSelectedIndex[i])
            {
                if (i <  choiceSelectedIndex.Length - 1)
                {
                    SelectOption(i + 1);
                }
                else
                {
                    SelectOption(0);
                }
                return;
            }
        }
        SelectOption(0);
    }

    private void SelectPreviousOption()
    {
        if (!isShowChoices) return;
        for(int i = 0; i < choiceSelectedIndex.Length; i++)
        {
            if (choiceSelectedIndex[i])
            {
                if (i > 0)
                {
                    SelectOption(i - 1);
                }
                else
                {
                    SelectOption(choiceSelectedIndex.Length - 1);
                }
                return;
            }
        }
        SelectOption(choiceSelectedIndex.Length - 1);
    }

    private void ChooseOptionSelected()
    {

    }
}


