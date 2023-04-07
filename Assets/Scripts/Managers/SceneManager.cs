using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : Singleton<SceneManager>
{
    [SerializeField] private List<GameObject> prefabList = new();
    [SerializeField] private List<EnemyData> enemyDataList = new();

    private readonly Queue<WaveInfo> _waveList = new();

    private int _waveCount;

    private void Awake()
    {
        _waveList.Enqueue(AddQueue(EnemyType.Man, 20));
        _waveList.Enqueue(AddQueue(EnemyType.Man, 30));

        _waveCount = _waveList.Count;
    }

    WaveInfo AddQueue(EnemyType enemyType, int value)
    {
        WaveInfo info = new WaveInfo();
        
        info.AddWave(enemyType, value);

        return info;
    }

    private void Start()
    {
        StartCoroutine(nameof(PrepareWave));
    }

    IEnumerator PrepareWave()
    {
        for (int i = 0; i < _waveCount; i++)
        {
            yield return new WaitUntil(() => WaveManager.Instance.IsWaveOver);
            
            var info = _waveList.Dequeue();
            
            WaveManager.Instance.StartInterval(info);
        }
    }

    public GameObject GetEnemyPrefab(EnemyType type)
    {
        return Instantiate(prefabList[(int)type]);
    }

    public EnemyData GetEnemyData(EnemyType type)
    {
        return enemyDataList[(int)type];
    }
}
