using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    [SerializeField] GameObject musicDevice;
    private float waitTime = 4f;

    void Update()
    {
        if (waitTime <= 0)
        {
            GameManager.instance.isLoading = false;
            musicDevice.SetActive(true);
            this.gameObject.SetActive(false);
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
