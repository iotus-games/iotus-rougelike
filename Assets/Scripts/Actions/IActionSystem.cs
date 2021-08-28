using UnityEngine;

namespace Actions
{
    public interface IActionSystem
    {
        public void Prepare(GameObject caller);
        
        public bool CanCast(UI.Logger logger);

        public void DoCast(UI.Logger logger);
    }
}