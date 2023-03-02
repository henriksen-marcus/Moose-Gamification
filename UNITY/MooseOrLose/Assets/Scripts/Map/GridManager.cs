using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _widthX, _widthZ;
    
    [SerializeField] private Terrain _terrainPrefab;
    // [SerializeField] private Terrain grassTerrain;
    // [SerializeField] private Terrain hillTerrain;
    // [SerializeField] private Terrain riverTerrain;
    [SerializeField] private GameObject tileParent;
    
    // [SerializeField] private Transform _mapMesh;
    // [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Terrain> _terrainTiles;

    // private void Awake()
    // {
    //     
    // }
    
    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        _terrainTiles = new Dictionary<Vector2, Terrain>();
        for (int x = 0; x < _widthX; x++)
        {
            for (int z = 0; z < _widthZ; z++)
            {
                var position = transform.position;
                var spawnedTerrain = Instantiate(_terrainPrefab, new Vector3(x * 10 + position.x, 15, z * 10 + position.z), Quaternion.identity, tileParent.transform);
                spawnedTerrain.name = $"Terrain {x} {z}";
                spawnedTerrain.Init();
                _terrainTiles[new Vector2(x, z)] = spawnedTerrain;
                Debug.Log("Spawned tile");
            }
        }
        
        // _cam.transform.position = new Vector3((float)_widthX/2-0.5f, 80, (float)_widthX/ 2 -0.5f);
        // _mapMesh.transform.position = new Vector3(50, 0, 50);
    }
    
    int GetMovementCost(Vector2 pos)
    {
        if (_terrainTiles.TryGetValue(pos, out var tile))
        {
            return tile.getMovementCost();
        }

        return -1;
    }

    bool IsWater(Vector2 pos)
    {
        if (_terrainTiles.TryGetValue(pos, out var tile))
        {
            return tile.isWater();
        }

        return false;
    }
    
    

}
