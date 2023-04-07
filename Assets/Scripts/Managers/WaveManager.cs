using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyObj;
    public EnemyData data;
    public int amount;

    private int _id = 0;
    private Queue<EnemyCharacter> _enemyList = new();

    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(enemyObj);
            
            go.SetActive(false);

            EnemyCharacter enemyCharacter = go.GetComponent<EnemyCharacter>();
            
            enemyCharacter.Initialize(data, ++_id, go);

            _enemyList.Enqueue(enemyCharacter);
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

    IEnumerator StartAttack(EnemyCharacter enemy)
    {
        var monster = enemy.monsterObj;
        var path = TilemapManager.Instance.GetPathToGo();
        var startPoint = path.Dequeue();

        monster.transform.position = startPoint;
        monster.SetActive(true);

        yield return new WaitForSeconds(1f);

        var pathCount = path.Count;

        for (int i = 0; i < pathCount; i++)
        {
            var dest = path.Dequeue();

            while (Vector3.Distance(monster.transform.position, dest) > 0.1)
            {
                yield return null;

                if (!enemy.isAlive)
                {
                    yield break;
                }

                monster.transform.position = Vector3.MoveTowards(monster.transform.position, dest, enemy.Speed * Time.deltaTime);
            }   
        }
    }
}
