using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneGrid : MonoBehaviour
{
    [SerializeField] private Tilemap ground;
    [SerializeField] private TileBaseData tileBaseData;
    public Tilemap Ground => ground;
}
