using System;
using UnityEngine;
using UnityEngine.Assertions;

public class TestLevel : MonoBehaviour
{
    //public GameObject levelObject;
    public GameObject testObject = null;
    public GameObject testCell = null;
    public Level level = null;

    void TestLoop(string message, Action<int, int> f)
    {
        Console.WriteLine("Running " + message);
        try
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    f(i, j);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e);
            throw;
        }

        Console.WriteLine("Success");
    }

    void Start()
    {
        TestLoop("Test Level init", (i, j) =>
        {
            level.AddObject(new Vector2Int(i, j), testCell);
            if (i % 3 == 0 && j % 3 == 0)
            {
                level.AddObject(new Vector2Int(i, j), testObject);
            }
        });

        TestLoop("Test Level init check", (i, j) =>
        {
            var objects = level.Query(
                new Vector2Int(i, j), typeof(Transform));
            Assert.AreEqual(objects.Count, 2);
            Assert.IsNotNull(objects[0]);
            Assert.IsNotNull(objects[1]);
        });

        TestLoop("Test Level move", (i, j) =>
        {
            if (i % 3 == 0 && j % 3 == 0)
            {
                var pos = new Vector2Int(i, j);
                var objects = level.Query(pos);
                level.MoveObject(objects[1], pos, pos * 2);
            }
        });

        TestLoop("Test Level move check", (i, j) =>
        {
            if (i % 3 == 0 && j % 3 == 0)
            {
                var pos = new Vector2Int(i, j);
                Assert.IsNotNull(level.Query(pos * 2));
            }
        });
    }
}