using System;
using UnityEngine;
using Steps;

namespace Actions
{
    public class Attack : MonoBehaviour, IActionSystem
    {
        public GameObject targetObject;
        public uint attackDistance = 1;

        private Cell cell;
        private UnitStats stats;
        private Cell targetCell;
        private UnitActions targetActions;
        
        public void Prepare(GameObject caller)
        {
            cell = caller.GetComponent<Cell>();
            stats = caller.GetComponent<UnitStats>();
            targetCell = targetObject.GetComponent<Cell>();
            targetActions = targetObject.GetComponent<UnitActions>();
        }

        public bool CanCast(UI.Logger logger)
        {
            var xDistance = Math.Abs(cell.x - targetCell.x);
            var yDistance = Math.Abs(cell.y - targetCell.y);
            return xDistance <= attackDistance && yDistance <= attackDistance;
        }

        public StepAction Step(UI.Logger logger)
        {
            var block = targetActions.GetActionComponent<Block>("Block");
            block.damage = stats.Value(UnitStat.Damage);
            targetActions.Cast("Block", cell.gameObject);
            return StepAction.Continue;
        }

    }
}