using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyObj;
    public int amount;

    private List<GameObject> _enemyList = new();

    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(enemyObj);
            
            go.SetActive(false);
            
            _enemyList.Add(go);
        }
    }
}
