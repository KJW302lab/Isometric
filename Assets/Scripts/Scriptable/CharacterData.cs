using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Object / Character Data", order = int.MaxValue)]
public class CharacterData : ScriptableObject
{
    public string className;
    public int hp;
    public int att;
    public int ap;
}
