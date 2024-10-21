using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class LanderData
{
	#region ATTRIBUTS
	private LanderDataNFC nfc = null;
	private string name = string.Empty;
	private string description = string.Empty;
	private ushort maxHp = 0;
	private ushort physicalAttack = 0;
	private ushort specialAttack = 0;
	private ushort physicalDefense = 0;
	private ushort specialDefense = 0;
	private ushort speed = 0;
	private List<ElementaryType> types = null;
	private Mesh mesh = null;
	#endregion

	#region EVENTS
	public Action<ushort, ushort> onHpChange;
	#endregion

	#region GETTERS/SETTERS
	public string Tag => nfc.tag;
	public ushort ID => nfc.id;
	public string Name => name;
	public string CustomName => nfc.customName;
	public string Description => description;
	public ushort MaxHp => maxHp;
	public ushort PhysicalAttack => physicalAttack;
	public ushort SpecialAttack => specialAttack;
	public ushort PhysicalDefense => physicalDefense;
	public ushort SpecialDefense => specialDefense;
	public ushort Speed => speed;
	public List<ElementaryType> Types => types;
	public Mesh Mesh => mesh;
	public ushort Hp { get => nfc.hp; private set => nfc.hp = value; }
	public ushort Level { get => nfc.level; private set => nfc.level = value; }
	public ushort Xp { get => nfc.xp; private set => nfc.xp = value; }
public ushort Height => nfc.height;
	public ushort Weight => nfc.weight;
	#endregion

	#region CONSTRUCTORS
	public LanderData()
	{
		nfc = null;
		SetLanderBaseData(string.Empty, string.Empty, 0, 0, 0, 0, 0, 0, null, null);
	}

	public LanderData(byte[] nfcData)
	{
		nfc = new LanderDataNFC(nfcData);
		SetLanderBaseData(LanderWebRequest(ID));
	}

	public LanderData(LanderDataNFC nfcData)
	{
		nfc = nfcData;
		SetLanderBaseData(LanderWebRequest(ID));
	}

	public LanderData(byte[] nfcData, string name, string description, ushort maxHp, ushort physicalAttack, ushort specialAttack, ushort physicalDefense, ushort specialDefense, ushort speed, List<ElementaryType> types, Mesh mesh)
	{
		nfc = new LanderDataNFC(nfcData);
		SetLanderBaseData(name, description, maxHp, physicalAttack, specialAttack, physicalDefense, specialDefense, speed, types, mesh);
	}

	public LanderData(string tag, ushort id, string customName, ushort hp, ushort level, ushort xp, ushort height, ushort weight, string name, string description, ushort maxHp, ushort physicalAttack, ushort specialAttack, ushort physicalDefense, ushort specialDefense, ushort speed, List<ElementaryType> types, Mesh mesh)
	{
		nfc = new LanderDataNFC(tag, id, customName, hp, level, xp, height, weight);
		SetLanderBaseData(name, description, maxHp, physicalAttack, specialAttack, physicalDefense, specialDefense, speed, types, mesh);
	}
	#endregion

	#region STATICS
	public static LanderData CreateRandomLander() => CreateRandomLander((ushort)UnityEngine.Random.Range(1, 100));

	public static LanderData CreateRandomLander(ushort level)
	{
		ushort id = (ushort)UnityEngine.Random.Range(0, 3);
		Dictionary<string, string> fetch = LanderWebRequest(id);
		ushort nfcLevel = (ushort)Mathf.Clamp(level + (ushort)UnityEngine.Random.Range(-5, 5), 0, 99);
		ushort xp = (ushort)StatsCurves.GetXpByLevel(nfcLevel);
		ushort hp = ushort.Parse(fetch["MaxHp"]);

		return new LanderData(
			"-1",
			id,
			fetch["Name"],
			hp,
			level,
			xp,
			0,
			0,
			fetch["Name"],
			fetch["Description"],
			hp,
			ushort.Parse(fetch["PhysicalAttack"]),
			ushort.Parse(fetch["SpecialAttack"]),
			ushort.Parse(fetch["PhysicalDefense"]),
			ushort.Parse(fetch["SpecialDefense"]),
			ushort.Parse(fetch["Speed"]),
			ElementaryTypeUtils.StringsToTypes(fetch["Types"].Split(',')).ToList(),
			LandersGameData.GetLanderMeshAtId(id)
		);
	}

	private static Dictionary<string, string> LanderWebRequest(int id)
	{
		Dictionary<string, string> result = null;
		WebRequests.instance.DoRequest($"GetLandersById.php?ID={id}", (fetch, e) =>
		{
			if (e != null)
			{
				Debug.LogError($"Request failed: {e}");
			}

			result = fetch;
		});
		return result;
	}
	#endregion

	#region METHODS
	private void SetLanderBaseData(string name, string description, ushort maxHp, ushort physicalAttack, ushort specialAttack, ushort physicalDefense, ushort specialDefense, ushort speed, List<ElementaryType> types, Mesh mesh)
	{
		this.name = name;
		this.description = description;
		this.maxHp = maxHp;
		this.physicalAttack = physicalAttack;
		this.specialAttack = specialAttack;
		this.physicalDefense = physicalDefense;
		this.specialDefense = specialDefense;
		this.speed = speed;
		this.types = types;
		this.mesh = mesh;
	}

	private void SetLanderBaseData(Dictionary<string, string> fetch) => SetLanderBaseData(fetch["Name"], fetch["Description"], ushort.Parse(fetch["Hp"]), ushort.Parse(fetch["PhysicalAttack"]), ushort.Parse(fetch["SpecialAttack"]), ushort.Parse(fetch["PhysicalDefense"]), ushort.Parse(fetch["SpecialDefense"]), ushort.Parse(fetch["Speed"]), ElementaryTypeUtils.StringsToTypes(fetch["Types"].Split(",")).ToList(), LandersGameData.GetLanderMeshAtId(ID));
	
	private void TakeDamage(ushort damage)
	{
		Hp -= damage;
		onHpChange?.Invoke(Hp, maxHp);
	}
	#endregion
}
