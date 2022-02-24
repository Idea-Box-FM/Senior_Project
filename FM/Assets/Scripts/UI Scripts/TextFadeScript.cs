using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// This script is placed on a Text element and will fadde in text and after a time fade it out
/// Also you have to put this on a text mesh Pro(TMP) element
/// </summary>

public class TextFadeScript : MonoBehaviour
{
    [Tooltip("This variable set how many seconds will wait until it starts to fade out")]
    public float fadeOutStart;

    public void FadeText()
    {
        StartCoroutine(FadeTextToFullAlpha(1f, GetComponent<TMP_Text>()));

        StartCoroutine(FadeTextToZeroAlpha(1f, GetComponent<TMP_Text>(), fadeOutStart));

        //Debug.Log("The text has Faded");

    }



    public IEnumerator FadeTextToFullAlpha(float t, TMP_Text i)
    {

        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TMP_Text i, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
