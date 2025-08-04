using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPC_Dialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image avt;
    public GameObject chatIcon;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    private void Start()
    {
        chatIcon.SetActive(false);
        dialoguePanel.SetActive(false);
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
        nameText.SetText(dialogueData.name_NPC);
        avt.sprite = dialogueData.avt_NPC;
        dialoguePanel.SetActive(true);
        Controller_Pause.SetPause(true);

        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");
        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            SoundEffectManager.PlayVoice(dialogueData.voiceSound, dialogueData.voicePitch);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;
        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])// Tự động chuyển
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    private void NextLine()
    {
        if (isTyping)
        {
            //Skip typing animation, show full text
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping= false;
        }
        else if (++dialogueIndex <  dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine()); // Start next line
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
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
