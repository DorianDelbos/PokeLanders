#nullable enable
using dgames.Utils;
using Landers;
using Landers.Utils;
using System;

namespace LanderFighter
{
    public class UserLanderManager : BaseUnitySingleton<UserLanderManager>
    {
        private LanderData? m_userLanderData;
        public LanderData UserLanderData
        {
            get
            {
                if (m_userLanderData == null)
                    Console.WriteLine("[DEBUG] Lander data is null. Random lander has been initialize.");

                return m_userLanderData ??= LanderUtils.RandomLander();
            }
            private set => m_userLanderData = value;
        }

        private void Awake()
        {
            TryInitializeInstance();
        }
    }
}
