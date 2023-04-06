using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : Singleton<SequenceManager>
{
    private List<PlayerCharacter> _playerList = new();
    private List<EnemyCharacter>  _enemyList  = new();

    public void RegisterCharacter(LivingEntity entity)
    {
        var player = entity.GetComponent<PlayerCharacter>();
        var enemy  = entity.GetComponent<EnemyCharacter>();

        if (player != null)
        {
            _playerList.Add(player);
        }
        else
        {
            _enemyList.Add(enemy);
        }
    }
}
