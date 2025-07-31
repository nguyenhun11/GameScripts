using UnityEngine;
using UnityEngine.UI;

public class MenuControl_Tabs : MonoBehaviour
{
    public Image[] tabsImage;
    public GameObject[] pages;

    void Start()
    {
        ActiveTab(0);
    }

    private void ResetTabMenus()
    {
        for(int i =0;  i<tabsImage.Length; i++)
        {
            pages[i].SetActive(false);
            tabsImage[i].color = Color.gray;
        }
    }

    public void ActiveTab(int tabNo)
    {
        ResetTabMenus();
        tabsImage[tabNo].color = Color.white;
        pages[tabNo].SetActive(true);
    }
}
