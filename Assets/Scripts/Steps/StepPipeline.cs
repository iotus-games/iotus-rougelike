using UnityEngine;

namespace Steps
{
// Позволяет выполнять одному юниту несколько последовательных действий в рамках одного хода
    public class StepPipeline : Pipeline, IStepSystem
    {
        public StepPipeline()
        {
            componentBaseType = typeof(IStepSystem);
        }

        public StepAction Step(UI.Logger logger)
        {
            if (systems.Count == 0)
            {
                return StepAction.Continue;
            }

            var result = ((IStepSystem)systems[currentSystem]).Step(logger);

            if (result == StepAction.Continue)
            {
                currentSystem += 1;
            }
            else if (result == StepAction.Stop)
            {
                RemoveSystem(systems[currentSystem]);
            }

            if (currentSystem == systems.Count)
            {
                currentSystem = 0;
                return StepAction.Continue;
            }
            else
            {
                return StepAction.Wait;
            }
        }

        public void AddSystemAfterCurrent(Component c)
        {
            AddSystem(c, currentSystem + 1);
        }

        public GameObject CurrentStepObject()
        {
            return systems[currentSystem].gameObject;
        }

        private int currentSystem;
    }
}