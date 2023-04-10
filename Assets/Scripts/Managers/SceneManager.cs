using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : Singleton<SceneManager>
{
    [Header("Player")]
    [SerializeField] private List<GameObject> characterPrefabList = new();
    [SerializeField] private List<CharacterData> characterDataList = new();
    
    [Header("Monster")]
    [SerializeField] private List<GameObject> monsterPrefabList = new();
    [SerializeField] private List<EnemyData> monsterDataList = new();

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
        return Instantiate(monsterPrefabList[(int)type]);
    }

    public EnemyData GetEnemyData(EnemyType type)
    {
        return monsterDataList[(int)type];
    }
    
    public GameObject GetCharacterPrefab(ClassType type)
    {
        var prefab = Instantiate(characterPrefabList[(int)type]);
            
        var spriteRenderer = prefab.GetComponent<SpriteRenderer>();
        
        switch (type)
        {
            case ClassType.Warrior:
                spriteRenderer.color = Color.black;
                break;
            
            case ClassType.Mage:
                spriteRenderer.color = Color.white;
                break;
        }

        return prefab;
    }

    public CharacterData GetCharacterData(ClassType type)
    {
        return characterDataList[(int)type];
    }
}
