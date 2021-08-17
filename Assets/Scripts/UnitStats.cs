using UnityEngine;

public enum Stats
{
    Damage = 0,
    Armor = 1,
}

struct StatInfo
{
    public float Value;
    public float Percent;
}

public class UnitStats : MonoBehaviour
{
    public void AddValue(Stats stat, float value)
    {
        state[(int) stat].Value += value;
    }

    public void AddPercent(Stats stat, float value)
    {
        state[(int) stat].Percent += value;
    }

    public float Value(Stats stat)
    {
        var info = state[(int) stat];
        return info.Value + info.Value * info.Percent;
    }

    private StatInfo[] state = new StatInfo[2];
}