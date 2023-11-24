using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibration : MonoBehaviour
{
    const string pluginName = "com.kareem.unity.MyPlugin";
    static AndroidJavaClass _pluginClass;
    static AndroidJavaObject _pluginInstance;
    float elapsedTime = 0;

    public static AndroidJavaClass PluginClass {
        get {
            if (_pluginClass == null) {
                _pluginClass = new AndroidJavaClass(pluginName);
            }
            return _pluginClass;
        }
    }

    public static AndroidJavaObject PluginInstance {
        get {
            if (_pluginInstance == null) {
                _pluginInstance = PluginClass.CallStatic<AndroidJavaObject>("getInstance");
            }
            if (_pluginInstance == null) {
                Debug.Log("PluginInstance returned null.");
            }
            return _pluginInstance;
        }
    }

    double getElapsedTime()
    {
        double result = 0;
        try
        {
            if (PluginInstance != null)
            {
                result = PluginInstance.Call<double>("getElapsedTime");
            }
            else
            {
                Debug.LogError("PluginInstance is null. (getElapsedTime)");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to get elapsed time: " + e);
        }

        return result;
    }

    public static void Vibrate(int milliseconds)
    {
        Debug.Log("Attempting to vibrate...");

        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            PluginInstance.CallStatic("vibrate", context, milliseconds);
        }
        else
        {
            Debug.LogError("Vibration is supported only on Android devices.");
        }
    }

    void Start() {
        Debug.Log("Start...");

        if (PluginClass != null)
        {
            Debug.Log("PluginClass is not null. Attempting to get PluginInstance...");

            if (PluginInstance != null)
            {
                Debug.Log("PluginInstance is not null.");
            }
            else
            {
                Debug.LogError("PluginInstance is null. (Start)");
            }
        }
        else
        {
            Debug.LogError("PluginClass is null. (Start)");
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 5) {
            elapsedTime -= 5;
            Debug.Log("Tick: " + getElapsedTime());
        }
    }
}
