using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private MouseInput _mouseInput;
    private Vector3 _destination;

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
        _destination = transform.position;
        _mouseInput.Mouse.MouseClick.performed += _ => MouseClick();
    }

    void MouseClick()
    {
        Vector2 mousePosition = _mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        // make sure we are clicking the cell
        var map = TilemapManager.Instance.Ground;
        Vector3Int gridPosition = map.WorldToCell(mousePosition);

        if (map.HasTile(gridPosition))
        {
            print(gridPosition);
        }
    }
}
