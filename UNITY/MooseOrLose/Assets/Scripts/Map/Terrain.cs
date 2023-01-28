using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    public Terrain(int movementCost, bool isWater, Texture texture)
    {
        this.movementCost = movementCost;
        this.hasWater = hasWater;
        // this.texture = texture;
    }

    [SerializeField] private Color _highlightColor;
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private int movementCost;
    [SerializeField] private bool hasWater;
    // [SerializeField] private Texture texture;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _menu;
    
    public void Init()
    {
        _highlightMaterial.color = _highlightColor;
    }
    
    public int getMovementCost()
    {
        return movementCost;
    }
    public bool isWater()
    {
        return hasWater;
    }
    // public Texture getTexture()
    // {
    //     return texture;
    // }
    // Start is called before the first frame update
    private void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }
    private void OnMouseExit()
    {
        _highlight.SetActive(false);
        _menu.SetActive(false);
    }
    private void OnMouseDown()
    {
        _menu.SetActive(!_menu.activeSelf);
    }
}
