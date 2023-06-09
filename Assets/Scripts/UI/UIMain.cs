using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    [Header("Wave Information")]
    [SerializeField] private TMP_Text txtTimer;
    [SerializeField] private TMP_Text txtRemainEnemies;
    [SerializeField] private Transform monsterInfo;
    [SerializeField] private GameObject monsterInfoItem;
    [SerializeField] private List<Sprite> monsterImages = new();

    [Header("Menu")] 
    [SerializeField] private RectTransform bottomNav;
    [SerializeField] private Button btnCharacter;
    
    private bool _isBotNavOpen;

    private void Awake()
    {
        Initialize();
        SetTimerText(WaveManager.Instance.IntervalSec);
    }

    public void Initialize()
    {
        _isBotNavOpen = false;
        bottomNav.gameObject.SetActive(_isBotNavOpen);
        
        btnCharacter.onClick.AddListener(SetBottomNavActive);
    }

    #region Wave Informations

    public void SetTimerText(int sec)
    {
        if (sec <= 0)
        {
            txtTimer.text = "Wave Start!";
            return;
        }

        var time = sec.ToString();

        txtTimer.text = $"{time}s";
    }

    public void UpdateRemainEnemies(int quantity)
    {
        txtRemainEnemies.gameObject.SetActive(true);
        
        txtRemainEnemies.text = $"Remain Enemies : {quantity}";
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

    public void ClearMonsterInfo()
    {
        var infos = monsterInfo.GetComponentsInChildren<MonsterInfoItem>();

        foreach (var infoItem in infos)
        {
            Destroy(infoItem.gameObject);
        }
    }

    #endregion

    #region Menu

    void SetBottomNavActive()
    {
        _isBotNavOpen = !_isBotNavOpen;
        bottomNav.gameObject.SetActive(_isBotNavOpen);
    }

    #endregion
}
