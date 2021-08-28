using System;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public class UnitActions : MonoBehaviour
    {
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

            var pipeline = obj.GetComponent<CastPipeline>();
            if (pipeline == null)
            {
                pipeline = obj.AddComponent<CastPipeline>();
                pipeline.autoInitComponents = true;
            }

            pipeline.Prepare(gameObject);
            actionObjects.Add(pipeline);
        }

        public bool CanCast(UI.Logger logger, string actionName)
        {
            return FindAction(actionName).CanCast(logger);
        }

        public void DoCast(UI.Logger logger, string actionName)
        {
            FindAction(actionName).DoCast(logger);
        }

        public bool TryCast(UI.Logger logger, string actionName)
        {
            if (CanCast(logger, actionName))
            {
                DoCast(logger, actionName);
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


        public List<GameObject> initObjects;
        private List<CastPipeline> actionObjects = new List<CastPipeline>();
    }
}