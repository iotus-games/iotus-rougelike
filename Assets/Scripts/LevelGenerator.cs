using UnityEngine;

// Генерирует пробный уровень для тестирования игры
public class LevelGenerator : MonoBehaviour
{
    public GameObject testObstacle;
    public GameObject testCell;
    public Location location;

    void Start()
    {
        const int width = 10;
        const int height = 20;

        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                var ground = location.InitObject(new Vector2Int(i, j), testCell);
                ground.AddComponent<GroundTag>();
                if (i % 3 == 0 && j % 3 == 0)
                {
                    var obstacle = location.InitObject(new Vector2Int(i, j), testObstacle);
                    obstacle.AddComponent<ObstacleTag>();
                }
            }
        }
    }
}