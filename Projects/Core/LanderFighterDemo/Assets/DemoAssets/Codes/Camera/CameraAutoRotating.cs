using dgames.Utils;
using UnityEngine;

namespace LanderFighter
{
    public class CameraAutoRotating : MonoBehaviour
    {
        [SerializeField] private float angleToRotate = 30.0f;
        [SerializeField] private float duration = 2.0f;

        private void Start()
        {
            StartCoroutine(TimeUtils.ElapsedTimeRoutine(duration, UpdateCameraRotation, null, true));
        }

        private void UpdateCameraRotation(float elapsedTime)
        {
            Vector3 euler = transform.eulerAngles;
            float f = elapsedTime * 2.0f / duration;
            euler.y = Mathf.LerpAngle(-angleToRotate / 2.0f, angleToRotate / 2.0f, EasingUtils.EaseInOutSine(f));
            transform.eulerAngles = euler;
        }
    }
}
