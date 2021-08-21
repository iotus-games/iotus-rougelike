public interface IActionSystem 
{
    public bool CanCast(LogState logger);
    
    public void DoCast(LogState logger);
}