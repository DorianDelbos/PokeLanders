using UnityEngine;
using UnityEngine.UI;

namespace LanderFighter
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Gradient healthGradient;
        private Slider healthSlider;

        private void Awake()
        {
            healthSlider = GetComponent<Slider>();
        }

        public void SetHealth(ushort hp, ushort maxHp)
        {
            float factor = (float)hp / maxHp;
            healthSlider.value = factor;
            healthSlider.fillRect.GetComponent<Image>().color = healthGradient.Evaluate(factor);
        }
    }
}
