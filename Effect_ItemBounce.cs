using System.Collections;
using UnityEngine;

public class Effect_ItemBounce : MonoBehaviour
{
    public float bounceHeight = 0.3f;
    public float bounceDuration = 0.5f;
    public int bounceCount = 3;

    public void StartBounce()
    {
        StartCoroutine(BounceHandler());
    }

    private IEnumerator BounceHandler()
    {
        Vector3 startPosition = transform.position;
        float localBounceHeight = bounceHeight;
        float localBoucneDuration = bounceDuration;
        for(int i = 0; i < bounceCount; i++)
        {
            yield return Bounce(startPosition, localBounceHeight, localBoucneDuration / 2);
            localBounceHeight *= 0.5f;
            localBoucneDuration *= .8f;

        }
        transform.position = startPosition;
    }

    private IEnumerator Bounce(Vector3 start, float height, float duration)
    {
        Vector3 peak = start + Vector3.up * height;
        float elapse = 0f;

        //Move up
        while (elapse < duration)
        {
            transform.position = Vector3.Lerp(start, peak, elapse / duration);
            elapse += Time.deltaTime;
            yield return null;
        }

        elapse = 0f; //reset time
        //Move down
        while (elapse < duration)
        {
            transform.position = Vector3.Lerp(peak, start, elapse / duration);
            elapse += Time.deltaTime;
            yield return null;
        }

    }
}
