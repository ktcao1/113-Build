using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartContainer : MonoBehaviour
{
    public HeartContainer next;

    private float fill;
    public Image fillImage;

    public void SetHeart(float count)
    {
        fill = count;
        fillImage.fillAmount = fill;
        count--;
        if (next != null)
        {
            next.SetHeart(count);
        }
    }
}
