using System;
using UnityEngine;

public enum Resources
{
    Health = 0,
    Stamina = 1,
}

struct ResourceInfo
{
    public float CurrentValue;
    public float MaxValue;
}

public class UnitResources : MonoBehaviour 
{
    public void AddCurrent(Resources resource, float value)
    {
        var res = state[(int) resource];
        res.CurrentValue += value;
        res.CurrentValue = Math.Max(res.CurrentValue, 0);
        res.CurrentValue = Math.Min(res.CurrentValue, res.MaxValue);
    }

    public void AddMax(Resources resource, float value)
    {
        state[(int) resource].MaxValue += value;
    }

    public float Value(Resources resource)
    {
        return state[(int) resource].CurrentValue;
    }

    private ResourceInfo[] state = new ResourceInfo[2];
}