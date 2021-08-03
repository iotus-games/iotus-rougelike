using System;
using UnityEngine;
using UnityEngine.Assertions;

public class TestLevel : MonoBehaviour
{
    //public GameObject levelObject;
    public GameObject testObject = null;
    public GameObject testCell = null;
    public Level level = null;

    void Start()
    {
        Debug.Log("Running TestLevel ...");
        try
        {
            const int width = 10;
            const int height = 20;
            var cellCount = 0;
            var cubesCount = 0;

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    cellCount += 1;
                    level.AddObject(new Vector2Int(i, j), testCell);
                    if (i % 3 == 0 && j % 3 == 0)
                    {
                        cubesCount += 1;
                        level.AddObject(new Vector2Int(i, j), testObject);
                    }
                }
            }

            var objects = level.QueryArea(
                new Vector2Int(0, 0),
                new Vector2Int(width, height),
                typeof(Cell));

            Assert.AreEqual(objects.Count, cellCount + cubesCount);

            var someObject = objects[0];
            var offset = new Vector2Int(10, -5);
            var newPosition = Level.ObjectPosition(someObject) + offset;
            level.MoveObject(someObject, newPosition);
            Assert.AreEqual(Level.ObjectPosition(someObject), newPosition);
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e);
            throw;
        }

        Debug.Log("Success");
    }
}