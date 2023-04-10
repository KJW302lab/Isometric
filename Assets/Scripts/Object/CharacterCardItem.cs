using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardItem : MonoBehaviour
{
    public ClassType classType;

    [SerializeField] private TMP_Text txtClassName;
    [SerializeField] private Image imgClassImg;
    [SerializeField] private TMP_Text txtSpec;
    
    private Button _btnSelect;

    private void Awake()
    {
        _btnSelect = GetComponent<Button>();

        switch (classType)
        {
            case ClassType.Warrior:
                imgClassImg.color = Color.black;
                break;
            case ClassType.Mage:
                imgClassImg.color = Color.white;
                break;
        }
        
        
        SetCardInfo();
        _btnSelect.onClick.AddListener(LoadCharacter);
        
    }

    void SetCardInfo()
    {
        txtClassName.text = classType.ToString();
    }

    void LoadCharacter()
    {
        CharacterSpawnManager.Instance.LoadPrefab(classType);
    }
}
