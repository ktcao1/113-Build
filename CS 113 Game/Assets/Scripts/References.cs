using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();
    public Dictionary<string, GameObject> enemyDict = new Dictionary<string, GameObject>();
    public List<GameObject> objectList = new List<GameObject>();
    public Dictionary<string, GameObject> objectDict = new Dictionary<string, GameObject>();
    public List<Sprite> spriteList = new List<Sprite>();
    public Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

    void Awake()
    {
        foreach (GameObject enemy in enemyList)
        {
            enemyDict.Add(enemy.name, enemy);
        }
        foreach (GameObject obj in objectList)
        {
            objectDict.Add(obj.name, obj);
        }
        foreach (Sprite spr in spriteList)
        {
            spriteDict.Add(spr.name, spr);
        }
    }
}
