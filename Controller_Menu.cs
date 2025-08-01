using UnityEngine;

public class Controller_Menu : MonoBehaviour
{
    #region Singleton
    public static Controller_Menu Instance { get; private set; }
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

    public GameObject menuCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (!menuCanvas.activeSelf && Controller_Pause.isGamePaused)
            {
                return;
            }
            SetActiveMenu(!menuCanvas.activeSelf);
        }
    }


    public void SetActiveMenu(bool on)
    {
        menuCanvas.SetActive(on);
        Controller_Pause.SetPause(menuCanvas.activeSelf);
    }
}
