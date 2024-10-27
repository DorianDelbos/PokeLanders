using Lander.Maths;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Lander.Gameplay;

namespace Lander.Battle
{
	public class BattleLanderHudHandler : MonoBehaviour
	{
		[Header("UI")]
		[SerializeField] private TMP_Text nameMesh;
		[SerializeField] private TMP_Text levelMesh;
		[SerializeField] private Image isMaleImage;
		[SerializeField] private Slider lifeBar;
		[SerializeField] private Slider xpBar;
		[SerializeField] private TMP_Text hpMesh;

        public void UpdateData(LanderData data) => UpdateData(data.Name, data.Level, data.Hp, data.MaxHp, data.BaseXp, data.Xp);

        public void UpdateData(string name, byte level, int hp, int maxHp, ushort baseXp, int xp)
        {
            nameMesh.text = name;
            levelMesh.text = $"Lvl.{level}";
            // TODO : Male/Female Image
            lifeBar.maxValue = maxHp;
            lifeBar.value = hp;
            hpMesh.text = $"{hp} / {maxHp}";
            xpBar.maxValue = StatsCurves.GetXpByLevel((byte)(level + 1), baseXp);
            xpBar.value = xp - StatsCurves.GetXpByLevel(level, baseXp);
        }
    }
}
