// Последовательно выполняет ходы всех юнитов на уровне

using System.Collections.Generic;
using UnityEngine;

namespace Steps
{
    public class StepState : StepPipeline
    {
        private new void Start()
        {
            base.Start();

            foreach (var unit in initUnits)
            {
                AddSystem(InitFromObject<StepPipeline>(unit));
            }

            initUnits.Clear();
        }

        private void Update()
        {
            if (prevObject != CurrentStepObject())
            {
                prevObject = CurrentStepObject();
                logger.Message("Step unit: " + prevObject.name, UI.MessageType.Step);
            }
            Step(logger);
        }

        public UI.Logger logger;
        public List<GameObject> initUnits;

        private GameObject prevObject = null;
    }
}