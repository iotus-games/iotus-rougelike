using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    [Serializable]
    public class ResourceCost
    {
        public UnitResource res;
        public float val;
    }

    public class ResourceCosts : MonoBehaviour, IActionSystem, IEnumerable
    {
        private UnitResources unitResources;
        [SerializeField] private List<ResourceCost> resourceCosts = new List<ResourceCost>();

        public void Prepare(GameObject caller)
        {
            unitResources = caller.GetComponent<UnitResources>();
        }

        public bool CanCast(UI.Logger logger)
        {
            foreach (var cost in resourceCosts)
            {
                if (unitResources.Current(cost.res) < cost.val)
                {
                    return false;
                }
            }

            return true;
        }

        public void DoCast(UI.Logger logger)
        {
            foreach (var cost in resourceCosts)
            {
                unitResources.AddCurrent(cost.res, -cost.val);
            }
        }

        public void SetCost(UnitResource res, float val)
        {
            for (int i = 0; i < resourceCosts.Count; i++)
            {
                if (resourceCosts[i].res == res)
                {
                    if (val == 0)
                    {
                        resourceCosts.RemoveAt(i);
                    }
                    else
                    {
                        resourceCosts[i] = new ResourceCost {res = res, val = val};
                    }

                    return;
                }
            }

            resourceCosts.Add(new ResourceCost {res = res, val = val});
        }

        public IEnumerator GetEnumerator()
        {
            return resourceCosts.GetEnumerator();
        }
    }
}