using UnityEngine;

public class ActionCountdown : MonoBehaviour, IActionSystem, IStepSystem
{
    public StepAction Step(LogState logger)
    {
        if (currentCountdown != 0)
        {
            currentCountdown -= 1;
        }

        return StepAction.Continue;
    }

    public void DoCast(LogState logger)
    {
        currentCountdown = countdown;
    }

    public bool CanCast(LogState logger)
    {
        return currentCountdown == 0;
    }

    public uint currentCountdown;
    public uint countdown;
}