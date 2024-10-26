using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lander.Gameplay
{
	using BundleAssetsLoad = Dictionary<System.Type, List<UnityEngine.Object>>;

	public class LanderMeshDisplayHandler : MonoBehaviour
    {
        [SerializeField] private MeshFilter landerMeshFilter;
        [SerializeField] private MeshRenderer landerMeshRenderer;

		public void SetMesh(BundleAssetsLoad bundleModel)
        {
            landerMeshFilter.mesh = (Mesh)bundleModel[typeof(Mesh)].First();
            landerMeshRenderer.SetMaterials(bundleModel[typeof(Material)].Select(x => (Material)x).ToList());
		}
    }
}
