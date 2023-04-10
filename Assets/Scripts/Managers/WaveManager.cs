using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveInfo
{
    public Dictionary<EnemyType, int> WaveDict = new();

    public void AddWave(EnemyType enemyType, int value)
    {
        WaveDict.Add(enemyType, value);
    }
}

public class WaveManager : Singleton<WaveManager>
{
    public readonly int IntervalSec = 3;
    
    private List<EnemyCharacter> _enemyList = new();
    private List<EnemyCharacter> _destroyList = new();
    private int _monsterQuantity;

    private UIMain UIMain => UIManager.Instance.GetUI<UIMain>();

    private Transform _enemies;

    private Transform Enemies
    {
        get
        {
            if (_enemies == null)
            {
                _enemies = new GameObject { name = "Enemies" }.transform;
            }

            return _enemies;
        }
    }

    public void StartInterval()
    {
        var waveInfo = SceneManager.Instance.GetNextInfo();

        if (waveInfo == null)
        {
            print("Scene End");
            return;
        }
        
        UIMain.AddMonsterInfo(waveInfo);
        StartCoroutine(StartCountDown(waveInfo));
    }

    void OnWaveFinish()
    {
        print("Wave Finished");
        
        UIMain.ClearMonsterInfo();
        
        foreach (var enemy in _destroyList)
        {
            Destroy(enemy.gameObject);
        }
        
        _destroyList.Clear();
        
        _enemyList.Clear();
        
        StartInterval();
    }
    
    IEnumerator StartCountDown(WaveInfo info)
    {
        var interval = IntervalSec;
        
        while (interval >= 0)
        {
            yield return new WaitForSeconds(1f);

            interval--;
            
            UIMain.SetTimerText(interval);
        }
        
        BeginWave(info);
    }

    private void BeginWave(WaveInfo info)
    {
        foreach (var pair in info.WaveDict)
        {
            var quantity = pair.Value;
            var type = pair.Key;
            var data = SceneManager.Instance.GetEnemyData(type);

            for (int i = 0; i < quantity; i++)
            {
                GameObject go = SceneManager.Instance.GetEnemyPrefab(type);
            
                go.SetActive(false);

                EnemyCharacter enemyCharacter = go.GetComponent<EnemyCharacter>();
            
                enemyCharacter.Initialize(data, go);

                enemyCharacter.Defeated += CountDefeatedMonster;
                
                go.transform.SetParent(Enemies, false);

                _enemyList.Add(enemyCharacter);

                _monsterQuantity++;
                
                UIMain.UpdateRemainEnemies(_monsterQuantity);
            }   
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

    void CountDefeatedMonster(EnemyCharacter enemyCharacter)
    {
        if (_enemyList.Contains(enemyCharacter))
        {
            if (_destroyList.Contains(enemyCharacter))
            {
                return;
            }
            
            _monsterQuantity--;
            UIMain.UpdateRemainEnemies(_monsterQuantity);
            _destroyList.Add(enemyCharacter);
            enemyCharacter.gameObject.SetActive(false);
        }

        if (_monsterQuantity <= 0)
        {
            OnWaveFinish();
        }
    }
}
