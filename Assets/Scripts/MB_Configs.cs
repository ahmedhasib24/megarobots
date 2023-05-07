using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MB_Configs : MonoBehaviour
{
    #region Singleton
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static MB_Configs s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static MB_Configs instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                s_Instance = FindObjectOfType(typeof(MB_Configs)) as MB_Configs;
                if (s_Instance == null)
                    Debug.LogWarning("Could not locate an MB_Configs object. \n You have to have exactly one PR_Config in the scene.");
            }
            return s_Instance;
        }
    }

    // Ensure that the instance is destroyed when the game is stopped in the editor.
    void OnApplicationQuit()
    {
        s_Instance = null;
    }
    #endregion

    public bool bLocalHost = false;
    public bool bDevelopment = false;
    public string curVersion = "1.0";
    public double curBundleVersion = 1;
    public static string myplayHost = "http://megarobots.fantasticmobilesolution.com/api";
    public string BundleID = "com.JoyDash.BundleID";
    public static string AndroidPublicKey = "";
    public static bool bDummyData = false;
    public static bool bReleaseVersion = true;
    public static bool IAP_TestMode = false;
    public static bool Ads_TestMode = false;
    public static string Results = "";
    
    //Social API Configurations
    public static string Port = "80";
    public static string SecurePort = "443";
    private static string scheme = "http://";
    private static string secureScheme = "https://";
    public static string Scheme { get { return scheme; } }
    public static string SecureScheme { get { return secureScheme; } }

    public string Host
    {
        get
        {
            if (bLocalHost)
                return "http://192.168.0.100:8080";
            else
            {
                if (bDevelopment)
                    return "http://sandbox.playrivals.com";
                else
                    return "http://megarobots.fantasticmobilesolution.com/api";
                    //return "http://www.aa13.com/api";
            }
        }
    }
}
