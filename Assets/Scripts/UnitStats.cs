using System;
using UnityEngine;

public enum Stats
{
    Damage = 0,
    Armor = 1,
}

public enum Resources
{
    Health = 0,
    Stamina = 1,
}

struct StatInfo
{
    public float Value;
    public float Percent;
}

struct ResourceInfo
{
    public float CurrentValue;
    public float MaxValue;
}

public class UnitStats : MonoBehaviour
{
    public void AddStatValue(Stats stat, float value)
    {
        stats[(int) stat].Value += value;
    }

    public void AddStatPercent(Stats stat, float value)
    {
        stats[(int) stat].Percent += value;
    }

    public void AddCurrentResource(Resources resource, float value)
    {
        var res = resources[(int) resource];
        res.CurrentValue += value;
        res.CurrentValue = Math.Max(res.CurrentValue, 0);
        res.CurrentValue = Math.Min(res.CurrentValue, res.MaxValue);
    }

    public void AddMaxResource(Resources resource, float value)
    {
        resources[(int) resource].MaxValue += value;
    }

    public float Value(Stats stat)
    {
        var info = stats[(int) stat];
        return info.Value + info.Value * info.Percent;
    }

    public float Value(Resources resource)
    {
        return resources[(int) resource].CurrentValue;
    }

    private StatInfo[] stats = new StatInfo[2];
    private ResourceInfo[] resources = new ResourceInfo[2];
}