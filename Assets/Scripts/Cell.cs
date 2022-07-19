using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

public class Cell : MonoBehaviour
{
    public Tile value;
    public List<Tile> entropy = new List<Tile>();
    public int entropyCount { get { return entropy.Count; } }
    public bool collapsed = false;

    public string position;
    [SerializeField] public int x;
    [SerializeField] public int y;

    [SerializeField] private List<Neighbor> neighbors = new List<Neighbor>();
    private bool hasLeftNeighbor => neighbors.Any(n => n.position == "left");
    private bool hasRightNeighbor => neighbors.Any(n => n.position == "right");
    private bool hasTopNeighbor => neighbors.Any(n => n.position == "top");
    private bool hasBottomNeighbor => neighbors.Any(n => n.position == "bottom");
    
    private SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();

        if (value.tileName.Equals("Empty"))
        {
            foreach (var entry in value.domain.top.ToList()) 
            {
                entropy.Add(entry.tile);
            }   
        }
    }

    // Update is called once per frame
    void Update()
    {
        renderer.sprite = value.sprite;
        if (!collapsed) 
        {
           CalculateEntropy();
        }
    }

    void CalculateEntropy()
    {
        this.entropy = this.entropy.EvaluateEntropy(this.neighbors);
    }


    public void Collapse()
    {
        // Random.InitState(seed);
        var index = (int)Random.Range(0.1f, (float)entropy.Count);
        var chosenState = entropy[index];
        value = chosenState;
        collapsed = true;
        entropy = new List<Tile>();
        // StartCoroutine(Propagate());
    }

    public void Propagate()
    {
        foreach (var neighbor in neighbors)
        {
            var neighborCell = neighbor.node.GetComponent<Cell>();
            neighborCell.CalculateEntropy();
        }
    }

    void OnMouseDown()
    {
       
    }

    void FixedUpdate()
    { 
        if (neighbors.Count == 0)
        {
            var potentialNeighbors = new List<string> {"left", "right", "top", "bottom"};
            foreach (var label in potentialNeighbors)
            {
                var rayDetails = new RayDetails(label, transform);
                GetNeighbors(rayDetails); 
            }
        }
     
    }


    void GetNeighbors(RayDetails rayDetails)
    {
        var alreadyDetected = neighbors.Any(n => n.position == rayDetails.label);
        var ray = Physics2D.Raycast(rayDetails.origin, rayDetails.direction);
        if (ray.collider != null)
        {
            neighbors.Add(new Neighbor(ray.transform.gameObject, rayDetails.label));
        }
    }
}

public struct RayDetails {
    private Vector2 _origin; 
    private Vector2 _direction;
    private string _label;
    
    public Vector2 origin {get { return _origin; } }
    public Vector2 direction { get { return _direction; } }
    public string label { get { return _label; } }

    public RayDetails(string dir, Transform pos) {
        _label = dir;
        var x = pos.position.x;
        var y = pos.position.y;
        switch (dir)
        {
            case "left": 
                _origin = new Vector2(x - 1, y);
                _direction = Vector2.left;
                break;
            case "right": 
                _origin = new Vector2(x + 1, y);
                _direction = -Vector2.left;
                break;
            case "top":
                _origin = new Vector2(x, y + 1);
                _direction = Vector2.up;
                break;
            case "bottom":
                _origin = new Vector2(x, y - 1);
                _direction = -Vector2.up;
                break;
            default:
                var defVector = new Vector2(x, y);
                _origin = defVector;
                _direction = defVector;
                break;
        }
    }
}

[System.Serializable]
public struct Neighbor
{ 
    public GameObject node;
    public string position;

    public Neighbor(GameObject _node, string _pos)
    {
        this.node = _node;
        this.position = _pos;
    }
}
