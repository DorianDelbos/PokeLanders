using UnityEngine;

public class LanderDisplayHandler : MonoBehaviour
{
    [SerializeField] private MeshFilter landerMeshFilter;

    public void SetMesh(Mesh mesh)
    {
        landerMeshFilter.mesh = mesh;
    }
}