using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : LivingEntity
{
    [SerializeField] 
    private CharacterData data;

    public override void Awake()
    {
        base.Awake();
        
        SetData(data);
    }
}
