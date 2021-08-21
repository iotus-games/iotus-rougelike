using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCost
{
    public UnitResource Res;
    public float Val;
}

public class ActionResourceCosts : MonoBehaviour, IActionSystem, IEnumerable
{
    public UnitResources unitResources;

    [SerializeField] private List<ResourceCost> resourceCosts = new List<ResourceCost>();

    public bool CanCast(LogState logger)
    {
        foreach (var cost in resourceCosts)
        {
            if (unitResources.Value(cost.Res) <= cost.Val)
            {
                return false;
            }
        }

        return true;
    }

    public void DoCast(LogState logger)
    {
        foreach (var cost in resourceCosts)
        {
            unitResources.AddCurrent(cost.Res, -cost.Val);
        }
    }

    public void SetCost(UnitResource res, float val)
    {
        for (int i = 0; i < resourceCosts.Count; i++)
        {
            if (resourceCosts[i].Res == res)
            {
                if (val == 0)
                {
                    resourceCosts.RemoveAt(i);
                }
                else
                {
                    resourceCosts[i] = new ResourceCost {Res = res, Val = val};
                }

                return;
            }
        }

        resourceCosts.Add(new ResourceCost {Res = res, Val = val});
    }

    public IEnumerator GetEnumerator()
    {
        return resourceCosts.GetEnumerator();
    }
}