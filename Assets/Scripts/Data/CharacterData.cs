using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Object / Character Data", order = int.MaxValue)]
public class CharacterData : ScriptableObject
{
    public ClassType type;
    public int attackRange;
    public int attackPoint;
    public float attackSpeed;
}
