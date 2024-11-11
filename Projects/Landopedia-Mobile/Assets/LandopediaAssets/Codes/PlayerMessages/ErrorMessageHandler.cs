using UnityEngine;

namespace Landopedia
{
    [CreateAssetMenu(fileName = "ErrorMessageHandler", menuName = "Landopedia/ErrorMessageHandler")]
    public class ErrorMessageHandler : ScriptableObject
    {
        [TextArea(3, int.MaxValue)] public string webServiceError = "Our web services are currently inactive or under maintenance, please try again later.";
    }
}
