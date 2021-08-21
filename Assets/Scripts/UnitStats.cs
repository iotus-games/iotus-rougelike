using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitStat
{
    Damage = 0,
    Armor = 1,
}

[Serializable]
public class StatInfo
{
    public UnitStat stat;
    public float value;
    public float percent;
}

public class UnitStats : MonoBehaviour, IEnumerable
{
    public void Activate(UnitStat stat, float value)
    {
        if (FindStat(stat) != state.Count)
        {
            throw new Exception("Stat has been already activated!");
        }

        state.Add(new StatInfo {stat = stat, value = value, percent = 0});
    }

    public void Deactivate(UnitStat stat)
    {
        var i = FindStatChecked(stat);
        state.RemoveAt(i);
    }

    public void AddValue(UnitStat stat, float value)
    {
        state[FindStatChecked(stat)].value += value;
    }

    public void AddPercent(UnitStat stat, float value)
    {
        state[FindStatChecked(stat)].percent += value;
    }

    public float Value(UnitStat stat)
    {
        var s = state[FindStatChecked(stat)];
        return s.value + s.value * s.percent;
    }

    public IEnumerator GetEnumerator()
    {
        return state.GetEnumerator();
    }

    private int FindStat(UnitStat stat)
    {
        for (int i = 0; i < state.Count; i++)
        {
            if (state[i].stat == stat)
            {
                return i;
            }
        }

        return state.Count;
    }

    private int FindStatChecked(UnitStat stat)
    {
        var i = FindStat(stat);
        if (i == state.Count)
        {
            throw new Exception("Stat doesn't activated!");
        }

        return i;
    }

    [SerializeField] private List<StatInfo> state = new List<StatInfo>();
}