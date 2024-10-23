using System.Collections.Generic;
using UnityEngine;

namespace Lander.Gameplay
{
	[CreateAssetMenu(fileName = "LandersGameData", menuName = "Landers/GameData")]
	public class LandersGameData : ScriptableObject
	{
		public List<Mesh> landersMesh = new List<Mesh>();
		public List<Sprite> landersSprites = new List<Sprite>();

		public static Mesh GetLanderMeshAtId(int id) => (Resources.Load("LandersGameData") as LandersGameData).landersMesh[id - 1];
	}
}
