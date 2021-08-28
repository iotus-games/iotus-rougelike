using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

// Базовая фунциональность для всех классов-конвейеров
public class Pipeline : MonoBehaviour, IEnumerable
{
    public void Start()
    {
        if (autoInitComponents)
        {
            foreach (var c in GetComponents(componentBaseType))
            {
                if (c != this)
                {
                    AddSystem(c);
                }
            }
        }
    }

    public T InitSystem<T>(GameObject obj) where T : Component, IStepSystem
    {
        var c = Library.GetOrAddComponent<T>(obj);
        systems.Add(c);
        return c;
    }

    public void AddSystem(Component c)
    {
        if (!componentBaseType.IsInstanceOfType(c))
        {
            throw new Exception(
                "Component '" + c.name + "' must be instance of " + componentBaseType.Name);
        }

        systems.Add(c);
    }

    public void RemoveSystem<T>(GameObject obj) where T : IStepSystem
    {
        RemoveSystem(obj.GetComponent(typeof(T)));
    }

    public IEnumerator GetEnumerator()
    {
        foreach (var s in systems)
        {
            yield return s;
        }
    }

    public void RemoveSystem(Component component)
    {
        if (systems.Remove(component))
        {
            Destroy(component);
        }
    }

    [SerializeField] protected List<Component> systems = new List<Component>();
    protected Type componentBaseType;

    // Автоматически добавить в конвеер все комоненты, наследующиеся от IStepSystem
    public bool autoInitComponents;
}