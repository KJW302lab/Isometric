using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Utils
{
    public static TileType GetTileType(TileBase tile)
    {
        if (tile.name == "Grass")
        {
            return TileType.Grass;
        }

        if (tile.name == "Start")
        {
            return TileType.Start;
        }

        if (tile.name == "End")
        {
            return TileType.End;
        }

        if (tile.name == "Road")
        {
            return TileType.Road;
        }

        else
        {
            return TileType.None;
        }
    }
}
