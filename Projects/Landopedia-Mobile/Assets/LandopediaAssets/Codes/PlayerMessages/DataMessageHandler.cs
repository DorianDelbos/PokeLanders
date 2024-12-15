using UnityEngine;

namespace Landopedia
{
    [CreateAssetMenu(fileName = "ErrorMessageHandler", menuName = "Landopedia/ErrorMessageHandler")]
    public class DataMessageHandler : ScriptableObject
    {
        private static DataMessageHandler _i;
        public static DataMessageHandler instance
        {
            get
            {
                if (_i == null)
                    _i = Resources.Load("ErrorMessageHandler") as DataMessageHandler;

                return _i;
            }
        }

        [TextArea(3, int.MaxValue)] public string webServiceError = "Our web services are currently inactive or under maintenance, please try again later.";
        [TextArea(3, int.MaxValue)] public string waitTagNfc = "Place your lander behind the device ...";
        [TextArea(3, int.MaxValue)] public string mifareTagError = "Tag is not MifareClassic.";
        [TextArea(3, int.MaxValue)] public string mifareTagAuthenticationError = "Authentication failed to MifareClasic !";
        [TextArea(3, int.MaxValue)] public string resetTextAlert = "Warning: This action will permanently reset all your landers. Are you sure you want to continue ?";
    }
}
