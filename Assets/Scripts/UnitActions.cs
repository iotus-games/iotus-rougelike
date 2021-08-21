using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitActions : MonoBehaviour
{
    public void Start()
    {
        var cp = actionObjects;
        actionObjects = new List<Component>();
        foreach (var component in cp)
        {
            AddAction(component);
        }
    }

    public TComponent GetActionComponent<TComponent>(string actionName)
    {
        return FindAction(actionName).gameObject.GetComponent<TComponent>();
    }

    public void AddAction(Component action)
    {
        if (!typeof(IActionSystem).IsInstanceOfType(action))
        {
            throw new Exception(
                action.name + " Component '" + action.name + "' must be instance of IActionSystem");
        }

        if (action.gameObject.name.StartsWith(gameObject.name + "-"))
        {
            actionObjects.Add(action);
        } 
        else
        {
            var i = Instantiate(action.gameObject);
            i.name = gameObject.name + "-" + action.gameObject.name;
            actionObjects.Add(i.GetComponent(action.GetType()));
        }
    }

    public bool CanCast(LogState logger, string actionName)
    {
        return ((IActionSystem) FindAction(actionName)).CanCast(logger);
    }

    public void DoCast(LogState logger, string actionName)
    {
        ((IActionSystem) FindAction(actionName)).DoCast(logger);
    }

    private Component FindAction(string actionName)
    {
        foreach (var a in actionObjects)
        {
            if (a.gameObject.name == gameObject.name + "-" + actionName)
            {
                return a;
            }
        }

        throw new Exception("Object doesn't contains action named '" + actionName + "'");
    }

    [SerializeField] private List<Component> actionObjects;
}