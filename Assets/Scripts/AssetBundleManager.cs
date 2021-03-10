using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetBundleManager : SceneSingleton<AssetBundleManager>
{
#if UNITY_EDITOR
    public bool LocalBundles;
#endif

    Dictionary<string, AssetBundle> LoadedBundles = new Dictionary<string, AssetBundle>();

    public bool Exists(string name)
    {
#if UNITY_EDITOR
        if (LocalBundles)
        {
            return File.Exists(Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + name.ToLower());
        }
#endif

        return false;
    }

    public GameObject LoadDynamicScene(string name)
    {
#if UNITY_EDITOR
        if (LocalBundles)
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + name.ToLower());
            LoadedBundles[name] = bundle;

            return Instantiate(bundle.LoadAsset<GameObject>(name));
        }
#endif

        return null;
    }
}
