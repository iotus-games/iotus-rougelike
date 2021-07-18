using System;
using System.Collections.Generic;
using UnityEngine;


// Требования
// Мир представлен сеткой
// Три слоя сетки - земля, объекты и окружение
// Перемещение объектов - мгновенно
// Добавление и удаление объектов и земли

class Cell
{
    public Cell(GameObject ground, GameObject o)
    {
        Ground = ground;
        Object = o;
    }

    public GameObject Ground;
    public GameObject Object;
}

public class Level : MonoBehaviour
{
    public float sellSize = 1;

    public void ToGridCoords(GameObject obj, Vector2 gridCoords)
    {
        var gridTo = gridCoords * sellSize;
        obj.transform.position = new Vector3(gridTo.x, obj.transform.position.y, gridTo.y);
    }

    public void AddGround(Vector2 pos, GameObject obj)
    {
        if (cells.TryGetValue(pos, out var cell))
        {
            Destroy(cell.Ground);
            cell.Ground = obj;
        }
        else
        {
            cells.Add(pos, new Cell(obj, null));
        }
        
        ToGridCoords(obj, pos);
    }

    public void AddObject(Vector2 pos, GameObject obj)
    {
        if (!cells.TryGetValue(pos, out var cell))
        {
            throw new Exception("Object can not exist without ground!");
        }

        cell.Object = obj;
        ToGridCoords(obj, pos);
    }

    public void AddAmbient(Vector2 pos, GameObject obj)
    {
        if (ambients.TryGetValue(pos, out var ambient))
        {
            Destroy(ambient);
        }
        ambients.Add(pos, obj);
        ToGridCoords(obj, pos);
    }

    public void RemoveObject(Vector2 pos)
    {
        var cell = cells[pos];
        if (layer == Layer.Ground)
        {
            if (cell.Object != null)
            {
                Destroy(cell.Object);
            }

            cells.Remove(pos);
        }
        else if (layer == Layer.Object)
        {
            Destroy(cell.Object);
        }
        else if (layer == Layer.Ambient)
        {
            Destroy(ambients[pos]);
            ambients.Remove(pos);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
        }

        Destroy(Get(pos, layer));
    }

    public void Move(Vector2 from, Vector2 to, Layer layer = Layer.Object)
    {
        var gridTo = to * sellSize;
        var obj = Get(from, layer);
        switch (layer)
        {
            case Layer.Ground:
                obj.transform.position = new Vector3(gridTo.x, 0, gridTo.y);
                Library.RenameKey(grounds, from, to);
                break;
            case Layer.Object:
                obj.transform.position = new Vector3(gridTo.x, groundHight, gridTo.y);
                Library.RenameKey(objects, from, to);
                break;
            case Layer.Ambient:
                obj.transform.position = new Vector3(gridTo.x, groundHight, gridTo.y);
                Library.RenameKey(ambients, from, to);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
        }
    }

    public GameObject Get(Vector2 pos, Layer layer = Layer.Object)
    {
        return layer switch
        {
            Layer.Ground => cells.TryGetValue(pos, out var cellValue) ? cellValue.Ground : null,
            Layer.Object => cells.TryGetValue(pos, out var cellValue) ? cellValue.Object : null,
            Layer.Ambient => ambients.TryGetValue(pos, out var cellValue) ? cellValue : null,
            _ => throw new ArgumentOutOfRangeException(nameof(layer), layer, null)
        };
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    private Dictionary<Vector2, Cell> cells = new Dictionary<Vector2, Cell>();
    private Dictionary<Vector2, GameObject> ambients = new Dictionary<Vector2, GameObject>();
}