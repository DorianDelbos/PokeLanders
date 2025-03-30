using dgames.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LanderFighter
{
    public class LoadingScreenHandler : BaseUnitySingleton<LoadingScreenHandler>
    {
        [SerializeField] private Canvas canva;
        [SerializeField] private TMP_Text textInfo;
        [SerializeField] private Image loadingImage;
        private float m_duration = 1.0f;

        public string Text
        {
            get => textInfo.text;
            set => textInfo.text = value;
        }

        public bool Enable
        {
            get => canva.gameObject.activeSelf;
            set => canva.gameObject.SetActive(value);
        }

        private void Awake()
        {
            TryInitializeInstance();
        }

        private void Start()
        {
            StartCoroutine(TimeUtils.ElapsedTimeRoutine(m_duration, UpdateIconRotation, null, true));
        }

        private void UpdateIconRotation(float elapsedTime)
        {
            Vector3 euler = loadingImage.rectTransform.eulerAngles;
            float f = elapsedTime / m_duration;
            euler.z = Mathf.LerpAngle(0.0f, 180.0f, EasingUtils.EaseInOutQuart(f));
            loadingImage.rectTransform.eulerAngles = euler;
        }
    }
}
