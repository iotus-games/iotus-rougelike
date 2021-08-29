using UnityEngine;

namespace Steps
{
    public class TemporaryStep : MonoBehaviour, IStepSystem
    {
        public IStepSystem system;
        public int stepsRemain;

        public StepAction Step(UI.Logger logger)
        {
            var result = system.Step(logger);

            if (result == StepAction.Continue)
            {
                stepsRemain -= 1;
            }

            if ((result == StepAction.Continue && stepsRemain == 0) || result == StepAction.Stop)
            {
                Destroy(this);
                return StepAction.Stop;
            }

            return result;
        }
    }
}