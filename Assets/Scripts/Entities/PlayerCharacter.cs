using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private CharacterData data;

    private ClassType _classType = ClassType.None;
    private int _attackRange = -1;
    private int _attackPoint = -1;
    private float _attackSpeed = -1f;

    private bool _canAttack = true;

    #region Properties
    // stats
    protected ClassType ClassType
    {
        get
        {
            if (_classType == ClassType.None)
            {
                _classType = data.type;
            }

            return _classType;
        }

        set => _classType = value;
    }
    protected int AttackRange
    {
        get
        {
            if (_attackRange < 0)
            {
                _attackRange = data.attackRange;
            }

            return _attackRange;
        }

        set => _attackRange = value;
    }
    protected int AttackPoint
    {
        get
        {
            if (_attackPoint < 0)
            {
                _attackPoint = data.attackPoint;
            }

            return _attackPoint;
        }

        set => _attackPoint = value;
    }

    protected float AttackSpeed
    {
        get
        {
            if (_attackSpeed < 0)
            {
                _attackSpeed = data.attackSpeed;
            }

            return _attackSpeed;
        }

        set => _attackSpeed = value;
    }
    
    // about attack
    protected Vector3Int CurrentTile => TilemapManager.Instance.GetCurrentTile(transform.position);
    protected List<Vector3Int> TilesInRange => TilemapManager.Instance.GetTilesInRange(CurrentTile, AttackRange);
    #endregion

    private void Start()
    {
        TilemapManager.Instance.EnemiesOnTile += AttackEnemy;
    }

    void AttackEnemy(Vector3Int tile, EnemyCharacter enemy)
    {
        foreach (Vector3Int road in TilesInRange)
        {
            if (tile == road)
            {
                if (_canAttack)
                {
                    StartCoroutine(Attack(enemy));
                }
            }
        }
    }

    IEnumerator Attack(EnemyCharacter enemy)
    {
        enemy.OnDamage(AttackPoint);
        
        _canAttack = false;

        yield return new WaitForSeconds(1f / AttackSpeed);

        _canAttack = true;
    }
}
