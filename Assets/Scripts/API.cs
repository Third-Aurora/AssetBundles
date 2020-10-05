using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class API : MonoBehaviour {

    const string BundleFolder = "https://www.matthewhallberg.com/AssetBundles/";

    public void GetBundleObject(string assetName, UnityAction<GameObject> callback, Transform bundleParent) {
        StartCoroutine(GetDisplayBundleRoutine(assetName, callback, bundleParent));
    }

    IEnumerator GetDisplayBundleRoutine(string assetName, UnityAction<GameObject> callback, Transform bundleParent) {

        string bundleURL = BundleFolder + assetName + "-";

        //append platform to asset bundle name
#if UNITY_ANDROID
        bundleURL += "Android";
#else
        bundleURL += "IOS";
#endif

        Debug.Log("Requesting bundle at " + bundleURL);

        //request asset bundle
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleURL);
        yield return www.SendWebRequest();

        if (www.isNetworkError) {
            Debug.Log("Network error");
        } else {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            if (bundle != null) {
                string rootAssetPath = bundle.GetAllAssetNames()[0];
                GameObject arObject = Instantiate(bundle.LoadAsset(rootAssetPath) as GameObject,bundleParent);
                bundle.Unload(false);
                callback(arObject);
            } else {
                Debug.Log("Not a valid asset bundle");
            }
        }
    }
}
