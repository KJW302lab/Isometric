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

    private PlayerCharacter _character;
    private IEnumerator _selectPosCoroutine;
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
        var prefab = SceneManager.Instance.GetCharacterPrefab(classType); 
        _character = prefab.GetComponent<PlayerCharacter>();
        var data = SceneManager.Instance.GetCharacterData(classType);
        _character.SetData(data);

        InputManager.Instance.ClickedPos += SetPosition;
        _selectPosCoroutine = SelectPosition(_character);
        StartCoroutine(_selectPosCoroutine);
    }

    IEnumerator SelectPosition(PlayerCharacter character)
    {
        while (!character.IsBuilt)
        {
            yield return null;
            character.gameObject.transform.position = InputManager.Instance.PointerPosition;
        }
    }

    void SetPosition(Vector3 position)
    {
        _character.transform.position = position;
        StopCoroutine(_selectPosCoroutine);
        _character.IsBuilt = true;
        _character.Initialize();
        
        Refresh();
    }

    void Refresh()
    {
        _character = null;
        _selectPosCoroutine = null;
        InputManager.Instance.ClickedPos -= SetPosition;
    }
}
