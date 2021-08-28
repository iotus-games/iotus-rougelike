using System;
using UnityEngine;

namespace Actions
{
    public class Attack : MonoBehaviour, IActionSystem
    {
        public GameObject targetObject;
        public uint attackDistance = 1;

        private Cell aiCell;
        private UnitStats aiStats;
        private Cell targetCell;
        private UnitResources targetResources;
        private UnitStats targetStats;
        
        public void Prepare(GameObject caller)
        {
            aiCell = caller.GetComponent<Cell>();
            aiStats = caller.GetComponent<UnitStats>();

            targetCell = targetObject.GetComponent<Cell>();
            targetResources = targetObject.GetComponent<UnitResources>();
            targetStats = targetObject.GetComponent<UnitStats>();
        }

        public bool CanCast(UI.Logger logger)
        {
            var xDistance = Math.Abs(aiCell.x - targetCell.x);
            var yDistance = Math.Abs(aiCell.y - targetCell.y);
            return xDistance <= attackDistance && yDistance <= attackDistance;
        }

        public void DoCast(UI.Logger logger)
        {
            logger.Message("Attacking '" + targetObject.name + "'", UI.MessageType.GameAction);
            var damage = Math.Max(aiStats.Value(UnitStat.Damage) - targetStats.Value(UnitStat.Armor), 0);
            targetResources.AddCurrent(UnitResource.Health, -damage);
        }

    }
}