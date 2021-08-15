using System;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelGenerator : MonoBehaviour
{
    //public GameObject levelObject;
    public GameObject testObject;
    public GameObject testCell;
    public GameObject testPlayer;
    public GameObject testCamera;
    public Location location;
    public StepState stepState;
    public float cameraHeight;

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
                    var ground = location.AddObject(new Vector2Int(i, j), testCell);
                    ground.AddComponent<GroundTag>();
                    if (i % 3 == 0 && j % 3 == 0)
                    {
                        cubesCount += 1;
                        var obstacle = location.AddObject(new Vector2Int(i, j), testObject);
                        obstacle.AddComponent<ObstacleTag>();
                    }
                }
            }

            var player = location.AddObject(new Vector2Int(5, 5), testPlayer);
            var controller = player.AddComponent<PlayerController>();
            controller.location = location;
            controller.player = player;
            controller.playerCamera = testCamera;
            controller.cameraHeight = cameraHeight;
            
            var pipeline = player.AddComponent<StepPipeline>();
            pipeline.StepSystems.Add(controller);
            stepState.stepObjects.Add(pipeline);

            var objects = location.QueryArea(
                new Vector2Int(0, 0),
                new Vector2Int(width, height),
                typeof(Cell));

            Assert.AreEqual(objects.Count, cellCount + cubesCount + 1);

            var someObject = objects[0];
            var offset = new Vector2Int(10, -5);
            var cell = someObject.GetComponent<Cell>();
            var newPosition = cell.ToVec() + offset;
            location.MoveObject(someObject, cell,  newPosition);
            Assert.AreEqual(cell.ToVec(), newPosition);
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e);
            throw;
        }

        Debug.Log("Success");
    }
}