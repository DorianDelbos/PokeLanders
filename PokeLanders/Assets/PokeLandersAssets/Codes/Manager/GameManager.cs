using LandersLegends.Extern.API;
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

            // Initialize all web requests
            TypeRepository.Initialize();
            StatRepository.Initialize();
            NatureRepository.Initialize();
            MoveRepository.Initialize();
            LanderRepository.Initialize();
            EvolutionChainRepository.Initialize();
            AilmentRepository.Initialize();
        }

        private void Start()
        {
            Landers = new Lander[2];
        }
    }
}
