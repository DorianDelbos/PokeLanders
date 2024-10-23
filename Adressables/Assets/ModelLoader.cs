using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ModelLoader : MonoBehaviour
{
    private string APIUrl = "https://localhost:7041/api/v1/model/3.glb";
    public AssetReferenceGameObject assetReferenceGameObject;
    private GameObject instanceReference;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
            assetReferenceGameObject.LoadAssetAsync().Completed += OnAddressableLoaded;
        else if (Input.GetKeyUp(KeyCode.D))
            assetReferenceGameObject.ReleaseInstance(instanceReference);
    }

    private void OnAddressableLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
            instanceReference = handle.Result;
    }
}
