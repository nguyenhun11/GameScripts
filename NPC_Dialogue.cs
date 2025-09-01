using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPC_Dialogue : ScriptableObject
{
    public string name_NPC;
    public Sprite avt_NPC;
    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public bool[] endDialogueLines;
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;

    public DialogueChoice[] choices;

    public int questInProgressIndex; // what to say if quest in progress
    public int questCompletedIndex; //what to say if quest completed
    public Quest quest; //Quest NPC give

}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex;//Dialogue line where choose appear
    public string[] choices;//Response option;
    public int[] nextDialogueIndexes;//Where choice lead
    public bool[] giveQuest;//If the choice give quest
}
