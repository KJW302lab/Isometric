using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMain : MonoBehaviour
{
    [SerializeField] private TMP_Text timer;
    [SerializeField] private Transform monsterInfo;
    [SerializeField] private GameObject monsterInfoItem;
    
    [SerializeField] private List<Sprite> monsterImages = new();

    public void SetTimerText(int sec)
    {
        var time = sec.ToString();

        timer.text = $"{time}s";
    }

    public void AddMonsterInfo(WaveInfo info)
    {
        foreach (var pair in info.WaveDict)
        {
            var type = pair.Key;
            var value = pair.Value;
        
            var sprite = monsterImages[(int)type];
            var monsterName = type.ToString();

            MonsterInfoItem item = Instantiate(monsterInfoItem).GetComponent<MonsterInfoItem>();
        
            item.Initialize(sprite, monsterName, value);
            item.transform.SetParent(monsterInfo, false);   
        }
    }
}
