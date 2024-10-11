using UnityEngine;

public class Lander : MonoBehaviour
{
    // DB data
    [SerializeField] private int id = -1;
    private new string name;
    private string description;

    private void Start()
    {
        GetLander(1);
    }

    public void GetLander(int id)
    {
        WebRequests.instance.DoRequest($"GetLandersById.php?ID={id}", fetch =>
        {
            this.id = id;
            name = fetch["Name"];
            description = fetch["Description"];
        });
    }
}
