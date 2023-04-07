using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterInfoItem : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text monsterName;
    [SerializeField] private TMP_Text monsterQuantity;
    
    public void Initialize(Sprite sprite, string monsterName, int quantity)
    {
        image.sprite = sprite;
        this.monsterName.text = monsterName;
        monsterQuantity.text = quantity.ToString();
    }
}
