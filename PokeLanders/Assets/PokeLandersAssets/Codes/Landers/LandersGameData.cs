using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LandersGameData", menuName = "Landers/GameData")]
public class LandersGameData : ScriptableObject
{
	public List<Mesh> landersMesh = new List<Mesh>();
	public List<Sprite> landersSprites = new List<Sprite>();
}
