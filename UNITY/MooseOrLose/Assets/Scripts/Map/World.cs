using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] private Terrain[][] tiles_;

    [SerializeField] private Terrain grassTerrain;
    [SerializeField] private Terrain hillTerrain;
    [SerializeField] private Terrain riverTerrain;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

    }
    
    int getMovementCost(int x, int y)
    {
        return tiles_[x][y].getMovementCost();
    }

    bool isWater(int x, int y)
    {
        return tiles_[x][y].isWater();
    }
    
    

}
