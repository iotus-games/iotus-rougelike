public enum StepAction
{
    Continue, // Передать ход следующему юниту
    Wait, // Повторить вызов Step на следующий кадр
}

// Интерфейс, необходимый реализовать для любого объекта, что может ходить
public interface IStepSystem
{
    StepAction Step(LogState logger, Location location);
}