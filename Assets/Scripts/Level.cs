using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Положительное направление по X: право
// Положительное направление по Y: низ
public class Level : MonoBehaviour
{
    public float sellSize = 1;

    public void ToWorldCoords(Vector2Int oldPos, Vector2Int newPos, GameObject obj)
    {
        var gridFrom = (Vector2) oldPos * sellSize;
        var gridTo = (Vector2) newPos * sellSize;
        var position = obj.transform.position;
        obj.transform.position = new Vector3(
            gridTo.x + position.x - gridFrom.x, position.y, gridTo.y + position.z - gridFrom.y);
    }

    public void AddObject(Vector2Int pos, GameObject prefab)
    {
        var obj = Instantiate(prefab);
        ToWorldCoords(new Vector2Int(), pos, obj);
        var cell = obj.AddComponent<Cell>();
        cell.x = pos.x;
        cell.y = pos.y;
        Library.GetOrCreate(cells, pos).Add(obj);
    }

    public void RemoveObject(GameObject obj)
    {
        var cell = obj.GetComponent<Cell>();
        if (cell == null)
        {
            throw new Exception("Game object" + obj.name + " is not from level grid");
        }

        var pos = cell.ToVec();
        var list = cells[pos];
        list.Remove(obj);
        if (list.Count == 0)
        {
            cells.Remove(pos);
        }
    }

    public void MoveObject(GameObject obj, Vector2Int from, Vector2Int to)
    {
        if (!cells.TryGetValue(from, out var fromObjects))
        {
            throw new Exception("Cell " + from + " doesn't exist");
        }

        var toObjects = Library.GetOrCreate(cells, to);

        if (!fromObjects.Remove(obj))
        {
            throw new Exception("Object in cell " + from + " doesn't exist");
        }

        ToWorldCoords(from, to, obj);
        toObjects.Add(obj);
    }

    // Возвращает объекты в клетке, содержащие все перечисленные компоненты
    public List<GameObject> Query(Vector2Int pos, List<Type> components)
    {
        var result = new List<GameObject>();
        if (cells.TryGetValue(pos, out var objects))
        {
            foreach (var obj in objects)
            {
                var hasAll = true;
                foreach (var component in components)
                {
                    if (obj.GetComponent(component) == null)
                    {
                        hasAll = false;
                        break;
                    }
                }

                if (hasAll)
                {
                    result.Add(obj);
                }
            }
        }
        else
        {
            throw new Exception("Cell " + pos + " doesn't exist");
        }

        return result;
    }

    // Возвращает объекты в клетке, содержащий компонент 
    public List<GameObject> Query(Vector2Int pos, Type component)
    {
        var a = new List<Type> {component};
        return Query(pos, a);
    }

    // Возвращает все объекты в клетке 
    public List<GameObject> Query(Vector2Int pos)
    {
        var a = new List<Type>();
        return Query(pos, a);
    }

    // Найти объект на уровне по имени
    public GameObject Query(string objName)
    {
        foreach (var list in cells.Values)
        {
            foreach (var obj in list)
            {
                if (obj.name == objName)
                {
                    return obj;
                }
            }
        }

        return null;
    }

    public List<GameObject> QueryArea(Vector2Int leftTop, Vector2Int rightBottom, List<Type> components)
    {
        var result = new List<GameObject>();
        Assert.IsTrue(leftTop.x < rightBottom.x && leftTop.y < rightBottom.y);

        for (var i = leftTop.y; i < rightBottom.y; i++)
        {
            for (var j = leftTop.x; j < rightBottom.x; j++)
            {
                result.AddRange(Query(new Vector2Int(j, i), components));
            }
        }

        return result;
    }

    public List<GameObject> QueryArea(Vector2Int leftTop, Vector2Int rightBot, Type component)
    {
        var a = new List<Type> {component};
        return QueryArea(leftTop, rightBot, a);
    }

    public List<GameObject> QueryArea(Vector2Int leftTop, Vector2Int rightBot)
    {
        var a = new List<Type>();
        return QueryArea(leftTop, rightBot, a);
    }

    private Dictionary<Vector2, List<GameObject>> cells = new Dictionary<Vector2, List<GameObject>>();
}