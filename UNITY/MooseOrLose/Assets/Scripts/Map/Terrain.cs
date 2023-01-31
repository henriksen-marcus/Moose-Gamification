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

    private Ray _ray;
    private RaycastHit _hit;

    public void Init()
    {
        highlightMaterial.color = highlightColor;
        _ray = new Ray(mooseSpawnPoint.position, -mooseSpawnPoint.up);
        if (Physics.Raycast(_ray, out _hit))
        {
            var pos = mooseSpawnPoint.position;
            pos = new Vector3(pos.x, pos.y -= _hit.distance - 1, pos.z);
            mooseSpawnPoint.position = pos;
        }
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
