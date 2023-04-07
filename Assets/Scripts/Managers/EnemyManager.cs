using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyObj;
    public int amount;

    private float _speed = 1;
    private int _looped = 0;
    private Queue<GameObject> _enemyList = new();

    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(enemyObj);
            
            go.SetActive(false);
            
            _enemyList.Enqueue(go);
        }

        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        foreach (var enemy in _enemyList)
        {
            StartCoroutine(StartAttack(enemy));

            yield return new WaitForSeconds(1f);   
        }
    }

    IEnumerator StartAttack(GameObject enemy)
    {
        var path = TilemapManager.Instance.GetPath();
        var startPoint = path.Dequeue();

        enemy.transform.position = startPoint;
        enemy.SetActive(true);

        yield return new WaitForSeconds(1f);

        var pathCount = path.Count;

        for (int i = 0; i < pathCount; i++)
        {
            var dest = path.Dequeue();
            
            while (Vector3.Distance(enemy.transform.position, dest) > 0.1)
            {
                yield return null;
            
                enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, dest, _speed * Time.deltaTime);
            }   
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            _speed++;
        }

        if (Input.GetKey(KeyCode.E))
        {
            _speed--;

            if (_speed <= 0)
            {
                _speed = 1;
            }
        }
        
        print(_speed);
    }
}
