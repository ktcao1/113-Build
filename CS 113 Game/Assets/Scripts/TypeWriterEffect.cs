using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeWriterEffect : MonoBehaviour
{
    public float delay = 0.1f;
    public string fullText;
    public char currentText;
    public TMP_Text txt;
    public string originalText;
    public bool loop, typeDone;
    public bool fadingOut = true;
    public bool typeSoundBool, playOnce = false;
    public AudioSource typeSound;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<TMP_Text>();
        originalText = txt.text;
        StartCoroutine(ShowText());
    }

    void FixedUpdate()
    {
        if (typeDone && !playOnce)
        {
            StartCoroutine(FadeTextToZeroAlpha(10f, txt));
            // StartCoroutine(FadeTextToFullAlpha(1f, txt));
        }
    }

    private IEnumerator ShowText()
    {
        while (true) 
        {
            for (int i = 0; i < fullText.Length; i++)
            {
                currentText = fullText[i];
                if (typeSoundBool && currentText != ' ') typeSound.Play();
                txt.text += currentText;
                yield return new WaitForSeconds(delay);
            }

            if (!loop)
            {
                typeDone = true;
                break;
            }
            yield return new WaitForSeconds(0.5f);
            txt.text = originalText;
        }
    }
 
    public IEnumerator FadeTextToZeroAlpha(float t, TMP_Text i)
    {
        while (i.color.a > 0.0f && fadingOut)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            if (i.color.a <= 0f) fadingOut = false;
            yield return null;
        }
        while (i.color.a <= 1.0f && !fadingOut)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            if (i.color.a >= 1f) fadingOut = true;
            yield return null;
        }
    }
}
