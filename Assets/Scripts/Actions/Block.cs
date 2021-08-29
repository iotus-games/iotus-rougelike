using System;
using Steps;
using UnityEngine;

namespace Actions
{
    public class Block : MonoBehaviour, IActionSystem
    {
        public float damage;
        private UnitResources resources;
        private UnitStats stats;

        public void Prepare(GameObject caller)
        {
            resources = caller.GetComponent<UnitResources>();
            stats = caller.GetComponent<UnitStats>();
        }

        public StepAction Step(UI.Logger logger)
        {
            var totalDamage = Math.Max(damage - stats.Value(UnitStat.Armor), 0);
            resources.AddCurrent(UnitResource.Health, -totalDamage);
            return StepAction.Continue;
        }

        public bool CanCast(UI.Logger logger)
        {
            return true;
        }
    }
}