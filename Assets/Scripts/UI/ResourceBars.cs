using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ResourceBars : MonoBehaviour, IUIResourcesListener
    {
        public FilledBar barPrefab;
        public UnitResources unitResources;
        public Image barPanel;
        public float barStride;
        private List<FilledBar> bars = new List<FilledBar>();

        private void Start()
        {
            if (unitResources != null)
            {
                OnResourceActivatedDeactivated();
                unitResources.UiListener = this;
            }
        }

        public void OnResourceModified(int index, ResourceInfo info)
        {
            bars[index].SetCurrent(info.currentValue);
            bars[index].SetMax(info.maxValue);
        }

        public void ChangeUnit(UnitResources newResources = null)
        {
            unitResources = newResources;
            if (unitResources != null)
            {
                unitResources.UiListener = this;
                OnResourceActivatedDeactivated();
            }
            else
            {
                foreach (var bar in bars)
                {
                    Destroy(bar.gameObject);
                }

                bars.Clear();
            }
        }

        public void OnResourceActivatedDeactivated()
        {
            float offset = 0;
            foreach (var bar in bars)
            {
                Destroy(bar.gameObject);
            }

            bars.Clear();

            foreach (ResourceInfo res in unitResources)
            {
                var obj = Instantiate(barPrefab, barPanel.transform);
                obj.transform.Translate(Vector3.down * offset);
                offset += barStride;
                var bar = obj.GetComponent<FilledBar>();

                bar.SetColor(ResourceColor(res.res));
                bar.SetMessage(res.res.ToString());
                bar.SetMax(res.maxValue);
                bar.SetCurrent(res.currentValue);
                bars.Add(bar);
            }
        }

        private static Color ResourceColor(UnitResource res)
        {
            return res switch
            {
                UnitResource.Health => Color.red,
                UnitResource.Stamina => Color.green,
                _ => throw new ArgumentOutOfRangeException(nameof(res), res, null)
            };
        }
    }
}