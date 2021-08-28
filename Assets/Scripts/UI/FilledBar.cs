using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FilledBar : MonoBehaviour
    {
        public Image barImage;
        public Text barText;
        [SerializeField] private Color color;
        [SerializeField] private float currentValue;
        [SerializeField] private float maxValue;
        [SerializeField] private string message;

        void Start()
        {
            SetColor(color);
            UpdateUI();
        }

        public void SetCurrent(float value)
        {
            currentValue = value;
            UpdateUI();
        }

        public void SetMax(float value)
        {
            maxValue = value;
            UpdateUI();
        }

        public void SetMessage(string value)
        {
            message = value;
            UpdateUI();
        }

        public void SetColor(Color newColor)
        {
            color = newColor;
            barImage.color = newColor;
        }

        private void UpdateUI()
        {
            barText.text = currentValue + "/" + maxValue + " " + message;
            barImage.fillAmount = currentValue / maxValue;
        }
    }
}