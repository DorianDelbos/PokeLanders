using System;
using System.Collections.Generic;

namespace GLTFUtils
{
    [Serializable]
    public class GLTFData
    {
        [Serializable]
        public class AssetData
        {
            public string generator;
            public string version;
        }

        [Serializable]
        public class SceneData
        {
            public List<int> nodes;
        }

        [Serializable]
        public class NodeData
        {
            public List<int> children;
            public List<float> matrix;
            public int? mesh;
            public int? camera;
        }

        [Serializable]
        public class CameraData
        {
            public PerspectiveData perspective;
            public string type;

            [Serializable]
            public class PerspectiveData
            {
                public float aspectRatio;
                public float yfov;
                public float zfar;
                public float znear;
            }
        }

        [Serializable]
        public class MeshData
        {
            public List<PrimitiveData> primitives;
            public string name;

            [Serializable]
            public class PrimitiveData
            {
                public AttributesData attributes;
                public int indices;
                public int mode;
                public int? material;
            }
        }

        [Serializable]
        public class AttributesData
        {
            public List<Attribute> attributes;

            [Serializable]
            public class Attribute
            {
                public string key;
                public int value;

                public Attribute(string key, int value)
                {
                    this.key = key;
                    this.value = value;
                }
            }
        }

        [Serializable]
        public class AccessorData
        {
            public int bufferView;
            public int byteOffset;
            public int componentType;
            public int count;
            public List<float> max;
            public List<float> min;
            public string type;
        }

        [Serializable]
        public class MaterialData
        {
            public PBRMetallicRoughness pbrMetallicRoughness;
            public List<float> emissiveFactor;
            public string name;

            [Serializable]
            public class PBRMetallicRoughness
            {
                public BaseColorTexture baseColorTexture;

                public float metallicFactor;

                [Serializable]
                public class BaseColorTexture
                {
                    public int index;
                }
            }
        }

        [Serializable]
        public class TextureData
        {
            public int sampler;
            public int source;
        }

        [Serializable]
        public class ImageData
        {
            public string uri;
        }

        [Serializable]
        public class SamplerData
        {
            public int magFilter;
            public int minFilter;
            public int wrapS;
            public int wrapT;
        }

        [Serializable]
        public class BufferViewData
        {
            public int buffer;
            public int byteOffset;
            public int byteLength;
            public int? byteStride;
            public int target;
        }

        [Serializable]
        public class BufferData
        {
            public int byteLength;
            public string uri;
        }

        public AssetData asset;
        public int scene;
        public List<SceneData> scenes;
        public List<NodeData> nodes;
        public List<CameraData> cameras;
        public List<MeshData> meshes;
        public List<AccessorData> accessors;
        public List<MaterialData> materials;
        public List<TextureData> textures;
        public List<ImageData> images;
        public List<SamplerData> samplers;
        public List<BufferViewData> bufferViews;
        public List<BufferData> buffers;
    }
}
