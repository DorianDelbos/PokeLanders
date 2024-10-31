using System.Linq;
using UnityEngine;

namespace GLTFUtils
{
    public class GLTFDebug : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
                GLTFLoader.CreateMesh(GLTFLoader.LoadGLTF("Assets/PokeLandersAssets/Arts/Models/Duck/glTF/Duck.gltf"));
        }
    }
}