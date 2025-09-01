using UnityEngine;
using UnityEngine.UI;

public class ChoicesContainer : MonoBehaviour
{
    public void SelectChoice(int index)
    {
        ResetColor();

        if (index >= 0 && index < transform.childCount)
        {
            Button selectedChoice = transform.GetChild(index).GetComponent<Button>();
            if (selectedChoice != null )
            {
                Color highlightColor = selectedChoice.colors.highlightedColor;
                selectedChoice.GetComponent<Image>().color = highlightColor;
            }
        }
    }


    private void ResetColor()
    {
        foreach (Transform child in transform)
        {
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                button.GetComponent<Image>().color = Color.white;
            }
        }
    }

}
