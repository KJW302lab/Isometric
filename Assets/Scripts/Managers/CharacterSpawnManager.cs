using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawnManager : Singleton<CharacterSpawnManager>
{
    public event Action<Vector3> CharacterPlaced; 

    private PlayerCharacter _character;
    private IEnumerator _selectPosCoroutine;

    private Transform _playerCharacters;

    public Transform PlayerCharacters
    {
        get
        {
            if (_playerCharacters == null)
            {
                _playerCharacters = new GameObject { name = "PlayerCharacter" }.transform;
            }

            return _playerCharacters;
        }
    }
    
    public void LoadPrefab(ClassType classType)
    {
        var prefab = SceneManager.Instance.GetCharacterPrefab(classType); 
        _character = prefab.GetComponent<PlayerCharacter>();
        var data = SceneManager.Instance.GetCharacterData(classType);
        _character.SetData(data);
        
        prefab.transform.SetParent(PlayerCharacters, false);

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
        CharacterPlaced?.Invoke(position);
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
