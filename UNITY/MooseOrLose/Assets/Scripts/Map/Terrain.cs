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
        this.hasWater = isWater;
        // this.texture = texture;
    }
    
    // [SerializeField] private ElgBT moosePrefab;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private int movementCost;
    [SerializeField] private bool hasWater;
    // [SerializeField] private Texture texture;
    [SerializeField] private GameObject highlight;
    [SerializeField] private GameObject menu;
    // [SerializeField] private BoxCollider _collider;
    [SerializeField] private Transform spawnPoint;
    
    private Transform _mooseParent;

    private Ray _ray;
    private RaycastHit _hit;

    public void Init()
    {
        highlightMaterial.color = highlightColor;
        _ray = new Ray(spawnPoint.position, -spawnPoint.up);
        if (Physics.Raycast(_ray, out _hit))
        {
            
            var pos = spawnPoint.position;
            pos = new Vector3(pos.x, pos.y -= _hit.distance - 1, pos.z);
            spawnPoint.position = pos;
        }
        _mooseParent = ElgManager.instance.transform;
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
        if (InfoUI.Instance.gridOn)
           highlight.SetActive(true);
    }
    private void OnMouseExit()
    {
        highlight.SetActive(false);
        menu.SetActive(false);
    }
    private void OnMouseDown()
    {
        if (InfoUI.Instance.gridOn)
            menu.SetActive(!menu.activeSelf);
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Debug.Log("On Dropped");
        if (eventData.pointerDrag.TryGetComponent(out DragDrop drop))
        {
            switch (drop.prefab.tag)
            {
                case "Elg":
                    ElgManager.instance.Spawn(spawnPoint.position);
                    break;
                case "Jeger":
                    JegerManager.instance.Spawn(spawnPoint.position);
                    break;
                case "Ulv":
                    UlvManager.instance.SpawnPack(UnityEngine.Random.Range(1,3),spawnPoint.position);
                    break;
                default:
                    break;
            }
            // Debug.Log("Dropped " + drop.prefab.name + " into world");
            ElgManager.instance.AddToList(drop.prefab);
        }
        else
        {
            Debug.Log("Could not retrieve DragDrop instance");
        }
    }
}
