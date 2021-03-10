using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleManager : SceneSingleton<AssetBundleManager>
{
    public bool LocalBundles;
    public string RemotePath;

    [Serializable]
    class Bundle
    {
        public string name;
        public int version;
    }

    [Serializable]
    class BundleManifest
    {
        public List<Bundle> bundles = new List<Bundle>();
    }

    const string MANIFEST_FILENAME = "manifest.json";

    Dictionary<string, AssetBundle> LoadedBundles = new Dictionary<string, AssetBundle>();

    public bool Exists(string name)
    {
        if (LocalBundles)
        {
            return File.Exists(Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + name.ToLower());
        }

        return File.Exists(Application.persistentDataPath + Path.AltDirectorySeparatorChar + name.ToLower());
    }

    public GameObject LoadDynamicScene(string name)
    {
        if (LocalBundles)
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + name.ToLower());
            LoadedBundles[name] = bundle;

            return Instantiate(bundle.LoadAsset<GameObject>(name));
        }
        else
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(Application.persistentDataPath + Path.AltDirectorySeparatorChar + name.ToLower());
            LoadedBundles[name] = bundle;

            return Instantiate(bundle.LoadAsset<GameObject>(name));
        }
    }

    public void Download()
    {
        StartCoroutine(DownloadFile(MANIFEST_FILENAME, DownloadLevels));
    }

    public void DownloadLevels()
    {
        string filePath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + MANIFEST_FILENAME;

        if (LocalBundles)
            filePath = Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + MANIFEST_FILENAME;

        if (File.Exists(filePath))
        {
            BundleManifest manifest = JsonUtility.FromJson<BundleManifest>(File.ReadAllText(filePath));

            foreach(Bundle bundle in manifest.bundles)
            {
                StartCoroutine(DownloadFile(bundle.name));
            }
        }
    }

    IEnumerator DownloadFile(string name, Action callback = null)
    {
        string uri = RemotePath + name;

        if (LocalBundles)
            uri = Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + name;
            
        UnityWebRequest request = UnityWebRequest.Get(uri);

        yield return request.SendWebRequest();

        while (!request.downloadHandler.isDone)
        {
            Debug.Log(request.downloadProgress.ToString());
            yield return new WaitForFixedUpdate();
        }

        if (!request.isNetworkError && !request.isHttpError)
        {
            byte[] data = request.downloadHandler.data;

            File.WriteAllBytes(Application.persistentDataPath + Path.AltDirectorySeparatorChar + name, request.downloadHandler.data);
        }

        callback?.Invoke();
    }
}
