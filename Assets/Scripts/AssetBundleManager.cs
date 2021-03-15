using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    BundleManifest LocalManifest = null;

    const string LOCAL_MANIFEST_FILENAME = "localmanifest.json";
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

    public void Initialize()
    {
        string manifestFilePath = GetLocalManifestFilePath();

        if (File.Exists(manifestFilePath))
        {
            LocalManifest = JsonUtility.FromJson<BundleManifest>(File.ReadAllText(manifestFilePath));
        }

        StartCoroutine(DownloadFile(MANIFEST_FILENAME, DownloadLevels));
    }

    public void DownloadLevels()
    {
        string filePath = GetManifestFilePath();

        if (File.Exists(filePath))
        {
            BundleManifest manifest = JsonUtility.FromJson<BundleManifest>(File.ReadAllText(filePath));

            foreach(Bundle bundle in manifest.bundles)
            {
                Bundle localBundle = LocalManifest != null ? LocalManifest.bundles.Where(i => i.name == bundle.name).First() : null;

                if(localBundle == null || bundle.version > localBundle.version)
                {
                    StartCoroutine(DownloadFile(bundle.name));
                }
            }

            File.Copy(filePath, GetLocalManifestFilePath(), true);

            if(!LocalBundles)
                File.Delete(GetManifestFilePath());
        }
    }

    string GetLocalManifestFilePath()
    {
        return Application.persistentDataPath + Path.AltDirectorySeparatorChar + LOCAL_MANIFEST_FILENAME;
    }

    string GetManifestFilePath()
    {
        if (LocalBundles)
            return Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + MANIFEST_FILENAME;

        return Application.persistentDataPath + Path.AltDirectorySeparatorChar + MANIFEST_FILENAME;
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
