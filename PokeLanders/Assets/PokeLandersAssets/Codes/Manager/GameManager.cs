using UnityEngine;

namespace Lander.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public LanderData[] Landers = new LanderData[2];

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
	}
}
