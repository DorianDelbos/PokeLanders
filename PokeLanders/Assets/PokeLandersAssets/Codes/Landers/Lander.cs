using System.Collections.Generic;
using UnityEngine;

public class Lander : MonoBehaviour
{
    // DB data
    [SerializeField] private LanderDataNFC data;
    private new string name;
    private string description;
    private List<ElementaryType> types = new List<ElementaryType>();

    private void Start()
    {
        GetLander(data.id);
    }

    public void GetLander(int id)
    {
        WebRequests.instance.DoRequest($"GetLandersById.php?ID={id}", fetch =>
        {
            name = fetch["Name"];
            description = fetch["Description"];

            string[] rows = fetch["Types"].Split(",");
            foreach (string row in rows)
                types.Add((ElementaryType)System.Enum.Parse(typeof(ElementaryType), row));
        });
    }
}
