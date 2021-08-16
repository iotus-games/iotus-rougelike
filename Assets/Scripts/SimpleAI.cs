using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour, IStepSystem
{
    public GameObject targetPlayer;
    private Cell targetCell;
    private Cell aiCell;

    void Start()
    {
        targetCell = targetPlayer.GetComponent<Cell>();
        aiCell = GetComponent<Cell>();
    }

    public StepAction Step(LogState logger, Location location)
    {
        var to = BreathFirstSearchStep(location, aiCell.ToVec(), targetCell.ToVec());
        if (to != targetCell.ToVec())
        {
            location.MoveObject(gameObject, aiCell, to);
        }

        return StepAction.Continue;
    }

    private static Dictionary<Vector2Int, uint> BreathFirstCountSteps(
        Location location, Vector2Int origin, Vector2Int target)
    {
        var unfinished = new List<Vector2Int>();
        var cellScores = new Dictionary<Vector2Int, uint> {{origin, 0}};

        // нумерация количества ходов в пути до каждой клетки
        while (true)
        {
            var neighbours = GetNeighbours(origin);

            foreach (var selectedPos in neighbours)
            {
                if (!location.Has(selectedPos, typeof(GroundTag)) ||
                    location.Has(selectedPos, typeof(ObstacleTag)) ||
                    selectedPos == origin)
                {
                    continue;
                }

                if (!cellScores.ContainsKey(selectedPos))
                {
                    cellScores.Add(selectedPos, cellScores[origin] + 1);
                    // останавливаемся, если нашли цель
                    if (selectedPos == target)
                    {
                        return cellScores;
                    }

                    // добавляем клетку для последующего исследования
                    unfinished.Add(selectedPos);
                }
            }

            // начинаем исследовать новую клетку
            if (unfinished.Count != 0)
            {
                origin = unfinished[0];
                unfinished.RemoveAt(0);
            }
            else
            {
                return cellScores;
            }
        }
    }

    // В случае, если нельзя добраться до цели напрямую, находим ближающую возможную до достижения клетку
    private static Vector2Int FindNearestReachable(
        Dictionary<Vector2Int, uint> scores, Vector2Int origin, Vector2Int target)
    {
        if (scores.ContainsKey(target))
        {
            return target;
        }

        var nearest = origin;
        foreach (var selected in scores.Keys)
        {
            if (Vector2Int.Distance(selected, target) < Vector2Int.Distance(nearest, target))
            {
                nearest = selected;
            }
        }

        return nearest;
    }

    private static Vector2Int BreathFirstSearchStep(Location location, Vector2Int origin, Vector2Int target)
    {
        var scores = BreathFirstCountSteps(location, origin, target);
        target = FindNearestReachable(scores, origin, target);

        if (origin == target)
        {
            return origin;
        }

        while (true)
        {
            var neighbours = GetNeighbours(target);
            var score = scores[target];

            foreach (var neighbour in neighbours)
            {
                if (neighbour == origin)
                {
                    return target;
                }

                if (scores.ContainsKey(neighbour))
                {
                    var neighbourScore = scores[neighbour];
                    if (neighbourScore == score - 1)
                    {
                        target = neighbour;
                        break;
                    }
                }
            }
        }
    }

    private static List<Vector2Int> GetNeighbours(Vector2Int target)
    {
        var neighbours = new List<Vector2Int>
        {
            target + Vector2Int.left + Vector2Int.up,
            target + Vector2Int.right + Vector2Int.up,
            target + Vector2Int.up,
            target + Vector2Int.left + Vector2Int.down,
            target + Vector2Int.right + Vector2Int.down,
            target + Vector2Int.down,
            target + Vector2Int.left,
            target + Vector2Int.right,
        };
        return neighbours;
    }
}