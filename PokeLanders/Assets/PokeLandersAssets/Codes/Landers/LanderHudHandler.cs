using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanderHudHandler : MonoBehaviour
{
	[Header("UI")]
	[SerializeField] private TMP_Text nameMesh;
	[SerializeField] private TMP_Text levelMesh;
	[SerializeField] private Image isMaleImage;
	[SerializeField] private Slider lifeBar;
	[SerializeField] private Slider xpBar;
	[SerializeField] private TMP_Text hpMesh;
}
