using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public bool IsBuilt { get; set; } = false;
    
    private CharacterData _data;

    private ClassType _classType = ClassType.None;
    private int _attackRange = -1;
    private int _attackPoint = -1;
    private float _attackSpeed = -1f;

    private bool _canAttack = true;
    private List<EnemyDetector> _colliders = new();

    #region Properties
    // stats
    protected ClassType ClassType
    {
        get
        {
            if (_classType == ClassType.None)
            {
                _classType = _data.type;
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
                _attackRange = _data.attackRange;
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
                _attackPoint = _data.attackPoint;
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
                _attackSpeed = _data.attackSpeed;
            }

            return _attackSpeed;
        }

        set => _attackSpeed = value;
    }
    
    // about attack
    protected Vector3Int CurrentTile => TilemapManager.Instance.WorldToCell(transform.position);
    protected List<Vector3Int> TilesInRange => TilemapManager.Instance.GetTilesInRange(CurrentTile, AttackRange);
    #endregion

    public void SetData(CharacterData data)
    {
        _data = data;
    }
    
    public void Initialize()
    {
        SetColliders();
    }

    private void SetColliders()
    {
        if (_colliders.Count > 0)
        {
            foreach (var circleCollider in _colliders)
            {
                Destroy(circleCollider.gameObject);
            }
        }
        
        _colliders.Clear();
        
        foreach (Vector3Int tile in TilesInRange)
        {
            var go = new GameObject { name = $"Collider {tile}" };

            EnemyDetector detector = go.AddComponent<EnemyDetector>();
            
            detector.Initialize(this, tile);

            detector.EnemyDetected += AttackEnemy;

            _colliders.Add(detector);
        }
    }

    void AttackEnemy(EnemyCharacter enemy)
    {
        if (_canAttack)
        {
            StartCoroutine(Attack(enemy));
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
