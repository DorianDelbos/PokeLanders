using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LanderPc : MonoBehaviour
{
    // DB data
    private LanderDataNFC data;
    private new string name;
    private string description;
    private List<ElementaryType> types = new List<ElementaryType>();
    LandersGameData gameData = null;
    [SerializeField] private MeshFilter landerMeshFilter;

	public LanderDataNFC Data
    {
        set
        {
            data = value;
            WebRequest(data.id);
        }
    }

	private void OnEnable()
	{
        NfcModule.onNewNfcDetect += data => SetData(data);
        NfcModule.onNfcRemove += () => ResetData();
	}

	private void OnDisable()
	{
		NfcModule.onNewNfcDetect -= data => SetData(data);
		NfcModule.onNfcRemove -= () => ResetData();
	}

	private void Awake()
	{
        gameData = Resources.Load("LandersGameData") as LandersGameData;
	}

	private void SetData(LanderDataNFC data)
    {
        this.data = data;
        WebRequest(data.id);
		landerMeshFilter.mesh = gameData.landersMesh[data.id - 1];
	}

	private void ResetData()
    {
        data = null;
        name = string.Empty;
        description = string.Empty;
        types.Clear();
        landerMeshFilter.mesh = null;

	}

	private void WebRequest(int id)
    {
        WebRequests.instance.DoRequest($"GetLandersById.php?ID={id}", fetch =>
        {
            name = fetch["Name"];
            description = fetch["Description"];

            types.Clear();
            string[] rows = fetch["Types"].Split(",");
            foreach (string row in rows)
                types.Add((ElementaryType)System.Enum.Parse(typeof(ElementaryType), row));
        });
    }
}
