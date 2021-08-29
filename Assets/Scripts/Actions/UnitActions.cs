using System;
using System.Collections.Generic;
using UnityEngine;
using Steps;

namespace Actions
{
    public class UnitActions : MonoBehaviour
    {
        public List<GameObject> initObjects;
        private List<CastPipeline> actionObjects = new List<CastPipeline>();

        public void Start()
        {
            foreach (var obj in initObjects)
            {
                AddAction(obj);
            }

            initObjects.Clear();
        }

        public TComponent GetActionComponent<TComponent>(string actionName)
        {
            return FindAction(actionName).gameObject.GetComponent<TComponent>();
        }

        public void AddAction(GameObject obj)
        {
            if (!obj.name.StartsWith(gameObject.name + "-"))
            {
                var objName = obj.name;
                obj = Instantiate(obj);
                obj.name = gameObject.name + "-" + objName;
            }

            var pipeline = Pipeline.InitFromObject<CastPipeline>(obj);
            pipeline.Prepare(gameObject);
            actionObjects.Add(pipeline);
        }

        public bool CanCast(UI.Logger logger, string actionName)
        {
            return FindAction(actionName).CanCast(logger);
        }

        public void Cast(string actionName, GameObject caller)
        {
            var action = FindAction(actionName);
            var tmp = action.gameObject.AddComponent<TemporaryStep>();
            tmp.stepsRemain = 1;
            tmp.system = action;

            var pipeline = caller.GetComponent<StepPipeline>();
            pipeline.AddSystemAfterCurrent(tmp);
        }

        public void Cast(string actionName)
        {
            Cast(actionName, gameObject);
        }

        public bool TryCast(UI.Logger logger, string actionName)
        {
            if (CanCast(logger, actionName))
            {
                Cast(actionName);
                return true;
            }

            return false;
        }

        private CastPipeline FindAction(string actionName)
        {
            foreach (var a in actionObjects)
            {
                if (a.gameObject.name == gameObject.name + "-" + actionName)
                {
                    return a;
                }
            }

            throw new Exception(gameObject.name + " doesn't contains action named '" + actionName + "'");
        }
    }
}