using UnityEngine;

namespace Landopedia
{
    public class RotateUI : MonoBehaviour
    {
        [SerializeField] private bool invert = false;
        [SerializeField] private float speed = 1.0f;
        private RectTransform rectTransform  = null;
        private float currentAngleZ          = 0.0f;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            currentAngleZ += speed * Time.deltaTime;
            currentAngleZ %= 360;

            Vector3 currentAngle = rectTransform.localEulerAngles;
            currentAngle.z = invert ? -currentAngleZ : currentAngleZ;
            rectTransform.localEulerAngles = currentAngle;
        }
    }
}
