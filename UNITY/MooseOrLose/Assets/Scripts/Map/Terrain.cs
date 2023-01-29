using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class Terrain : MonoBehaviour, IDropHandler
{
    public Terrain(int movementCost, bool isWater, Texture texture)
    {
        this.movementCost = movementCost;
        this.hasWater = hasWater;
        // this.texture = texture;
    }
    
    [SerializeField] private ElgBT moosePrefab;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private int movementCost;
    [SerializeField] private bool hasWater;
    // [SerializeField] private Texture texture;
    [SerializeField] private GameObject highlight;
    [SerializeField] private GameObject menu;
    // [SerializeField] private BoxCollider _collider;
    [SerializeField] private Transform mooseSpawnPoint;

    public void Init()
    {
        highlightMaterial.color = highlightColor;
    }

    // private void Update()
    // {
    //     throw new NotImplementedException();
    // }

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

    private void OnMouseEnter()
    {
        if (MapUI.GridOn)
           highlight.SetActive(true);
    }
    private void OnMouseExit()
    {
        highlight.SetActive(false);
        menu.SetActive(false);
    }
    private void OnMouseDown()
    {
        if (MapUI.GridOn)
            menu.SetActive(!menu.activeSelf);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("On Dropped");
        Instantiate(moosePrefab, mooseSpawnPoint.position, Quaternion.identity);
    }
}
