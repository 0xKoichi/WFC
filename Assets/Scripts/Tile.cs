using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Tile", order = 1)]
public class Tile : ScriptableObject
{
   
    public string tileName;
    public Sprite sprite;
    public Domain domain;
}

[System.Serializable]
public struct Domain
{ 
    public TileData[] top;
    public TileData[] bottom;
    public TileData[] left;
    public TileData[] right;
}

[System.Serializable]
public struct TileData
{

    public Tile tile;
    
    [Range(0.1f, 1.0f)]
    public float probability;

}