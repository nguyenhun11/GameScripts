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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ActiveNextTab();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ActivePreviousTab();
        }
    }

    private void ResetTabMenus()
    {
        for (int i = 0; i < tabsImage.Length; i++)
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

    private void ActiveNextTab()
    {
        for(int i = 0;i < tabsImage.Length;i++)
        {
            if (pages[i].activeSelf)
            {
                if (i <  tabsImage.Length - 1)
                {
                    ActiveTab(i + 1);
                }
                else ActiveTab(0);
                break;
            }
        }
    }

    private void ActivePreviousTab()
    {
        for (int i = 0; i < tabsImage.Length; i++)
        {
            if (pages[i].activeSelf)
            {
                if (i > 0)
                {
                    ActiveTab(i - 1);
                }
                else ActiveTab(tabsImage.Length - 1);
                break;
            }
        }
    }
}
