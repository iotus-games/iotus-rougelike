using UnityEngine;
using System.Collections.Generic;

// Позволяет выполнять одному юниту несколько последовательных действий в рамках одного хода
public class StepPipeline : MonoBehaviour, IStepSystem
{
    public StepAction Step(LogState logger, Location location)
    {
        if (stepSystems.Count == 0)
        {
            return StepAction.Continue;
        }
        
        var result = stepSystems[currentSystem].System.Step(logger, location);

        if (result == StepAction.Continue)
        {
            currentSystem += 1;
            if (currentSystem == stepSystems.Count)
            {
                currentSystem = 0;
                result = StepAction.Continue;
            }
        }
        else if (result == StepAction.Stop)
        {
            RemoveStepSystem(stepSystems[currentSystem].Object);
            return StepAction.Continue;
        }

        return result;
    }

    public void AddStepSystem(GameObject obj)
    {
        stepSystems.Add(new StepInfo {Object = obj, System = obj.GetComponent<IStepSystem>()});
    }

    public void RemoveStepSystem(GameObject obj)
    {
        for (int i = 0; i < stepSystems.Count; i++)
        {
            if (stepSystems[i].Object == obj)
            {
                stepSystems.RemoveAt(i);
                Destroy(obj.GetComponent(typeof(IStepSystem)));
                break;
            }
        }
    }

    public GameObject CurrentStepObject()
    {
        return stepSystems[currentSystem].Object;
    }

    private List<StepInfo> stepSystems = new List<StepInfo>();
    private int currentSystem;
}