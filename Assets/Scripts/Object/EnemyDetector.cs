using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public event Action<EnemyCharacter> EnemyDetected; 

    private CircleCollider2D _collider;

    public void Initialize(PlayerCharacter character, Vector3Int tile)
    {
        _collider = gameObject.AddComponent<CircleCollider2D>();
        
        _collider.radius = 0.75f;
        _collider.offset = new Vector2(0f, 1f);
        _collider.isTrigger = true;

        var parent = character.transform;
        
        transform.SetParent(parent, false);
        transform.position = TilemapManager.Instance.CellToWorld(tile);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var enemy = other.GetComponent<EnemyCharacter>();
        
        if (enemy != null)
        {
            EnemyDetected?.Invoke(enemy);
        }
    }
}
