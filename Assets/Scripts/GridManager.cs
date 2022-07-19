using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Random;
using System;

public class GridManager : MonoBehaviour
{
    private int[,] grid;

    public int width;
    public int height;
    public GameObject node;
    public GameObject target;
    public List<GameObject> cells = new List<GameObject>();
    public int seed = 0;

    void Start()
    {
        GenerateGrid();
    }

    void Update()
    {
        if (target == null) target = GetNextTarget();
        if (Input.GetButtonDown("Fire1") && target != null)
        {
            var dateTime = DateTime.Now;
            if (seed == 0) seed = (int)dateTime.TimeOfDay.TotalMilliseconds;
            var cell = target.GetComponent<Cell>();
            cell.Collapse();
            target = null;
        }
    }


    void GenerateGrid()
    {
        grid = new int[width, height];
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var pos = new Vector3(-width/2 + x + .5f, -height/2 + y + .5f, 0);
                var currentNode = Instantiate(node, pos, Quaternion.identity);
                var cell = currentNode.GetComponent<Cell>();
                cell.position = GetCellPosition(x, y);
                cell.x = x;
                cell.y = y;
                cells.Add(currentNode);
            }
        }
    }

    GameObject GetNextTarget()
    {
        var tempCells = new List<Cell>();
        foreach (var cell in cells)
        {
            var component = cell.GetComponent<Cell>();
            tempCells.Add(component);
        }
        var sortedCells = tempCells.OrderBy(x => x.entropyCount).Where(x => !x.collapsed).ToList();
        var target = sortedCells.FirstOrDefault();
        if (target == null) return new GameObject();
        return target.gameObject;
    }

    string GetCellPosition(int x, int y)
    {
        
        if (x == 0) return "left";
        if (x == width - 1) return "right";

        return "center";


    }

    void OnDrawGizmos()
    {
        if (grid != null)
        {
            for (var x = 0; x < width; x++)
            { 
                for (var y = 0; y < height; y++)
                { 
                    Gizmos.color = Color.black;
                    Vector3 pos = new Vector3(-width/2 + x + .5f, -height/2 + y + .5f, 0);
                    Gizmos.DrawWireCube(pos, Vector3.one);
                }
            }
        }
    }
}
