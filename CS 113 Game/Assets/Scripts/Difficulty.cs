using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    [SerializeField] GameObject loadingPanel;

    public void EasyMode()
    {
        GameManager.instance.difficulty = "easy";
        loadingPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void NormalMode()
    {
        GameManager.instance.difficulty = "normal";
        loadingPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void HardMode()
    {
        GameManager.instance.difficulty = "hard";
        loadingPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
