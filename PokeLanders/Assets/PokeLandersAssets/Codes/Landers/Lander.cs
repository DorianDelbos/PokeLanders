using System.Collections.Generic;
using UnityEngine;

public class Lander : MonoBehaviour
{
    // DB data
    private LanderDataNFC data;
    private new string name;
    private string description;
    private List<ElementaryType> types = new List<ElementaryType>();

    public LanderDataNFC Data
    {
        set
        {
            data = value;
            SetLander(data.id);
        }
    }

    public void SetLander(int id)
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
