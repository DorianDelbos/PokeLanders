using UnityEngine;

namespace LandersLegends.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public Lander[] Landers;

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

        private void Start()
        {
            Landers = new Lander[2];
        }
    }
}
