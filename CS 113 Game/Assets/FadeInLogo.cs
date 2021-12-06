using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeInLogo : MonoBehaviour
{
    public TypeWriterEffect typeWriter;
    public GameObject startPanel;
    public GameObject titleArt;
    public bool switchScreens;

    void Start()
    {
        gameObject.SetActive(true);
    }

    void FixedUpdate()
    {
        if (!typeWriter.typeDone) return;
        if (switchScreens)
        {
            gameObject.SetActive(false);
            startPanel.SetActive(true);
            titleArt.SetActive(true);
        }
        if (!switchScreens)
        {
            StartCoroutine(FadeToFullAlphaImg(50f, GetComponent<Image>(), "first"));
            StartCoroutine(FadeToFullAlphaText(50f, GetComponentInChildren<TMP_Text>()));
        }
    }

    IEnumerator FadeToFullAlphaImg(float t, Image i, string first)
    {
        while (i.color.a >= 0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            if (i.color.a <= 0f && first == "first") { yield return new WaitForSeconds(0.5f); switchScreens = true;}
            yield return null;
        }
    }

    IEnumerator FadeToFullAlphaText(float t, TMP_Text i)
    {
        while (i.color.a >= 0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
