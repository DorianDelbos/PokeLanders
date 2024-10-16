using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LanderData
{
    private LanderDataNFC nfc = new LanderDataNFC();
    public string name = string.Empty;
    public string description = string.Empty;
    public List<ElementaryType> types = new List<ElementaryType>();
    public Mesh mesh;

    public LanderDataNFC Nfc
    {
        get => nfc;
        set
        {
            nfc = value;

            if (nfc != null)
                WebRequest(nfc.id);
        }
    }

    public void SetRandom()
    {
        nfc.tag = $"{Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)}";
        nfc.id = (ushort)Random.Range(0, 3);
        nfc.currentLevel = (ushort)Random.Range(0, 100);
        nfc.currentXp = (ushort)StatsCurves.GetXpByLevel(nfc.currentLevel);
        nfc.currentHp = (ushort)StatsCurves.GetMaxHpByLevel(nfc.currentLevel);

        WebRequest(nfc.id);
    }

    public void SetRandom(ushort level)
    {
        nfc.tag = $"{Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)} {Random.Range(0, 99)}";
        nfc.id = (ushort)Random.Range(0, 3);
        nfc.currentLevel = (ushort)Mathf.Clamp(level + (ushort)Random.Range(-5, 5), 0, 99);
        nfc.currentXp = (ushort)StatsCurves.GetXpByLevel(nfc.currentLevel);
        nfc.currentHp = (ushort)StatsCurves.GetMaxHpByLevel(nfc.currentLevel);

        WebRequest(nfc.id);
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

        mesh = (Resources.Load("LandersGameData") as LandersGameData).landersMesh[id - 1];
    }
}
