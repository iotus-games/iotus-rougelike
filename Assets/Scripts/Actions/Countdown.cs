using UnityEngine;

namespace Actions
{
    public class Countdown : MonoBehaviour, IActionSystem, IStepSystem
    {
        [SerializeField] public uint currentCountdown;
        [SerializeField] public uint countdown;

        public void Prepare(GameObject caller)
        {
        }

        public StepAction Step(UI.Logger logger)
        {
            if (currentCountdown != 0)
            {
                currentCountdown -= 1;
            }

            return StepAction.Continue;
        }

        public void DoCast(UI.Logger logger)
        {
            currentCountdown = countdown;
        }

        public bool CanCast(UI.Logger logger)
        {
            return currentCountdown == 0;
        }
    }
}