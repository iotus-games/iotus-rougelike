using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Положительное направление по X: право
// Положительное направление по Y: низ
public class Location : MonoBehaviour
{
    private void Start()
    {
        if (initObjects != null)
        {
            foreach (var obj in initObjects)
            {
                AddObject(obj);
            }
        }
    }


    // Добавляет объект на сетку уровня.
    // Компонент Transform определяет положение объекта относительно центра клетки
    public void AddObject(GameObject obj)
    {
        var pos = obj.GetComponent<Cell>().ToVec();
        Library.GetOrCreate(cells, pos).Add(obj);
        ToWorldCoords(new Vector2Int(), pos, obj);
    }

    public GameObject InitObject(Vector2Int pos, GameObject prefab)
    {
        var obj = Instantiate(prefab);
        var cell = obj.AddComponent<Cell>();
        cell.x = pos.x;
        cell.y = pos.y;
        Library.GetOrCreate(cells, pos).Add(obj);
        ToWorldCoords(new Vector2Int(), pos, obj);
        return obj;
    }

    // Удаляет объект, находящийся на сетке уровня
    public void RemoveObject(GameObject obj)
    {
        var pos = obj.GetComponent<Cell>().ToVec();

        var list = cells[pos];
        list.Remove(obj);
        Destroy(obj);
        if (list.Count == 0)
        {
            cells.Remove(pos);
        }
    }

    // Мгновнно перемещает с клетки на клетку
    public void MoveObject(GameObject obj, Cell cell, Vector2Int to)
    {
        var from = cell.ToVec();

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
        cell.x = to.x;
        cell.y = to.y;
        toObjects.Add(obj);
    }

    // Проверяет, есть ли объект со всеми нужными компоентами в клетке
    public bool Has(Vector2Int pos, List<Type> includeComponents, List<Type> excludeComponents)
    {
        if (cells.TryGetValue(pos, out var objects))
        {
            foreach (var obj in objects)
            {
                var hasAll = true;
                foreach (var component in includeComponents)
                {
                    if (obj.GetComponent(component) == null)
                    {
                        hasAll = false;
                        break;
                    }
                }

                if (!hasAll)
                {
                    continue;
                }

                foreach (var component in excludeComponents)
                {
                    if (obj.GetComponent(component) != null)
                    {
                        hasAll = false;
                        break;
                    }
                }


                if (hasAll)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool Has(Vector2Int pos, List<Type> includeComponents)
    {
        return Has(pos, includeComponents, new List<Type>());
    }

    // Проверяет, есть ли объект с нужным компоентом в клетке
    public bool Has(Vector2Int pos, Type include, Type exclude)
    {
        var i = new List<Type> {include};
        var e = new List<Type> {exclude};
        return Has(pos, i, e);
    }

    public bool Has(Vector2Int pos, Type include)
    {
        var i = new List<Type> {include};
        var e = new List<Type>();
        return Has(pos, i, e);
    }

    // Проверяет, есть ли объекты в клетке
    public bool Has(Vector2Int pos)
    {
        return cells.ContainsKey(pos);
    }

    // Возвращает объекты в клетке, содержащие все перечисленные компоненты
    public List<GameObject> Query(Vector2Int pos, List<Type> includeComponents, List<Type> excludeComponents)
    {
        var result = new List<GameObject>();
        if (cells.TryGetValue(pos, out var objects))
        {
            foreach (var obj in objects)
            {
                var hasAll = true;
                foreach (var component in includeComponents)
                {
                    if (obj.GetComponent(component) == null)
                    {
                        hasAll = false;
                        break;
                    }
                }

                if (!hasAll)
                {
                    continue;
                }

                foreach (var component in excludeComponents)
                {
                    if (obj.GetComponent(component) != null)
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
    public List<GameObject> Query(Vector2Int pos, Type include, Type exclude)
    {
        var i = new List<Type> {include};
        var e = new List<Type> {exclude};
        return Query(pos, i, e);
    }

    public List<GameObject> Query(Vector2Int pos, Type include)
    {
        var i = new List<Type> {include};
        var e = new List<Type>();
        return Query(pos, i, e);
    }

    // Возвращает все объекты в клетке 
    public List<GameObject> Query(Vector2Int pos)
    {
        var i = new List<Type>();
        return Query(pos, i, i);
    }

    // Возвращает объекты на прямоугольной площади, содержащие все перечисленные компоненты
    public List<GameObject> QueryArea(Vector2Int leftBottom, Vector2Int rightTop, List<Type> includeComponents,
        List<Type> excludeComponents)
    {
        var result = new List<GameObject>();
        Assert.IsTrue(leftBottom.x < rightTop.x && leftBottom.y < rightTop.y);

        for (var i = leftBottom.y; i <= rightTop.y; i++)
        {
            for (var j = leftBottom.x; j <= rightTop.x; j++)
            {
                var pos = new Vector2Int(j, i);
                if (Has(pos))
                {
                    result.AddRange(Query(pos, includeComponents, excludeComponents));
                }
            }
        }

        return result;
    }

    // Возвращает объекты на прямоугольной площади, содержащие указанный компонент
    public List<GameObject> QueryArea(Vector2Int leftBot, Vector2Int rightTop, Type include, Type exclude)
    {
        var i = new List<Type> {include};
        var e = new List<Type> {exclude};
        return QueryArea(leftBot, rightTop, i, e);
    }

    public List<GameObject> QueryArea(Vector2Int leftBot, Vector2Int rightTop, Type include)
    {
        var i = new List<Type> {include};
        var e = new List<Type>();
        return QueryArea(leftBot, rightTop, i, e);
    }

    // Возвращает все объекты на прямоугольной площади
    public List<GameObject> QueryArea(Vector2Int leftBot, Vector2Int rightTop)
    {
        var a = new List<Type>();
        return QueryArea(leftBot, rightTop, a, a);
    }

    public bool HasArea(Vector2Int leftBottom, Vector2Int rightTop, List<Type> includeComponents,
        List<Type> excludeComponents)
    {
        Assert.IsTrue(leftBottom.x < rightTop.x && leftBottom.y < rightTop.y);

        for (var i = leftBottom.y; i <= rightTop.y; i++)
        {
            for (var j = leftBottom.x; j <= rightTop.x; j++)
            {
                var pos = new Vector2Int(j, i);
                if (Has(pos, includeComponents, excludeComponents))
                {
                    return true;
                }
            }
        }

        return false;
    }

    // Возвращает объекты на прямоугольной площади, содержащие указанный компонент
    public bool HasArea(Vector2Int leftBottom, Vector2Int rightTop, Type include, Type exclude)
    {
        var i = new List<Type> {include};
        var e = new List<Type> {exclude};
        return HasArea(leftBottom, rightTop, i, e);
    }

    public bool HasArea(Vector2Int leftBottom, Vector2Int rightTop, Type include)
    {
        var i = new List<Type> {include};
        var e = new List<Type>();
        return HasArea(leftBottom, rightTop, i, e);
    }

    // Возвращает все объекты на прямоугольной площади
    public bool HasArea(Vector2Int leftBottom, Vector2Int rightTop)
    {
        var a = new List<Type>();
        return HasArea(leftBottom, rightTop, a, a);
    }

    private void ToWorldCoords(Vector2Int oldPos, Vector2Int newPos, GameObject obj)
    {
        var gridFrom = (Vector2) oldPos * sellSize;
        var gridTo = (Vector2) newPos * sellSize;
        var position = obj.transform.position;
        obj.transform.position = new Vector3(
            gridTo.x + position.x - gridFrom.x, position.y, gridTo.y + position.z - gridFrom.y);
    }

    private Dictionary<Vector2, List<GameObject>> cells = new Dictionary<Vector2, List<GameObject>>();

    public float sellSize = 1;
    public List<GameObject> initObjects;
}