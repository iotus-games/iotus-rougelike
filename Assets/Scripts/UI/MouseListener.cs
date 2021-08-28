using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class MouseListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool isOverlapped;

        public void OnPointerEnter(PointerEventData eventData)
        {
            isOverlapped = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isOverlapped = false;
        }
    }
}