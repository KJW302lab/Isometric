using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : LivingEntity
{
    [SerializeField] 
    private CharacterData data;

    private void Awake()
    {
        SetData(data);
    }
}
