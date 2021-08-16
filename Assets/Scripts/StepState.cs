using System.Collections.Generic;
using UnityEngine;

struct StepInfo
{
    public GameObject Object;
    public IStepSystem System;
}

// Последовательно выполняет ходы всех юнитов на уровне
public class StepState : MonoBehaviour
{
    public void Update()
    {
        var result = stepObjects[currentUnit].System.Step(logger, location);

        if (stepBegin)
        {
            logger.Message("Step unit: " + stepObjects[currentUnit].Object.name, MessageType.Step);
            stepBegin = false;
        }

        if (result == StepAction.Continue)
        {
            currentUnit += 1;
            stepBegin = true;

            if (currentUnit == stepObjects.Count)
            {
                currentUnit = 0;
            }
        }
    }

    public void AddStepObject(GameObject obj)
    {
        stepObjects.Add(new StepInfo{Object = obj, System = obj.GetComponent<IStepSystem>()});
    }

    public void RemoveStepObject(GameObject obj)
    {
        for (int i = 0; i < stepObjects.Count; i++)
        {
            if (stepObjects[i].Object == obj)
            {
                stepObjects.RemoveAt(i);
                break;
            }
        }
    }

    public Location location;
    public LogState logger;
    
    private List<StepInfo> stepObjects = new List<StepInfo>();
    private int currentUnit;
    private bool stepBegin = true;
}