using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Object / Enemy Data", order = int.MaxValue)]
public class EnemyData : ScriptableObject
{
    public int hp;
    public float speed;
}
