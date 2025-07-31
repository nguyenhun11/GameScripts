using UnityEngine;

public class People : MonoBehaviour
{
    public string ID_People = "PP-01";
    [SerializeField] private GameObject ChatBox;
    [SerializeField] private GameObject DialogBox;
    private PlayerControl playerControl;

    void Start()
    {
        ChatBox.SetActive(false);
        DialogBox.SetActive(false);
    }

    void Update()
    {
        if (playerControl != null)
        {
            if (playerControl.action)
            {
                Action();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ChatBox.SetActive(true);
        playerControl = collision.GetComponent<PlayerControl>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ChatBox.SetActive(false);
        playerControl = null;
    }

    private void Action()
    {
        DialogBox.SetActive(true);
    }
}
