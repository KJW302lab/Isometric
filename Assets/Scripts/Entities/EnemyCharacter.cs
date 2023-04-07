using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCharacter : MonoBehaviour
{
    [SerializeField] private Scrollbar hpIndicator;
    
    public int id;
    public GameObject monsterObj;
    public bool isAlive = true;

    private int _maxHp;
    private int _hp = 0;
    private float _speed = 0;

    public int Hp
    {
        get => _hp;
        set => _hp = value;
    }
    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }
    
    public void Initialize(EnemyData enemyData, int id, GameObject monsterObj)
    {
        Speed = enemyData.speed;
        Hp = enemyData.hp;
        _maxHp = enemyData.hp;
        
        this.id = id;
        this.monsterObj = monsterObj;
    }

    public void OnDamage(int attackPoint)
    {
        if (monsterObj == null)
        {
            return;
        }
        
        Hp -= attackPoint;
        hpIndicator.size = (float)Hp / _maxHp;

        if (Hp <= 0)
        {
            isAlive = false;
            Destroy(monsterObj);
        }
    }
}
