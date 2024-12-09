using LandersLegends.Extern;
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

            // InitializeLanders
            InitializeLanders();
		}

        private void InitializeLanders()
        {
            Landers = new Lander[2];

#if UNITY_EDITOR
            string landerDebug1 = "00 00 00 01 41 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E FF 51 00 01 00 0A 00 00 00 40 00 0A 00 14 00 01 00 00 00 00 00 00 01 02 03 04 05 06 01 02 03 04 05 06";
            string landerDebug2 = "00 00 00 02 4D 61 72 73 68 6D 61 6C 6C 6F 77 20 20 20 20 33 00 03 00 0F 00 00 02 14 00 43 00 20 00 01 00 00 00 00 00 00 0A 14 1E 28 32 3C 46 50 5A 64 6E 78";

			LanderDataNFC nfcDebug = new LanderDataNFC(landerDebug1.ToByte());
			Landers[0] = new Lander(nfcDebug, LanderRepository.GetById(nfcDebug.id));
			nfcDebug = new LanderDataNFC(landerDebug2.ToByte());
			Landers[1] = new Lander(nfcDebug, LanderRepository.GetById(nfcDebug.id));
#endif
        }
    }
}
