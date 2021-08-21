using System;
using UnityEngine;

public class ActionAttack : MonoBehaviour, IActionSystem
{
    private void Start()
    {
        aiCell = aiObject.GetComponent<Cell>();
        aiStats = aiObject.GetComponent<UnitStats>();
        
        targetCell = targetObject.GetComponent<Cell>();
        targetResources = targetObject.GetComponent<UnitResources>();
        targetStats = targetObject.GetComponent<UnitStats>();
    }

    public bool CanCast(LogState logger)
    {
        var xDistance = Math.Abs(aiCell.x - targetCell.x);
        var yDistance = Math.Abs(aiCell.y - targetCell.y);
        return xDistance <= attackDistance && yDistance <= attackDistance;
    }

    public void DoCast(LogState logger)
    {
        logger.Message("Attacking '" + targetObject.name + "'", MessageType.GameAction);
        var damage = Math.Max(aiStats.Value(UnitStat.Damage) - targetStats.Value(UnitStat.Armor), 0);
        targetResources.AddCurrent(UnitResource.Health, -damage);
    }

    public GameObject aiObject;
    public GameObject targetObject;
    public uint attackDistance = 1;

    private Cell aiCell;
    private UnitStats aiStats;
    private Cell targetCell;
    private UnitResources targetResources;
    private UnitStats targetStats;
}