using UnityEngine;
using Steps;

namespace Actions
{
    public interface IActionSystem : IStepSystem
    {
        public void Prepare(GameObject caller);
        
        public bool CanCast(UI.Logger logger);
    }
}