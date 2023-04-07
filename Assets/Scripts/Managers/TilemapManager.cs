using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileData
{
    public TileType type;

    public TileData(TileBase tile)
    {
        type = Utils.GetTileType(tile);
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

    private void MakeDict()
    {
        foreach (Vector3Int pos in Ground.cellBounds.allPositionsWithin)
        {
            if (!Ground.HasTile(pos)) continue;

            var tile = Ground.GetTile(pos);

            TileData data = new TileData(tile);

            if (data.type == TileType.Start)
            {
                _startTile = pos;
            }

            if (data.type == TileType.End)
            {
                _goalTile = pos;
            }

            if (data.type == TileType.Road)
            {
                _roads.Add(pos);
            }

            _tileDict[pos] = data;
        }
        
        _roads.Add(_startTile);
        _roads.Add(_goalTile);
    }

    private void Awake()
    {
        MakeDict();
    }

    public Queue<Vector3> GetPath()
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
}
