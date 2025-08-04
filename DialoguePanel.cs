using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    public void Close()
    {
        FindAnyObjectByType<NPC>().EndDialogue();
    }
}
