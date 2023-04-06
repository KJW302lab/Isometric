using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileBase Data", menuName = "Scriptable Object / TileBase Data", order = int.MaxValue)]
public class TileBaseData : ScriptableObject
{
    public TileBase ground;
    public TileBase path;
}
