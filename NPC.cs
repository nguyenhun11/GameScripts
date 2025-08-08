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

    private void Start()
    {
        chatIcon.SetActive(false);
        dialogueController = Controller_Dialogue.Instance;
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
        isDialogueActive = true;
        dialogueIndex = 0;
        dialogueController.SetNPCInfor(dialogueData.name_NPC, dialogueData.avt_NPC);
        dialogueController.ShowDialogueUI(true);
        Controller_Pause.SetPause(true);

        DisplayCurrentLine();
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
        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])// Tự động chuyển
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    private void NextLine()
    {
        //Clear choices
        dialogueController.ClearChoices();
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
        //If has next line, show next line
        else if (++dialogueIndex <  dialogueData.dialogueLines.Length)
        {
            DisplayCurrentLine();
        }

        //Check if has choices
        //CheckAndShowChoices();
        
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
        for(int i = 0; i <  choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            dialogueController.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex));
        }
    }

    private void ChooseOption(int nextIndex)
    {
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
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueController.SetDialogueText("");
        dialogueController.ShowDialogueUI(false);
        Controller_Pause.SetPause(false);
    }

    private void SetChatIcon(bool on)
    {
        chatIcon.SetActive(on);
    }
    
    public void SetHighLight(bool on)
    {
        SetChatIcon(on);
    }
}
