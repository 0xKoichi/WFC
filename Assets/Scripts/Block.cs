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


    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        w = (int)sprite.rect.width;
        h = (int)sprite.rect.height;
        GetAllNodes();
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
    /// square is power of two, e.g., 8x8, 8x16, 16x16, 16x32, 32x32 etc...
    /// </summary>
    void GetAllNodes()
    {
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
                    var texture = GetNode(i, j, nWidth, nHeight);
                    texture.name = $"{gameObject.name} ({i}, {j})";
                }
            }
        }
    }

    /// <summary>
    /// Get the pixels from the sprite's texture at the given coordinates, create a new texture from
    /// those pixels, and save the new texture to disk
    /// </summary>
    /// <param name="x">The x coordinate of the top left corner of the node.</param>
    /// <param name="y">The y coordinate of the top-left corner of the node.</param>
    /// <param name="w">width of the node</param>
    /// <param name="h">height of the node</param>
    /// <returns>
    /// A Texture2D object.
    /// </returns>
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
