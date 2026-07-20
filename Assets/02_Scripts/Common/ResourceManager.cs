using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager
{
    private Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();


    private const int MAX_LOAD_COUNT = 4;
    private int _progressCount = 0;

    #region Init Load

    public async UniTask Init(System.Action<float> onProgress = null)
    {
        _progressCount = 0;
        onProgress?.Invoke(0f);

        var dataTable = GameManager.DataTable.PreLoadAssetDataTable;

        int totalCount = dataTable.Count;

        if (dataTable.Count == 0)
        {
            onProgress?.Invoke(1f);
            return;
        }

        List<UniTask> loadTasks = new(totalCount);
        using SemaphoreSlim semaphore = new(MAX_LOAD_COUNT);

        foreach (PreLoadAssetData preLoadData in dataTable.Values)
        {
            loadTasks.Add(LoadWithSemaphoreAsync(preLoadData, semaphore,
                () =>
                {
                    _progressCount++;
                    onProgress?.Invoke(_progressCount / (float)totalCount);
                }));
        }

        await UniTask.WhenAll(loadTasks);

        onProgress?.Invoke(1f);
    }

    private async UniTask LoadWithSemaphoreAsync(PreLoadAssetData preLoadData, SemaphoreSlim semaphore, System.Action onCompleted)
    {
        await semaphore.WaitAsync();

        try
        {
            switch (preLoadData.AssetType)
            {
                case "Mesh":
                    await LoadAsset<Mesh>(preLoadData.Address);
                    break;

                case "Material":
                    await LoadAsset<Material>(preLoadData.Address);
                    break;

                case "Prefab":
                case "GameObject":
                    await LoadAsset<GameObject>(preLoadData.Address);
                    break;

                case "AudioClip":
                    await LoadAsset<AudioClip>(preLoadData.Address);
                    break;

                default:
                    await LoadAsset<UnityEngine.Object>(preLoadData.Address);
                    break;
            }
        }
        finally
        {
            onCompleted?.Invoke();
            semaphore.Release();
        }
    }

    #endregion

    public async UniTask<T> LoadAsset<T>(string address) where T : UnityEngine.Object
    {
        if (_handles.TryGetValue(address, out AsyncOperationHandle handle))
        {
            return handle.Result as T;
        }

        AsyncOperationHandle<T> loadHandle = Addressables.LoadAssetAsync<T>(address);

        try
        {
            T result = await loadHandle.ToUniTask();

            _handles[address] = loadHandle;
            return result;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"에셋 로드 실패: {address} / Error: {e.Message}");

            if (loadHandle.IsValid())
                Addressables.Release(loadHandle);

            return null;
        }
    }

    public async UniTask<GameObject> InstantiateAsync(string address, Transform parent = null, bool instantiateInWorldSpace = false)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(address, parent, instantiateInWorldSpace);

        try
        {
            GameObject instance = await handle.ToUniTask();

            return instance;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"프리팹 생성 실패: {address} / Error: {e.Message}");

            if (handle.IsValid())
                Addressables.Release(handle);

            return null;
        }
    }

    public T GetLoadedAsset<T>(string address) where T : UnityEngine.Object
    {
        if (!_handles.TryGetValue(address, out AsyncOperationHandle handle))
        {
            Debug.LogWarning($"로드되지 않은 에셋입니다: {address}");
            return null;
        }

        if (!handle.IsValid())
        {
            Debug.LogWarning($"유효하지 않은 에셋 핸들입니다: {address}");
            return null;
        }

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogWarning($"에셋 로드가 완료되지 않았거나 실패한 에셋입니다: {address}");
            return null;
        }

        if (handle.Result is not T asset)
        {
            Debug.LogWarning($"에셋 타입이 일치하지 않습니다: {address}");
            return null;
        }

        return asset;
    }


    public void Release(string address)
    {
        if (_handles.TryGetValue(address, out AsyncOperationHandle handle))
        {
            Addressables.Release(handle);
            _handles.Remove(address);
            Debug.Log($"에셋 메모리 해제 완료: {address}");
        }
    }
}
