using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneManager : Singleton<SceneManager>
{
    [Header("Tiles")] 
    public TileBase roadTile;
    
    [Header("Player")]
    [SerializeField] private List<GameObject> characterPrefabList = new();
    [SerializeField] private List<CharacterData> characterDataList = new();
    
    [Header("Monster")]
    [SerializeField] private List<GameObject> monsterPrefabList = new();
    [SerializeField] private List<EnemyData> monsterDataList = new();

    private readonly Queue<WaveInfo> _waveList = new();

    private void Awake()
    {
        _waveList.Enqueue(AddQueue(EnemyType.Man, 20));
        _waveList.Enqueue(AddQueue(EnemyType.Man, 30));
        
        TilemapManager.Instance.ChangeStartGoalTileToRoad(roadTile);
    }

    WaveInfo AddQueue(EnemyType enemyType, int value)
    {
        WaveInfo info = new WaveInfo();
        
        info.AddWave(enemyType, value);

        return info;
    }

    private void Start()
    {
        WaveManager.Instance.StartInterval();
    }

    public WaveInfo GetNextInfo()
    {
        if (_waveList == null || _waveList.Count <= 0)
        {
            return null;
        }

        return _waveList.Dequeue();
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
        return Instantiate(characterPrefabList[(int)type]);
    }

    public CharacterData GetCharacterData(ClassType type)
    {
        return characterDataList[(int)type];
    }
}
