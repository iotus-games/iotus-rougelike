using UnityEngine;

public struct StepInfo
{
    public GameObject Object;
    public IStepSystem System;
}

// Последовательно выполняет ходы всех юнитов на уровне
public class StepState : MonoBehaviour
{
    public void Awake()
    {
        pipeline = gameObject.AddComponent<StepPipeline>();
    }

    private void Update()
    {
        if (stepBegin)
        {
            logger.Message("Step unit: " + pipeline.CurrentStepObject().name, MessageType.Step);
            stepBegin = false;
        }

        var result = pipeline.Step(logger, location);

        if (result == StepAction.Continue)
        {
            stepBegin = true;
        }
    }

    public void AddStepSystem(GameObject obj)
    {
        pipeline.AddStepSystem(obj);
    }

    public void RemoveStepSystem(GameObject obj)
    {
        pipeline.RemoveStepSystem(obj);
    }

    public Location location;
    public LogState logger;

    private StepPipeline pipeline;
    private bool stepBegin = true;
}