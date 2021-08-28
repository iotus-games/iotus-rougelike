// Последовательно выполняет ходы всех юнитов на уровне
public class StepState : StepPipeline
{
    private void Update()
    {
        if (stepBegin)
        {
            logger.Message("Step unit: " + CurrentStepObject().name, UI.MessageType.Step);
            stepBegin = false;
        }

        var result = Step(logger);

        if (result == StepAction.Continue)
        {
            stepBegin = true;
        }
    }

    public UI.Logger logger;
    private bool stepBegin = true;
}