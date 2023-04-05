using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    
    public Tilemap map;
    
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
        Vector3Int gridPosition = map.WorldToCell(mousePosition);
        
        if (map.HasTile(gridPosition))
        {
            _destination = mousePosition;
        }
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _destination) > 0.1f)
            transform.position = Vector3.MoveTowards(transform.position, _destination,
                movementSpeed * Time.deltaTime);
        
        
    }
}
