using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public event Action<Vector3> ClickedPos; 

    public Vector2 PointerPosition => Camera.main.ScreenToWorldPoint(_mouseInput.Mouse.MousePosition.ReadValue<Vector2>());

    private Vector3Int _gridPos;
    private MouseInput _mouseInput;

    private void Awake()
    {
        _mouseInput = new MouseInput();
    }

    private void OnEnable()
    {
        _mouseInput.Enable();
    }

    private void OnDisable()
    {
        _mouseInput.Disable();
    }

    private void Start()
    {
        _mouseInput.Mouse.MouseClick.performed += _ => SetPositionForCharacter();
        _mouseInput.Mouse.MousePosition.performed += _ => HasTileInPointerPos();
    }

    public void SetPositionForCharacter()
    {
        if (HasTileInPointerPos())
        {
            ClickedPos?.Invoke(TilemapManager.Instance.CellToWorld(_gridPos));
        }
    }

    public bool HasTileInPointerPos()
    {
        Vector2 mousePosition = _mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        // make sure we are clicking the cell
        var map = TilemapManager.Instance.Ground;
        _gridPos = map.WorldToCell(mousePosition);
        
        return map.HasTile(_gridPos);
    }
}
