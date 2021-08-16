using UnityEngine;

// Генерирует пробный уровень для тестирования игры
public class LevelGenerator : MonoBehaviour
{
    public GameObject testObject;
    public GameObject testCell;
    public GameObject testPlayer;
    public GameObject testCamera;
    public GameObject testEnemy;
    public Location location;
    public StepState stepState;
    public float cameraHeight;

    void Start()
    {
        const int width = 10;
        const int height = 20;

        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                var ground = location.AddObject(new Vector2Int(i, j), testCell);
                ground.AddComponent<GroundTag>();
                if (i % 3 == 0 && j % 3 == 0)
                {
                    var obstacle = location.AddObject(new Vector2Int(i, j), testObject);
                    obstacle.AddComponent<ObstacleTag>();
                }
            }
        }

        var player = location.AddObject(new Vector2Int(5, 5), testPlayer);
        var controller = player.AddComponent<PlayerController>();
        controller.playerCamera = testCamera;
        controller.cameraHeight = cameraHeight;
        stepState.AddStepObject(player);
        
        var enemy = location.AddObject(new Vector2Int(0, 1), testEnemy);
        var ai = enemy.AddComponent<SimpleAI>();
        ai.targetPlayer = player;
        ai.visionDistance = 4;
        stepState.AddStepObject(enemy);

        var objects = location.QueryArea(
            new Vector2Int(0, 0),
            new Vector2Int(width, height),
            typeof(Cell));

        var someObject = objects[0];
        var offset = new Vector2Int(10, -5);
        var cell = someObject.GetComponent<Cell>();
        var newPosition = cell.ToVec() + offset;
        location.MoveObject(someObject, cell, newPosition);
    }
}