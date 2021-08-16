using UnityEngine;
using System.Collections.Generic;

// Позволяет выполнять одному юниту несколько последовательных действий в рамках одного хода
public class StepPipeline : MonoBehaviour, IStepSystem
{
    public StepAction Step(LogState logger, Location location)
    {
        var result = StepSystems[currentSystem].Step(logger, location);

        if (result == StepAction.Continue)
        {
            currentSystem += 1;
            if (currentSystem == StepSystems.Count)
            {
                currentSystem = 0;
                result = StepAction.Continue;
            }
        }

        return result;
    }

    public List<IStepSystem> StepSystems = new List<IStepSystem>();
    private int currentSystem;
}