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

public interface IUIResourcesListener
{
    void OnResourceModified(int index, ResourceInfo info);

    void OnResourceActivatedDeactivated();
}

public class UnitResources : MonoBehaviour, IEnumerable
{
    [SerializeField] private List<ResourceInfo> state = new List<ResourceInfo>();
    public IUIResourcesListener UiListener = null;

    public void Activate(UnitResource resource, float currentValue, float maxValue)
    {
        if (FindResource(resource) != state.Count)
        {
            throw new Exception("Resource has been already activated!");
        }


        state.Add(new ResourceInfo {res = resource, currentValue = currentValue, maxValue = maxValue});
        UiListener?.OnResourceActivatedDeactivated();
    }

    public void Deactivate(UnitResource resource)
    {
        var i = FindResourceChecked(resource);
        state.RemoveAt(i);
        UiListener?.OnResourceActivatedDeactivated();
    }

    public void AddCurrent(UnitResource resource, float value)
    {
        var id = FindResourceChecked(resource);
        var res = state[id];
        res.currentValue += value;
        res.currentValue = Math.Max(res.currentValue, 0);
        res.currentValue = Math.Min(res.currentValue, res.maxValue);
        UiListener?.OnResourceModified(id, res);
    }

    public void AddMax(UnitResource resource, float value)
    {
        var id = FindResourceChecked(resource);
        state[id].maxValue += value;
        UiListener?.OnResourceModified(id, state[id]);
    }

    public float Current(UnitResource resource)
    {
        var id = FindResourceChecked(resource);
        return state[id].currentValue;
    }

    public float Max(UnitResource resource)
    {
        var id = FindResourceChecked(resource);
        return state[id].maxValue;
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
}