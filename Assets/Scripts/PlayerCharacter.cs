using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : LivingEntity
{
    [SerializeField] 
    private CharacterData data;

    private void Awake()
    {
        SetData(data);
    }
}