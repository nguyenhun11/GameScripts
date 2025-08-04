using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPC_Dialogue : ScriptableObject
{
    public string name_NPC;
    public Sprite avt_NPC;
    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
}
