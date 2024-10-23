using UnityEngine;

namespace Lander.Gameplay
{
    public class AutoRotateAround : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float angle = 1.0f;
        [SerializeField] private float time = 1.0f;

        private float elapsedTime = 0.0f;
        private float radius = 0.0f;
        private float baseAngle = 0.0f;

        void Start()
        {
            radius = Vector3.Distance(transform.position, target.position);

            Vector3 directionToCamera = transform.position - target.position;
            baseAngle = Mathf.Atan2(directionToCamera.z, directionToCamera.x);
        }

        void Update()
        {
            elapsedTime += Time.deltaTime;
            elapsedTime %= time;

            float factor = Mathf.Sin(elapsedTime / time * Mathf.PI * 2.0f) / 2.0f + 0.5f;
            float result = (Mathf.Lerp(baseAngle, angle, factor) - angle / 2.0f) * Mathf.Deg2Rad;

            float x = target.position.x + Mathf.Sin(result) * radius;
            float z = target.position.z - Mathf.Cos(result) * radius;

            transform.position = new Vector3(x, transform.position.y, z);

            Vector3 targetPositionWithOffset = target.position + offset;
            transform.LookAt(targetPositionWithOffset);
        }
    }
}
