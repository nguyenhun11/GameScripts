using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Notice : MonoBehaviour
{
    #region Singleton
    public static UI_Notice Instance {  get; private set; }
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

    public GameObject popUpPrefab;
    public int maxPopUp;
    public float popUpDuration = 5f;
    public readonly Queue<GameObject> activePopUps = new Queue<GameObject>();

    public void ShowNotice(string ItemName, Sprite icon)
    {
        GameObject newPopUp = Instantiate(popUpPrefab, transform);
        newPopUp.GetComponentInChildren<TMP_Text>().text = ItemName;
        Image itemImage = newPopUp.transform.Find("Icon")?.GetComponent<Image>();
        if(itemImage != null)
        {
            itemImage.sprite = icon;
        }
        activePopUps.Enqueue(newPopUp);
        if(activePopUps.Count > maxPopUp)
        {
            Destroy(activePopUps.Dequeue());
        }
        //Fade out and Destroy
        StartCoroutine(FadeOutAndDestroy(newPopUp));
    }


    private IEnumerator FadeOutAndDestroy(GameObject popUp)
    {
        yield return new WaitForSeconds(popUpDuration);
        if (popUp == null)
        {
            yield break;
        }
        CanvasGroup canvasGroup = popUp.GetComponent<CanvasGroup>();
        for(float timePassed = 0f; timePassed < 1f;  timePassed += Time.deltaTime)
        {
            if (popUp == null)
            {
                yield break;
            }
            canvasGroup.alpha = 1f - timePassed;
            yield return null;
        }
        Destroy(popUp);
    }
}
