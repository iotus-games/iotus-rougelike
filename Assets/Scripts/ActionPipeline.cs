public class ActionPipeline : Pipeline, IActionSystem
{
    public void Awake()
    {
        ComponentBaseType = typeof(IActionSystem);
    }

    public bool CanCast(LogState logger)
    {
        foreach (IActionSystem a in this)
        {
            if (!a.CanCast(logger))
            {
                return false;
            }
        }

        return true;
    }

    public void DoCast(LogState logger)
    {
        foreach (IActionSystem a in this)
        {
            a.DoCast(logger);
        }
    }
}