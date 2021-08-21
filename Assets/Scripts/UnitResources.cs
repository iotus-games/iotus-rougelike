using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitResource
{
    Health = 0,
    Stamina = 1,
}

[Serializable]
public class ResourceInfo
{
    public UnitResource res;
    public float currentValue;
    public float maxValue;
}

public class UnitResources : MonoBehaviour, IEnumerable
{
    public void Activate(UnitResource resource, float currentValue, float maxValue)
    {
        if (FindResource(resource) != state.Count)
        {
            throw new Exception("Resource has been already activated!");
        }


        state.Add(new ResourceInfo {res = resource, currentValue = currentValue, maxValue = maxValue});
    }

    public void Deactivate(UnitResource resource)
    {
        var i = FindResourceChecked(resource);
        state.RemoveAt(i);
    }

    public void AddCurrent(UnitResource resource, float value)
    {
        var id = FindResourceChecked(resource);
        var res = state[id];
        res.currentValue += value;
        res.currentValue = Math.Max(res.currentValue, 0);
        res.currentValue = Math.Min(res.currentValue, res.maxValue);
    }

    public void AddMax(UnitResource resource, float value)
    {
        var id = FindResourceChecked(resource);
        state[id].maxValue += value;
    }

    public float Value(UnitResource resource)
    {
        var id = FindResourceChecked(resource);
        return state[id].currentValue;
    }

    public IEnumerator GetEnumerator()
    {
        return state.GetEnumerator();
    }

    private int FindResource(UnitResource resource)
    {
        for (int i = 0; i < state.Count; i++)
        {
            if (state[i].res == resource)
            {
                return i;
            }
        }

        return state.Count;
    }

    private int FindResourceChecked(UnitResource resource)
    {
        var i = FindResource(resource);
        if (i == state.Count)
        {
            throw new Exception("Resource doesn't activated!");
        }

        return i;
    }

    [SerializeField] private List<ResourceInfo> state = new List<ResourceInfo>();
}