using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GLTFUtils
{
    public static class GLTFLoader
    {
        public static GLTFData LoadGLTF(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError("Le fichier GLTF est introuvable : " + path);
                return null;
            }

            string gltfJson = File.ReadAllText(path);
            GLTFData gltfData = JsonUtility.FromJson<GLTFData>(gltfJson);

            Debug.Log("GLTF chargé avec succès : " + gltfData.asset.version);
            return gltfData;
        }

        public static void CreateMesh(GLTFData gltfData)
        {
            if (gltfData == null)
            {
                Debug.LogError("gltfData est nul, impossible de créer le maillage.");
                return;
            }

            if (gltfData.meshes == null || gltfData.meshes.Count == 0)
            {
                Debug.LogError("Aucun maillage trouvé dans gltfData.");
                return;
            }

            foreach (var meshData in gltfData.meshes)
            {
                if (meshData.primitives == null || meshData.primitives.Count == 0)
                {
                    Debug.LogError("Aucune primitive trouvée dans le maillage : " + meshData.name);
                    continue; // Passer au prochain maillage
                }

                Mesh unityMesh = new Mesh();
                List<Vector3> vertices = new List<Vector3>();
                List<Vector3> normals = new List<Vector3>();
                List<Vector2> uvs = new List<Vector2>();
                List<int> indices = new List<int>();

                foreach (var primitive in meshData.primitives)
                {
                    if (primitive.attributes == null)
                    {
                        Debug.LogError("Aucun attribut trouvé dans la primitive.");
                        continue; // Passer à la prochaine primitive
                    }

                    // Vérifier si "POSITION" est disponible
                    foreach (var attribute in primitive.attributes.attributes)
                    {
                        if (attribute.key == "POSITION")
                        {
                            int positionAccessorIndex = attribute.value;
                            GLTFData.AccessorData positionAccessor = gltfData.accessors[positionAccessorIndex];
                            GLTFData.BufferViewData positionBufferView = gltfData.bufferViews[positionAccessor.bufferView];
                            byte[] positionBufferData = LoadBuffer(gltfData.buffers[positionBufferView.buffer].uri);
                            ParseVertices(positionBufferData, positionAccessor, positionBufferView, vertices);
                        }

                        // Vérifier si "NORMAL" est disponible
                        if (attribute.key == "NORMAL")
                        {
                            int normalAccessorIndex = attribute.value;
                            GLTFData.AccessorData normalAccessor = gltfData.accessors[normalAccessorIndex];
                            GLTFData.BufferViewData normalBufferView = gltfData.bufferViews[normalAccessor.bufferView];
                            byte[] normalBufferData = LoadBuffer(gltfData.buffers[normalBufferView.buffer].uri);
                            ParseVertices(normalBufferData, normalAccessor, normalBufferView, normals);
                        }

                        // Vérifier si "TEXCOORD_0" est disponible
                        if (attribute.key == "TEXCOORD_0")
                        {
                            int uvAccessorIndex = attribute.value;
                            GLTFData.AccessorData uvAccessor = gltfData.accessors[uvAccessorIndex];
                            GLTFData.BufferViewData uvBufferView = gltfData.bufferViews[uvAccessor.bufferView];
                            byte[] uvBufferData = LoadBuffer(gltfData.buffers[uvBufferView.buffer].uri);
                            ParseUVs(uvBufferData, uvAccessor, uvBufferView, uvs);
                        }
                    }

                    // Charger les indices
                    if (primitive.indices != -1) // -1 indique qu'il n'y a pas d'indices
                    {
                        int indicesAccessorIndex = primitive.indices;
                        GLTFData.AccessorData indicesAccessor = gltfData.accessors[indicesAccessorIndex];
                        GLTFData.BufferViewData indicesBufferView = gltfData.bufferViews[indicesAccessor.bufferView];
                        byte[] indicesBufferData = LoadBuffer(gltfData.buffers[indicesBufferView.buffer].uri);
                        ParseIndices(indicesBufferData, indicesAccessor, indicesBufferView, indices);
                    }
                }

                // Appliquer les données au Mesh Unity
                unityMesh.SetVertices(vertices);
                if (normals.Count > 0) unityMesh.SetNormals(normals);
                if (uvs.Count > 0) unityMesh.SetUVs(0, uvs);
                unityMesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
                unityMesh.RecalculateNormals();

                // Création d'un GameObject pour afficher le Mesh
                GameObject meshObject = new GameObject(meshData.name);
                MeshFilter filter = meshObject.AddComponent<MeshFilter>();
                filter.mesh = unityMesh;
                MeshRenderer renderer = meshObject.AddComponent<MeshRenderer>();
                renderer.material = new Material(Shader.Find("Standard"));
            }
        }

        private static byte[] LoadBuffer(string uri)
        {
            return File.ReadAllBytes(uri);
        }

        private static void ParseVertices(byte[] bufferData, GLTFData.AccessorData accessor, GLTFData.BufferViewData bufferView, List<Vector3> vertices)
        {
            int byteStride = bufferView.byteStride ?? 12; // Assurer un byte stride par défaut
            for (int i = bufferView.byteOffset; i < bufferView.byteOffset + bufferView.byteLength; i += byteStride)
            {
                float x = BitConverter.ToSingle(bufferData, i);
                float y = BitConverter.ToSingle(bufferData, i + 4);
                float z = BitConverter.ToSingle(bufferData, i + 8);
                vertices.Add(new Vector3(x, y, z));
            }
        }

        private static void ParseUVs(byte[] bufferData, GLTFData.AccessorData accessor, GLTFData.BufferViewData bufferView, List<Vector2> uvs)
        {
            int byteStride = bufferView.byteStride ?? 8; // Assurer un byte stride par défaut
            for (int i = bufferView.byteOffset; i < bufferView.byteOffset + bufferView.byteLength; i += byteStride)
            {
                float u = BitConverter.ToSingle(bufferData, i);
                float v = BitConverter.ToSingle(bufferData, i + 4);
                uvs.Add(new Vector2(u, v));
            }
        }

        private static void ParseIndices(byte[] bufferData, GLTFData.AccessorData accessor, GLTFData.BufferViewData bufferView, List<int> indices)
        {
            for (int i = bufferView.byteOffset; i < bufferView.byteOffset + bufferView.byteLength; i += 2)
            {
                ushort index = BitConverter.ToUInt16(bufferData, i);
                indices.Add(index);
            }
        }
    }
}
