using TMPro;
using UnityEngine;

namespace LanderFighter
{
    public class BattleTextInfo : MonoBehaviour
    {
        [SerializeField] private GameObject TextInfoDisplay;
        [SerializeField] private TMP_Dialogue textMesh;
        public TMP_Dialogue DialogueText => textMesh;

        public bool Enable
        {
            get => TextInfoDisplay.activeSelf;
            set => TextInfoDisplay.SetActive(value);
        }
    }
}
