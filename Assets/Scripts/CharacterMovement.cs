using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private TileBase clickedTile;
    
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
        
        SelectTiles(gridPosition);
    }

    void SelectTiles(Vector3Int gridPosition)
    {
        var list = MakeList(gridPosition);
        
        foreach (var pos in list)
        {
            map.SetTile(pos, clickedTile);
        }
    }

    List<Vector3Int> MakeList(Vector3Int gridPosition)
    {
        List<Vector3Int> list = new();
        int[] arr = new[] { -1, 1, -1, 1 };

        var pos = new Vector3Int();
        
        for (int i = 0; i < 4; i++)
        {
            if (i <= 1)
            {
                pos = new Vector3Int((int)gridPosition.x + arr[i], (int)gridPosition.y);
            }
            else
            {
                pos = new Vector3Int((int)gridPosition.x, (int)gridPosition.y + arr[i]);
            }

            if (map.HasTile(pos))
            {
                list.Add(pos);
            }
        }

        return list;
    }

    private void Update()
    {
        // if (Vector3.Distance(transform.position, _destination) > 0.1f)
        //     transform.position = Vector3.MoveTowards(transform.position, _destination,
        //         movementSpeed * Time.deltaTime);
    }
}
