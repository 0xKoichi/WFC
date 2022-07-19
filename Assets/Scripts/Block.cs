using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using System.Linq;
using System.IO;
using System;

public class Block : MonoBehaviour
{
    public Sprite sprite;
    public int w;
    public int h;
    public List<Color[]> pixelSections;
    public List<GameObject> nodes;
    public SpriteRenderer testingSprite;
    public int maxNodes = 4;


    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        w = (int)sprite.rect.width;
        h = (int)sprite.rect.height;
        var pixelCount = w * h;
        var offset = pixelCount / 4;

        GetAllNodes();
        
        // Color[] pixels = sprite.texture.GetPixels(0);
        // pixelSections = SplitPixelArray(pixels, offset);
        // if (pixelSections.Count > 0) GetNodes();
        
    }

    /// <summary>
    /// If the log base 2 of n is an integer, then n is a power of 2
    /// </summary>
    /// <param name="n">The number of desired nodes.</param>
    /// <returns>
    /// True if n is a power of 2, otherwise false.
    /// </returns>
    bool ValidateMaxNodes(int n)
    {
        if (n == 0) return false;
        return (int)(Math.Ceiling((Math.Log(n) /
                               Math.Log(2)))) ==
           (int)(Math.Floor(((Math.Log(n) /
                              Math.Log(2)))));
    }

    /// <summary>
    /// Ensures that the current block can be divided by the user
    /// defined minimum node count. Assumes that the block is a
    /// square with even width and height and is power of two,
    /// i.e., 8x8, 16x16, 32x32, 64x64, 128x128...
    /// </summary>
    void GetAllNodes()
    {

        
        // int? potentialMinNode = GetLowestMinNode();
        // bool blockValid = (potentialMinNode != null);
        // if (!blockValid) Debug.LogError($"Block size not valid!\n(Block Width: {w}, Block Height: {h}");

        
        // bool minNodeValid = (minNodes >= potentialMinNode && minNodes % potentialMinNode == 0);

        // if (minNodeValid)
        // {
            
        //     var minPixelDensityY = (int)(Math.Pow(h, 2) / minNode);

        //     var width = maxSlicesWidth * 2;
        //     var height = maxSlicesHeight * 2;
            
        //     for (var i = 0; i < minNodes; i++)
        //     {
        //         var x = 0;
        //         var y = 0;
        //     }
        // 

        if(ValidateMaxNodes(maxNodes))
        {
            var nHeight = (h / (maxNodes / 2));
            var nWidth = (w / (maxNodes / 2));
            var source = sprite.texture;
            var mY = h;
            var mX = w;



            // Get other nodes.
            for (var i = 0; i < mX; i+=nWidth)
            {
                for (var j = 0; j < mY; j+=nHeight)
                { 
                    // var position = new Rect(i, j, nWidth, nHeight);
                    var texture = GetNode(i, j, nWidth, nHeight);
                    texture.name = $"{gameObject.name} ({i}, {j})";
                    // GameObject go = new GameObject($"{i} {j}", typeof(SpriteRenderer));
                    // var sprite = Sprite.Create(texture, position);
                    // Instantiate(go);
                    
                }
            }
        }
    }

    Texture2D GetNode(int x, int y, int w, int h)
    {
        var mipLevel = 0;
        var destMipLevel = 0;

        var nodePixels = sprite.texture.GetPixels(x, y, w, h, mipLevel);
        var node = new Texture2D(w, h);
        node.SetPixels(nodePixels, destMipLevel);
        node.filterMode = FilterMode.Point;
        node.wrapMode = TextureWrapMode.Clamp;
        node.Apply();
        SaveTexture(node, $"{gameObject.name} ({x}, {y})");
        return node;
    }

    void SaveTexture(Texture2D data, string fileName)
    {
        byte[] bytes = ImageConversion.EncodeArrayToPNG(data.GetRawTextureData(), data.graphicsFormat, (uint)data.width, (uint)data.height);
        File.WriteAllBytes($"{Application.dataPath}/{fileName}.png", bytes);
    }

    bool isDivisible (int p)
    {
        var sqrt = Mathf.Sqrt(p);
        return (sqrt % 4 == 0);
    }

    // List<Color[]> SplitPixelArray (Color[] pixels, int offset)
    // {
    //     List<Color[]> result = new List<Color[]>();
    //     for (var i = 0; i < pixels.Length; i+=offset)
    //     {
    //         Color[] currentPixels = new Color[offset];
    //         Array.Copy(pixels, i, currentPixels, 0, offset);
    //         result.Add(currentPixels);
    //     }
    //     Debug.Log(result.Count());
    //     return result;
    // }

    // void GetNodes()
    // {
    //    foreach (var section in pixelSections)
    //    {
    //         var node = new Texture2D(w / 4, h / 4);
    //         node.SetPixels(section);
    //         node.Apply();
    //         nodes.Add(node);
    //    }
    // }



    // Update is called once per frame
    void Update()
    {
        // if (nodes.Count() > 0)
        // {
        //     var nodeSprite = Sprite.Create(nodes[0], new Rect(0.0f, 0.0f, nodes[1].width, nodes[1].height), new Vector2(0f, 0f), 8.0f);
        //     testingSprite.sprite = nodeSprite;
        // }
    }
}

[System.Serializable]
public class Coords {
    private int _x;
    private int _y;

}
