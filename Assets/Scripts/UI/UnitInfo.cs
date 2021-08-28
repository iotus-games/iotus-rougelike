using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UnitInfo : MonoBehaviour
    {
        public new Camera camera;
        public ResourceBars bars;
        public Text unitName;
        public GameObject activateObject;
        public GameObject player;

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out var hit);
                if (hit.collider == null || hit.collider.gameObject == player)
                {
                    activateObject.SetActive(false);
                    return;
                }

                var resources = hit.collider.gameObject.GetComponent<UnitResources>();
                if (resources == null)
                {
                    activateObject.SetActive(false);
                    return;
                }

                bars.ChangeUnit(resources);
                activateObject.SetActive(true);
                unitName.text = hit.collider.gameObject.name;
            }
        }
    }
}