using UnityEngine;

public class Controller_Pause : MonoBehaviour
{
    public static bool isGamePaused { get; private set; } = false;
    
    public static void SetPause(bool on)
    {
        isGamePaused = on; 
    }
}
