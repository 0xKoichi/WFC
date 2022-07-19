using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public static class Helpers
{
    public static IEnumerable<T[]> Split<T>(this IEnumerable<T> source, int size)
    {
        List<T> result = new List<T>(size);
        foreach (T x in source)
        {
            result.Add(x);
            if (result.Count == size)
            {
                yield return result.ToArray();
                result = new List<T>(size);
            }
        }
    } 


    public static List<Tile> EvaluateEntropy(this List<Tile> p, List<Neighbor> neighbors)
    {
        var newPotential = new List<Tile>(p);
        foreach (var neighbor in neighbors)
        {
            var nRules = neighbor.GetPotentialEntropy();
            try 
            {
                newPotential = newPotential.Intersect(nRules).ToList();
            }
            catch (ArgumentNullException e)
            {
                // We'll handle this later.
                Debug.LogWarning("No potential entropy found!");
            }
        }
        return newPotential;
    }


    public static List<Tile> GetPotentialEntropy(this Neighbor n) 
    {
        var domain = n.node.GetComponent<Cell>().value.domain;
        var tiles = new List<Tile>();
        switch(n.position)
        {
            case "top":
                tiles = domain.bottom.GetTiles();
                break;
            case "bottom":
                tiles = domain.top.GetTiles();
                break;
            case "left":
                tiles = domain.right.GetTiles();
                break;
            case "right":
                tiles = domain.left.GetTiles();
                break;
        }

        return tiles;
    }  

    public static List<Tile> GetTiles(this TileData[] data)
    {
        var tiles = new List<Tile>();
        foreach (var element in data)
        {
            tiles.Add(element.tile);
        }
        return tiles;
    }

}
