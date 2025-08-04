using UnityEngine;
using UnityEngine.UI;

public class CursorDebug : MonoBehaviour
{
    void Update()
    {
        gameObject.transform.position = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            gameObject.GetComponent<Image>().color = Color.red;
        }
        else
        {
            gameObject.GetComponent<Image>().color = Color.white;
        }
    }    
}
