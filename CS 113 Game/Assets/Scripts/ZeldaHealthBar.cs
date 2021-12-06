using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeldaHealthBar : MonoBehaviour
{
    public static ZeldaHealthBar instance;

    [SerializeField] private GameObject heartContainerPrefab;
    [SerializeField] private List<GameObject> heartContainers;
    public int totalHearts;
    public float currentHearts;
    HeartContainer currentContainer;

    private void Awake()
    {
        if (ZeldaHealthBar.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void SetupHearts(int heartsIn)
    {
        heartContainers.Clear();
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        totalHearts = heartsIn;
        currentHearts = (float) totalHearts;
        
        for (int i = 0; i < totalHearts; i++)
        {
            GameObject newHeart = Instantiate(heartContainerPrefab, transform);
            heartContainers.Add(newHeart);
            if (currentContainer != null)
            {
                currentContainer.next = newHeart.GetComponent<HeartContainer>();
            }
            currentContainer = newHeart.GetComponent<HeartContainer>();
        }
        currentContainer = heartContainers[0].GetComponent<HeartContainer>();
    }

    public void SetCurrentHealth(float health)
    {
        currentHearts = health;
        currentContainer.SetHeart(currentHearts);
    }

    public void AddHearts(float healthUp)
    {
        currentHearts += healthUp;
        if (currentHearts > totalHearts)
        {
            currentHearts = (float) totalHearts;
        }
        currentContainer.SetHeart(currentHearts);
    }

    public void RemoveHearts(float healthDown)
    {
        currentHearts -= healthDown;
        if (currentHearts < 0)
        {
            currentHearts = 0f;
        }
        currentContainer.SetHeart(currentHearts);
    }

    public void AddContainer()
    {
        if (totalHearts == 15)
        {
            AddHearts(1);
            return;
        }

        GameObject newHeart = Instantiate(heartContainerPrefab, transform);
        currentContainer = heartContainers[heartContainers.Count - 1].GetComponent<HeartContainer>();
        heartContainers.Add(newHeart);

        if (currentContainer != null)
        {
            currentContainer.next = newHeart.GetComponent<HeartContainer>();
        }

        currentContainer = heartContainers[0].GetComponent<HeartContainer>();

        totalHearts++;
        currentHearts++;
        SetCurrentHealth(currentHearts);
    }
}
