using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum HttpType
{
    GET,
    POST,
    PUT,
    DELETE
}

public enum ServerType
{
    Global,
    Local
}


public class NetworkConnector : MonoBehaviour
{

    #region Singleton
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static NetworkConnector s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static NetworkConnector instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                s_Instance = FindObjectOfType(typeof(NetworkConnector)) as NetworkConnector;
                if (s_Instance == null)
                    Debug.LogWarning("Could not locate an NetworkConnector object. \n You have to have exactly one PR_Connector in the scene.");
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

    //Async calls to HTTP Request
    public void HttpGet(ServerType serverType, string url, Hashtable data, Func<WWW, int> callbackFunction = null, bool bPlainUrl = false)
    {
        StartCoroutine(HttpASyncCall(serverType, HttpType.GET, url, data, callbackFunction, bPlainUrl));
    }

    public void HttpPost(ServerType serverType, string url, Hashtable data, Func<WWW, int> callbackFunction = null, bool bPlainUrl = false)
    {
        StartCoroutine(HttpASyncCall(serverType, HttpType.POST, url, data, callbackFunction, bPlainUrl));
    }

    public void HttpDelete(ServerType serverType, string url, Hashtable data, Func<WWW, int> callbackFunction = null)
    {
        StartCoroutine(HttpASyncCall(serverType, HttpType.DELETE, url, data, callbackFunction));
    }

    public void UploadFile(ServerType serverType, string url, string fieldName, string fileName, string contentType, byte[] data, Hashtable parameters, Func<WWW, int> callbackFunction)
    {
        StartCoroutine(HttpASyncCallFileUpload(serverType, url, fieldName, fileName, contentType, data, parameters, callbackFunction));
    }

    //Direct calls to the server
    public void HttpGetDirect(string url, Hashtable data, Func<WWW, int> callbackFunction = null)
    {
        StartCoroutine(HttpASyncCallDirect(HttpType.GET, url, data, null, callbackFunction));
    }

    public void HttpPostDirect(string url, Hashtable data, Func<WWW, int> callbackFunction = null)
    {
        StartCoroutine(HttpASyncCallDirect(HttpType.POST, url, data, null, callbackFunction));
    }

    public void HttpPostDirect(string url, Hashtable data, Hashtable headers, Func<WWW, int> callbackFunction = null)
    {
        StartCoroutine(HttpASyncCallDirect(HttpType.POST, url, data, headers, null, callbackFunction));
    }

    public void HttpPostDirect(string url, Hashtable data, Hashtable headers, Hashtable body, Func<WWW, int> callbackFunction = null)
    {
        StartCoroutine(HttpASyncCallDirect(HttpType.POST, url, data, headers, body, callbackFunction));
    }

    public void HttpDeleteDirect(string url, Hashtable data, Func<WWW, int> callbackFunction = null)
    {
        StartCoroutine(HttpASyncCallDirect(HttpType.DELETE, url, data, null, callbackFunction));
    }

    public void DownloadProfilePicture(string url, Func<WWW, int> callbackFunction)
    {
        StartCoroutine(DownloadPicture(url, callbackFunction));
    }

    //Request the web data async
    private IEnumerator HttpASyncCall(ServerType serverType, HttpType type, string baseUrl, Hashtable data, Func<WWW, int> callbackFunction, bool bPlainUrl = false)
    {
        //Dummy request body
        byte[] dummmy = new byte[1];
        dummmy[0] = 0;

        //Construct the url if not plain
        string url = baseUrl;
        if (!bPlainUrl)
            url = BuildURL(serverType, type, baseUrl, data);

        if (!MB_Configs.bReleaseVersion)
            Debug.Log(type + " : " + url);

        WWW www;
        if (type == HttpType.GET)
        {
            www = new WWW(url);
            yield return www;
        }
        else
        {
            //Post with form data
            WWWForm form = new WWWForm();

            if (data != null)
            {
                foreach (DictionaryEntry pair in data)
                {
                    //Escape the data string while posting or updating
                    string strVal = pair.Value.ToString();
                    form.AddField(pair.Key.ToString(), strVal);
                    //Debug.Log(pair.Key.ToString() + " : " + strVal);
                }
            }

            //Attach the app sercret as header
            //Check AppMaster key for delete request (Delete are not allowed when the game is released)
            var headers = form.headers;
            var rawData = form.data;

            //if (type == JM_HttpType.POST)
            //    headers["Authorization"] = PR_Config.instance.serverConfig.AppSecret;
            //else
            //    headers["Authorization"] = PR_Config.instance.serverConfig.AppMasterKey;

            //print(" Request with : " + headers["Authorization"]);

            //Add authentication to post method
            www = new WWW(url, rawData, headers);
            yield return www;
        }

        //Add to callback function if there are no errors
        if (www.error != null)
        {
            //Send connection failed event
            MB_Configs.Results += www.error.ToString();

            if (!MB_Configs.bReleaseVersion)
                Debug.Log(www.error);
        }

        callbackFunction(www);
    }

    //Custom ASync call for calling directly (Get or Post )
    public IEnumerator HttpASyncCallDirect(HttpType type, string url, Hashtable data, Hashtable headers, Func<WWW, int> callbackFunction)
    {
        WWW www = null;
        if (type == HttpType.GET)
        {
            url = BuildDirectURL(type, url, data);
            www = new WWW(url);
            yield return www;
        }
        else
        {
            //Post with form data
            WWWForm form = new WWWForm();

            foreach (DictionaryEntry pair in data)
            {
                if (pair.Value != null)
                {
                    string strVal = pair.Value.ToString();
                    form.AddField(pair.Key.ToString(), strVal);
                }
                //Debug.Log(pair.Key.ToString() + " : " + strVal);
            }

            //Add Header
            var oldheaders = form.headers;
            var rawData = form.data;

            if (type == HttpType.PUT)
            {
                headers["X-HTTP-Method-Override"] = "PUT";
                www = new WWW(url, rawData, oldheaders);
            }
            else if (type == HttpType.DELETE)
            {
                print("Adding header in Delete");
                headers["X-HTTP-Method-Override"] = "DELETE";
                www = new WWW(url, rawData, oldheaders);
            }
            else
            {
                if (headers != null)
                {
                    foreach (DictionaryEntry entry in headers)
                        oldheaders.Add(entry.Key.ToString(), entry.Value.ToString());

                    www = new WWW(url, rawData, oldheaders);
                }
                else
                {
                    www = new WWW(url, rawData);
                }
            }

            yield return www;
        }

        if (!MB_Configs.bReleaseVersion)
            Debug.Log(type + " : " + url);

        //Add to callback function if there are no errors
        if (www.error != null)
        {
            //Send connection failed event
            MB_Configs.Results += www.error.ToString();

            if (!MB_Configs.bReleaseVersion)
                Debug.Log("Error: " + www.error);
        }

        if (callbackFunction != null)
            callbackFunction(www);
        else
        {
            if (!MB_Configs.bReleaseVersion)
                Debug.Log("Data retrieved : ");
        }
    }

    public IEnumerator HttpASyncCallDirect(HttpType type, string url, Hashtable data, Hashtable headers, Hashtable body, Func<WWW, int> callbackFunction)
    {
        WWW www = null;

        //Post with form data
        WWWForm form = new WWWForm();

        if (data != null)
        {
            foreach (DictionaryEntry pair in data)
            {
                if (pair.Value != null)
                {
                    string strVal = pair.Value.ToString();
                    form.AddField(pair.Key.ToString(), strVal);
                }
            }
        }

        //Add Header
        var oldheaders = form.headers;
        var rawData = form.data;

        if (type == HttpType.PUT)
        {
            oldheaders["X-HTTP-Method-Override"] = "PUT";
            www = new WWW(url, rawData, oldheaders);
        }
        else if (type == HttpType.DELETE)
        {
            print("Adding header in Delete");
            oldheaders["X-HTTP-Method-Override"] = "DELETE";
            www = new WWW(url, rawData, oldheaders);
        }
        else
        {
            //POST
            foreach (DictionaryEntry entry in headers)
                oldheaders[entry.Key.ToString()] = entry.Value.ToString();

            if (body == null)
                www = new WWW(url, rawData, oldheaders);
            else
            {
                byte[] bodyData = System.Text.Encoding.ASCII.GetBytes(body.toJson());
                www = new WWW(url, bodyData, oldheaders);
            }
        }

        yield return www;

        if (!MB_Configs.bReleaseVersion)
            Debug.Log(type + " : " + url);

        //Add to callback function if there are no errors
        if (www.error != null)
        {
            //Send connection failed event
            MB_Configs.Results += www.error.ToString();

            if (!MB_Configs.bReleaseVersion)
                Debug.Log("Error: " + www.error);
        }

        if (callbackFunction != null)
            callbackFunction(www);
        else
        {
            if (!MB_Configs.bReleaseVersion)
                Debug.Log("Data retrieved : ");
        }
    }

    //Multipart form upload
    public IEnumerator HttpASyncCallFileUpload(ServerType serverType, string url, string fieldName, string fileName, string contentType, byte[] data, Hashtable parameters, Func<WWW, int> callbackFunction)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData(fieldName, data, fileName, contentType);

        //Add form parameters
        if (data != null)
        {
            foreach (DictionaryEntry pair in parameters)
            {
                string strVal = pair.Value.ToString();
                form.AddField(pair.Key.ToString(), strVal);
                //Debug.Log(pair.Key.ToString() + " : " + strVal);
            }
        }

        // Upload to a server
        WWW www = new WWW(url, form);
        yield return www;

        callbackFunction(www);
    }

    //Download the facebook profile picture and store as static
    public IEnumerator DownloadPicture(string url, Func<WWW, int> callbackFunction = null)
    {
        // Start a download of the given URL
        WWW www = new WWW(url);
        yield return www;

        if (www.error != null)
        {
            Debug.LogWarning(www.error);
        }

        if (callbackFunction != null)
            callbackFunction(www);

    }

    ////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////          Utilities Functions       //////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////
    private string BuildURL(ServerType serverType, HttpType type, string baseUrl, Hashtable data)
    {
        string strHost = MB_Configs.instance.Host;
        string url = MB_Configs.Scheme + strHost + ":" + MB_Configs.Port + "/";

        //Add the additional url for the request
        url += baseUrl;

        //Add hashtable form values for GET request
        if (type == HttpType.GET)
        {
            if (data != null)
            {
                int i = 0;

                foreach (DictionaryEntry pair in data)
                {
                    if (i == 0)
                        url += "?" + pair.Key + "=" + pair.Value;
                    else
                        url += "&" + pair.Key + "=" + pair.Value;

                    i++;
                }
                //Add Timestamp for disable cache inside unity
                url += "&timestamp=" + DateTime.UtcNow.ToString();
            }
            else
            {
                //Add Timestamp for disable cache inside unity
                url += "?timestamp=" + DateTime.UtcNow.ToString();
            }
        }

        //Save the string as URI
        url.Replace("\r\n", "\n");
        url = System.Uri.EscapeUriString(url).Replace("#", "%23");

        return url;
    }

    private string BuildDirectURL(HttpType type, string url, Hashtable data)
    {
        //Add hashtable form values for GET request
        if (type == HttpType.GET)
        {
            if (data != null)
            {
                int i = 0;

                foreach (DictionaryEntry pair in data)
                {
                    if (i == 0)
                        url += "?" + pair.Key + "=" + pair.Value;
                    else
                        url += "&" + pair.Key + "=" + pair.Value;

                    i++;
                }
                //Add Timestamp for disable cache inside unity
                url += "&timestamp=" + DateTime.UtcNow.Millisecond.ToString();
            }
            else
            {
                //Add Timestamp for disable cache inside unity
                url += "?timestamp=" + DateTime.UtcNow.Millisecond.ToString();
            }
        }

        //Save the string as URI
        url.Replace("\r\n", "\n");
        url = System.Uri.EscapeUriString(url).Replace("#", "%23");

        return url;
    }

    //Check whether the network connection is usable
    private bool IsConnectedToInternet()
    {
#if !UNITY_WINRT && !UNITY_IOS && !UNITY_ANDROID && UNITY_EDITOR || UNITY_STANDALONE
            if (Network.player.ipAddress.ToString() != "127.0.0.1")
                return true;
            else
                return false;
#elif UNITY_IOS || UNITY_ANDROID
            if (Application.internetReachability == NetworkReachability.NotReachable)
                return false;

            return true;
#else
        return false;
#endif
    }
}
