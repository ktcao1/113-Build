using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeldaBarrierBar : MonoBehaviour
{
    public static ZeldaBarrierBar instance;

    [SerializeField] private GameObject barrierContainerPrefab;
    [SerializeField] private List<GameObject> barrierContainers;
    public int totalBarriers;
    public float currentBarriers;
    HeartContainer currentContainer;

    private void Awake()
    {
        if (ZeldaBarrierBar.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void SetupBarriers(int barriersIn)
    {
        barrierContainers.Clear();
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        totalBarriers = barriersIn;
        currentBarriers = (float) totalBarriers;
        
        for (int i = 0; i < totalBarriers; i++)
        {
            GameObject newBarrier = Instantiate(barrierContainerPrefab, transform);
            barrierContainers.Add(newBarrier);
            if (currentContainer != null)
            {
                currentContainer.next = newBarrier.GetComponent<HeartContainer>();
            }
            currentContainer = newBarrier.GetComponent<HeartContainer>();
        }
        currentContainer = barrierContainers[0].GetComponent<HeartContainer>();
    }

    public void SetCurrentHealth(float health)
    {
        currentBarriers = health;
        currentContainer.SetHeart(currentBarriers);
    }

    public void AddBarriers(float healthUp)
    {
        currentBarriers += healthUp;
        if (currentBarriers > totalBarriers)
        {
            currentBarriers = (float) totalBarriers;
        }
        currentContainer.SetHeart(currentBarriers);
    }

    public void RemoveBarriers(float healthDown)
    {
        currentBarriers -= healthDown;
        if (currentBarriers < 0)
        {
            currentBarriers = 0f;
        }
        currentContainer.SetHeart(currentBarriers);
    }

    public void AddContainer()
    {
        GameObject newBarrier = Instantiate(barrierContainerPrefab, transform);
        currentContainer = barrierContainers[barrierContainers.Count - 1].GetComponent<HeartContainer>();
        barrierContainers.Add(newBarrier);

        if (currentContainer != null)
        {
            currentContainer.next = newBarrier.GetComponent<HeartContainer>();
        }

        currentContainer = barrierContainers[0].GetComponent<HeartContainer>();

        totalBarriers++;
        currentBarriers++;
        SetCurrentHealth(currentBarriers);
    }
}
