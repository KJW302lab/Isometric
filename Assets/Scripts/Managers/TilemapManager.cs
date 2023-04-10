using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileData
{
    public TileType TileType;
    public bool IsCharacterPlaced;

    public TileData(TileBase tile)
    {
        TileType = Utils.GetTileType(tile);
    }
}

public class TilemapManager : Singleton<TilemapManager>
{
    // properties
    private SceneGrid SceneGrid => FindObjectOfType<SceneGrid>();
    public Tilemap Ground => SceneGrid.Ground; 
    
    // fields
    private Vector3Int _startTile;
    private Vector3Int _goalTile;
    
    private Dictionary<Vector3Int, TileData> _tileDict = new();
    private List<Vector3Int>                 _roads = new();
    private Queue<Vector3Int>                _path = new();

    
    
    private void Awake()
    {
        MakeDict();
    }

    private void Start()
    {
        CharacterSpawnManager.Instance.CharacterPlaced += CharacterOnTile;
    }

    private void MakeDict()
    {
        foreach (Vector3Int pos in Ground.cellBounds.allPositionsWithin)
        {
            if (!Ground.HasTile(pos)) continue;

            var tile = Ground.GetTile(pos);

            TileData data = new TileData(tile);

            if (data.TileType == TileType.Start)
            {
                _startTile = pos;
            }

            if (data.TileType == TileType.End)
            {
                _goalTile = pos;
            }

            if (data.TileType == TileType.Road)
            {
                _roads.Add(pos);
            }

            _tileDict[pos] = data;
        }
        
        _roads.Add(_startTile);
        _roads.Add(_goalTile);
    }

    public void ChangeStartGoalTileToRoad(TileBase roadTile)
    {
        Ground.SetTile(_startTile, roadTile);
        Ground.SetTile(_goalTile, roadTile);
    }

    private bool IsRoadTile(Vector3Int tile)
    {
        return Ground.GetTile(tile).name == "Road";
    }

    public Queue<Vector3> GetPathToGo()
    {
        if (_path.Count <= 0)
        {
            _path = PathFind.GetPathList(_startTile, _goalTile, _roads);
        }

        Queue<Vector3> worldPath = new();
        
        foreach (Vector3Int tile in _path)
        {
            worldPath.Enqueue(Ground.CellToWorld(tile));
        }

        return worldPath;
    }

    public Vector3Int WorldToCell(Vector3 currentPosition)
    {
        return Ground.WorldToCell(currentPosition);
    }

    public Vector3 CellToWorld(Vector3Int cell)
    {
        return Ground.CellToWorld(cell);
    }

    public List<Vector3Int> GetTilesInRange(Vector3Int currentTile ,int attackRange)
    {
        List<Vector3Int> list = new();

        Vector3Int start = new Vector3Int(currentTile.x - attackRange, currentTile.y - attackRange, 0);

        int searchRange = attackRange * 2 + 1; 
        
        for (int i = 0; i < searchRange; i++)
        {
            for (int j = 0; j < searchRange; j++)
            {
                Vector3Int specificTile = new Vector3Int(start.x + i, start.y + j, 0);

                if (!Ground.HasTile(specificTile)) continue;

                if (!IsRoadTile(specificTile)) continue;
                
                list.Add(specificTile);
            }
        }

        return list;
    }

    public bool CanPlaceCharacter(Vector3Int tilePos)
    {
        if (Ground.HasTile(tilePos))
        {
            if (_tileDict[tilePos].TileType == TileType.Grass)
            {
                return !_tileDict[tilePos].IsCharacterPlaced;   
            }
        }

        return false;
    }

    void CharacterOnTile(Vector3 tileWorldPos)
    {
        var tilePos = WorldToCell(tileWorldPos);

        if (Ground.HasTile(tilePos))
        {
            _tileDict[tilePos].IsCharacterPlaced = true;
        }
    }
}
