using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFind
{
    public static Queue<Vector3Int> GetPathList(Vector3Int startPoint, Vector3Int goalPoint, List<Vector3Int> roads)
    {
        Queue<Vector3Int> path = new();

        Vector3Int current = startPoint;
        
        while (true)
        {
            if (current == goalPoint)
            {
                path.Enqueue(goalPoint);
                break;
            }

            if (roads.Count <= 0)
            {
                break;
            }
            
            path.Enqueue(current);
            roads.Remove(current);

            current = GetNearestTile(current, roads);
        }

        return path;
    }

    private static Vector3Int GetNearestTile(Vector3Int currentTile ,List<Vector3Int> roads)
    {
        var arr = roads.OrderBy(tile => Vector3Int.Distance(currentTile, tile)).ToArray();

        return arr[0];
    }
}
