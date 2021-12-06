using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarEnemy : MonoBehaviour
{
    [SerializeField] private Transform fill;
    private Vector3 maxHealth;
    private Vector3 localScale;

    void Start()
    {
        maxHealth = fill.localScale;
        localScale = fill.localScale;
    }

    public void UpdateHealth(float health)
    {
        localScale.x = maxHealth.x * health;
        fill.localScale = localScale;
    }
}
