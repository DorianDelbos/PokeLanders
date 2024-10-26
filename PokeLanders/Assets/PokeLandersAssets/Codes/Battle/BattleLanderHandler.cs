using UnityEngine;
using UnityEngine.Playables;

namespace Lander.Battle
{
    public class BattleLanderHandler : MonoBehaviour
    {
        [SerializeField] private PlayableDirector playableDirector;

        public void StartTurn()
        {
            playableDirector.Play();
        }

        public void EndTurn()
        {
            playableDirector.time = 0.0f;
            playableDirector.Stop();
        }
    }
}
