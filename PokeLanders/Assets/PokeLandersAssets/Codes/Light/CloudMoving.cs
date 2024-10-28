using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LandersLegends.Gameplay
{
    [RequireComponent(typeof(UniversalAdditionalLightData))]
    public class CloudMoving : MonoBehaviour
    {
        private UniversalAdditionalLightData lightData;
        [SerializeField] private Vector2 speed = Vector2.one;

        private void Start()
        {
            lightData = GetComponent<UniversalAdditionalLightData>();
        }

        private void Update()
        {
            lightData.lightCookieOffset += speed * Time.deltaTime;
        }
    }
}
