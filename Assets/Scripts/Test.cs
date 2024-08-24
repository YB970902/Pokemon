using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("MapData/MapData");
        Debug.Log(textAsset.text);
        string[] lines = textAsset.text.Split("\n");
        var x = lines[0].Length;
        var y = lines.Length;
        Debug.Log($"x : {x}, y : {y}");
        Sprite[] sprites = Resources.LoadAll<Sprite>("Tile/Tile");
        var spriteRenderer1 = new GameObject().AddComponent<SpriteRenderer>();
        spriteRenderer1.sprite = sprites[0];
        spriteRenderer1.transform.position = Vector2.zero;
        var spriteRenderer2 = new GameObject().AddComponent<SpriteRenderer>();
        spriteRenderer2.sprite = sprites[1];
        spriteRenderer2.transform.position = Vector2.right;
        var spriteRenderer3 = new GameObject().AddComponent<SpriteRenderer>();
        spriteRenderer3.sprite = sprites[2];
        spriteRenderer3.transform.position = Vector2.right * 2;
    }
}
