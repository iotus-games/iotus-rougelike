using UnityEngine;
using Steps;

namespace Actions
{
    public class CastPipeline : StepPipeline, IActionSystem
    {
        private GameObject caller;

        public void Awake()
        {
            componentBaseType = typeof(IActionSystem);
        }

        private new void Start()
        {
            base.Start();
            
            foreach (IActionSystem a in this)
            {
                a.Prepare(caller);
            }
        }

        public void Prepare(GameObject obj)
        {
            caller = obj;
        }

        public bool CanCast(UI.Logger logger)
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
    }
}