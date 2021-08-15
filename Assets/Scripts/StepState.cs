using System;
using System.Collections.Generic;
using UnityEngine;

public enum StepAction
{
    Continue,
    Wait,
}

public struct StepResult
{
    public StepAction Action;
    public List<MessageInfo> Messages;

    public StepResult(StepAction action)
    {
        Action = action;
        Messages = new List<MessageInfo>();
    }

    public StepResult Message(string text, MessageType type)
    {
        Messages.Add(new MessageInfo(text, type));
        return this;
    }
}

public interface IStepSystem
{
    StepResult Step();
}

public class StepPipeline : MonoBehaviour
{
    public StepResult Step()
    {
        var result = StepSystems[currentSystem].Step();

        if (result.Action == StepAction.Continue)
        {
            currentSystem += 1;
            if (currentSystem == StepSystems.Count)
            {
                currentSystem = 0;
                result.Action = StepAction.Continue;
            }
        }

        return result;
    }

    public List<IStepSystem> StepSystems = new List<IStepSystem>();
    private int currentSystem = 0;
}

public class StepState : MonoBehaviour
{
    public void Update()
    {
        var result = stepObjects[currentUnit].Step();

        if (stepBegin)
        {
            logger.Message("Step unit: " + gameObject.name, MessageType.Step);
            stepBegin = false;
        }

        if (result.Action == StepAction.Continue)
        {
            currentUnit += 1;
            stepBegin = true;

            if (currentUnit == stepObjects.Count)
            {
                currentUnit = 0;
            }
        }

        foreach (var message in result.Messages)
        {
            logger.Message(message.Text, message.Type);
        }
    }

    public InfoMessage logger;
    public List<StepPipeline> stepObjects;
    private int currentUnit;
    private bool stepBegin = true;
}