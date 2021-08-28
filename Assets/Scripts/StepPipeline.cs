using UnityEngine;

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

        var result = ((IStepSystem) systems[currentSystem]).Step(logger);

        if (result == StepAction.Continue)
        {
            currentSystem += 1;
            if (currentSystem == systems.Count)
            {
                currentSystem = 0;
                result = StepAction.Continue;
            }
        }
        else if (result == StepAction.Stop)
        {
            RemoveSystem(systems[currentSystem]);
            return StepAction.Continue;
        }

        return result;
    }

    public GameObject CurrentStepObject()
    {
        return systems[currentSystem].gameObject;
    }

    private int currentSystem;
}