using System;
using System.Collections.Generic;
using UnityEngine;


// Требования
// Мир представлен сеткой
// Два слоя сетки - земля и объекты
// Перемещение объектов - мгновенно
// Добавление и удаление объектов и земли

public enum Layer
{
    Background,
    Foreground,
}

public class Level : MonoBehaviour
{
    public float sellSize = 1;
    public float groundHight = 1;
    public GameObject testObject = null;
    public GameObject testCell = null;

    public void Add(Vector2 pos, GameObject obj, Layer layer = Layer.Foreground)
    {
        switch (layer)
        {
            case Layer.Background:
                background.Add(pos, obj);
                break;
            case Layer.Foreground:
                foreground.Add(pos, obj);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
        }

        Move(pos, pos, layer);
    }

    public void Remove(Vector2 pos, Layer layer = Layer.Foreground)
    {
        switch (layer)
        {
            case Layer.Background:
                background.Remove(pos);
                break;
            case Layer.Foreground:
                foreground.Remove(pos);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
        }

        Destroy(Get(pos, layer));
    }

    public void Move(Vector2 from, Vector2 to, Layer layer = Layer.Foreground)
    {
        var gridTo = to * sellSize;
        var obj = Get(from, layer);
        switch (layer)
        {
            case Layer.Background:
                obj.transform.position = new Vector3(gridTo.x, 0, gridTo.y);
                Library.RenameKey(background, from, to);
                break;
            case Layer.Foreground:
                obj.transform.position = new Vector3(gridTo.x, groundHight, gridTo.y);
                Library.RenameKey(foreground, from, to);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
        }
    }

    public GameObject Get(Vector2 pos, Layer layer = Layer.Foreground)
    {
        return layer switch
        {
            Layer.Background => background[pos],
            Layer.Foreground => foreground[pos],
            _ => throw new ArgumentOutOfRangeException(nameof(layer), layer, null)
        };
    }

    private void Start()
    {
        background = new Dictionary<Vector2, GameObject>();
        foreground = new Dictionary<Vector2, GameObject>();

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                Add(new Vector2(i, j), Instantiate(testCell), Layer.Background);
                if (i % 3 == 0 && j % 3 == 0)
                {
                    Add(new Vector2(i, j), Instantiate(testObject), Layer.Foreground);
                }
            }
        }
    }

    private void Update()
    {
    }

    private Dictionary<Vector2, GameObject> background;
    private Dictionary<Vector2, GameObject> foreground;
}