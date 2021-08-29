namespace Steps
{
    public enum StepAction
    {
        Continue, // Передать ход следующему юниту
        Wait, // Повторить вызов Step во время следующего кадра
        Stop, // Прекратить выполнение системы и удалить компонент
    }

    // Интерфейс, необходимый реализовать для любого объекта, что может ходить
    public interface IStepSystem
    {
        StepAction Step(UI.Logger logger);
    }
}