#define JSONDotNET

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//License Type : MIT
//Copyright (C) <2013> <JoyDash Pte Ltd, Singapore>
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;


#if PARSE
using Parse;
#endif

#if JSONDotNET
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.IO;
#endif


#if PHOTON

public class SortByMoneyHelper : IComparer<PhotonPlayer>
{
    public int Compare(PhotonPlayer x, PhotonPlayer y)
    {
        PhotonPlayer playerX = x;
        PhotonPlayer playerY = y;

        //Sort by ascending scores order
        long scoreX = PhotonUtility.GetPlayerProperties<long>(playerX, PhotonEnums.Player.Money);
        long scoreY = PhotonUtility.GetPlayerProperties<long>(playerY, PhotonEnums.Player.Money);
        Debug.Log("Money x: " + scoreX + "Money y: " + scoreY);



        return (scoreX).CompareTo(scoreY);
    }
}

public class SortByScoreHelper : IComparer<PhotonPlayer>
{
    public int Compare(PhotonPlayer x, PhotonPlayer y)
    {
        PhotonPlayer playerX = x;
        PhotonPlayer playerY = y;

        //Sort by descending scores order
        int scoreX = PhotonUtility.GetPlayerProperties<int>(playerX, PhotonEnums.Player.Scores);
        int scoreY = PhotonUtility.GetPlayerProperties<int>(playerY, PhotonEnums.Player.Scores);

        return (scoreY).CompareTo(scoreX);

        return 0;
    }
}
#endif

public class SortByLevelHelper : IComparer
{
    public int Compare(object x, object y)
    {
        //PR_FBScoresArray scoreX = (PR_FBScoresArray)x;
        //PR_FBScoresArray scoreY = (PR_FBScoresArray)y;

        ////Sort by ascending kills order
        //return (scoreX.CurLevel).CompareTo(scoreY.CurLevel);

        return 0;
    }
}

public class UrlShortener
{
    private static String ALPHABET = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private static int BASE = 62;

    public static String Encode(int num)
    {
        StringBuilder sb = new StringBuilder();

        while (num > 0)
        {
            sb.Append(ALPHABET[(num % BASE)]);
            num /= BASE;
        }

        StringBuilder builder = new StringBuilder();
        for (int i = sb.Length - 1; i >= 0; i--)
        {
            builder.Append(sb[i]);
        }
        return builder.ToString();
    }

    public static int Decode(String str)
    {
        int num = 0;

        for (int i = 0, len = str.Length; i < len; i++)
        {
            num = num * BASE + ALPHABET.IndexOf(str[(i)]);
        }

        return num;
    }
}

public class PR_Utility : MonoBehaviour
{
    #region Singleton
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static PR_Utility s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static PR_Utility instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first PR_Utility object in the scene.
                s_Instance = FindObjectOfType(typeof(PR_Utility)) as PR_Utility;
                if (s_Instance == null)
                    Debug.Log("Could not locate an PR_Utility object. \n You have to have exactly one PR_Utility in the scene.");
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

    public static Hashtable LoadData(string strContent, out string statusCode)
    {
        Hashtable data = new Hashtable();
        statusCode = "-1";

        try
        {
            data = (Hashtable)MiniJSONL.jsonDecode(strContent);

            if (data != null && data.ContainsKey("result") && data.ContainsKey("status"))
            {
                statusCode = PR_Utility.ParseString(data, "status");
                Hashtable objTable = PR_Utility.ParseHashtable(data, "result");
                return objTable;
            }
        }
        catch (Exception ex) { }

        return new Hashtable();
    }

    public static JObject LoadJData(string strContent, out string statusCode)
    {
        JObject data = new JObject();
        statusCode = "-1";

        try
        {
            data = JObject.Parse(strContent);
            if (data != null && data["result"] != null && data["status"] != null)
            {
                statusCode = PR_Utility.ParseString(data, "status");
                JObject objTable = PR_Utility.ParseHashtable(data, "result");
                return objTable;
            }
        }
        catch (Exception ex) { }

        return new JObject();
    }

    public static ArrayList LoadDataArray(string strContent, out string statusCode)
    {
        Hashtable data = new Hashtable();
        statusCode = "-1";

        try
        {
            data = (Hashtable)MiniJSONL.jsonDecode(strContent);
            if (data != null && data.ContainsKey("result") && data.ContainsKey("status"))
            {
                statusCode = PR_Utility.ParseString(data, "status");
                if (statusCode == "success")
                {
                    ArrayList objList = PR_Utility.ParseArrayList(data, "result");
                    return objList;
                }
            }
        }
        catch (Exception ex) { }

        return new ArrayList();
    }

    public static JArray LoadJDataArray(string strContent, out string statusCode)
    {
        JObject data = new JObject();
        statusCode = "-1";

        try
        {
            data = JObject.Parse(strContent);
            if (data != null && data["result"] != null && data["status"] != null)
            {
                statusCode = PR_Utility.ParseString(data, "status");
                if (statusCode == "success")
                {
                    JArray objList = PR_Utility.ParseArray(data, "result");
                    return objList;
                }
            }
        }
        catch (Exception ex) { }

        return new JArray();
    }

    public static string GetPrettyDate(DateTime d, bool bUTC = true)
    {
        // 1.
        // Get time span elapsed since the date.
        TimeSpan s = DateTime.Now.Subtract(d);

        if (bUTC)
            s = DateTime.UtcNow.Subtract(d);

        // 2.
        // Get total number of days elapsed.
        int dayDiff = (int)s.TotalDays;

        // 3.
        // Get total number of seconds elapsed.
        int secDiff = (int)s.TotalSeconds;

        // 4.
        // Don't allow out of range values.
        if (dayDiff < 0 || dayDiff >= 31)
        {
            return d.ToShortDateString();
        }

        // 5.
        // Handle same-day times.
        if (dayDiff == 0)
        {
            // A.
            // Less than one minute ago.
            if (secDiff < 60)
            {
                return "just now";
            }
            // B.
            // Less than 2 minutes ago.
            if (secDiff < 120)
            {
                return "1 minute ago";
            }
            // C.
            // Less than one hour ago.
            if (secDiff < 3600)
            {
                return string.Format("{0} minutes ago",
                    Math.Floor((double)secDiff / 60));
            }
            // D.
            // Less than 2 hours ago.
            if (secDiff < 7200)
            {
                return "1 hour ago";
            }
            // E.
            // Less than one day ago.
            if (secDiff < 86400)
            {
                return string.Format("{0} hours ago",
                    Math.Floor((double)secDiff / 3600));
            }
        }
        // 6.
        // Handle previous days.
        if (dayDiff == 1)
        {
            return "yesterday";
        }
        if (dayDiff < 7)
        {
            return string.Format("{0} days ago",
            dayDiff);
        }
        if (dayDiff < 31)
        {
            return string.Format("{0} weeks ago",
            Math.Ceiling((double)dayDiff / 7));
        }
        return null;
    }

    //public static bool IsSequential(int[] array)
    //{
    //    for (int i = 1; i < array.Length; i++)
    //        if (array[i] != array[i - 1] + 1)
    //            return false;
    //    return true;
    //}

    public static bool IsSequential(int[] a)
    {
        return Enumerable.Range(1, a.Length - 1).All(i => a[i] - 1 == a[i - 1]);
    }

    public static string CutoutString(string strVal, int length)
    {
        if (strVal.Length > length)
            strVal = strVal.Substring(0, length) + "...";

        return strVal;
    }

    //Using Json.et
    #region JSON.NET
    public static string ParseString(JArray data, string name, bool bCleanLine = false)
    {
        if (data != null && data[name] != null)
        {
            string strFinal = data[name].ToString();//.Replace("&#x0040;", "@");
            if (bCleanLine)
                strFinal = strFinal.Replace("\"", "").Replace("[", "").Replace("]", "");

            return strFinal;
        }

        return "";
    }

    public static string ParseString(JObject data, string name, bool bCleanLine = false)
    {
        if (data != null && data[name] != null)
        {
            string strFinal = data[name].ToString();//.Replace("&#x0040;", "@");
            if (bCleanLine)
                strFinal = strFinal.Replace("\"", "").Replace("[", "").Replace("]", "");

            return strFinal;
        }

        return "";
    }

    public static void SetString(ref JObject data, string field, string strNewField, string newData)
    {
        if (data != null && data[field] != null)
        {
            if (strNewField != "")
                data[strNewField] = newData;
            else
                data[field] = newData;
        }
    }

    public static float ParseFloat(JObject data, string name)
    {
        float val = 0.0f;
        if (data != null && data[name] != null)
        {
            if (float.TryParse(data[name].ToString(), out val))
                return float.Parse(data[name].ToString());
        }
        return 0.0f;
    }

    public static double ParseDouble(JObject data, string name)
    {
        double val = 0.0;
        if (data != null && data[name] != null)
        {
            if (double.TryParse(data[name].ToString(), out val))
                return double.Parse(data[name].ToString());
        }
        return 0.0;
    }

    public static JObject ParseHashtable(JObject data, string name)
    {
        if (data != null && data[name] != null)
        {
            if (data[name].GetType() == typeof(JObject))
                return (JObject)data[name];
            else if (data[name].GetType() == typeof(JValue))
            {
                string value = ((JValue)data[name]).ToString();
                try
                {
                    return new JObject(value);
                }
                catch (Exception ex)
                {
                    //Debug.Log(ex.Message);
                    return new JObject();
                }
            }
            else
            {
                //Debug.Log("You should parse with : " + data[name].GetType());
                return new JObject();
            }
        }
        return new JObject();
    }

    public static JArray ParseArray(JObject data, string name, int defaultNum = 0)
    {
        if (data != null && data[name] != null)
        {
            if (data[name].GetType() == typeof(JArray))
                return (JArray)data[name];
            else
                return new JArray();

        }
        return new JArray();
    }

    public static int ParseInt(JObject data, string name)
    {
        int val = 0;
        if (data != null && data[name] != null)
        {
            if (int.TryParse(data[name].ToString(), out val))
                return int.Parse(data[name].ToString());
        }

        return 0;
    }

    public static bool ParseBool(JObject data, string name)
    {
        bool val = false;
        if (data != null && data[name] != null)
        {
            if (bool.TryParse(data[name].ToString(), out val))
                return bool.Parse(data[name].ToString());
        }

        return false;
    }

    public static DateTime ParseDate(JObject data, string name)
    {
        DateTime dateVal;
        if (data != null && data[name] != null)
        {
            if (DateTime.TryParse((String)data[name], out dateVal))
                return DateTime.Parse((String)data[name]);
        }

        return DateTime.UtcNow;
    }

    #endregion

    public static string EncodeString(string strOrig)
    {
        strOrig = strOrig.Replace("&#x003c;", "<");
        strOrig = strOrig.Replace("&#x003e;", ">");
        return strOrig;
    }

    //Parsing data safely
    public static string ConvertToISODate(DateTime date)
    {
        return date.ToString("s");
        //return date.ToString("yyyy-MM-ddTHH:mm:ss.FFFZ");
        //return string.Format("ISODate(\"{0}\")", date.ToString("yyyy-MM-ddTHH:mm:ss.FFFZ"));
    }

    public static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    public static string GetString(byte[] bytes)
    {
        char[] chars = new char[bytes.Length / sizeof(char)];
        System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }

    public static string GetSeperatedValues<T>(T[] values, string seperation = ", ")
    {
        StringBuilder builder = new StringBuilder();
        foreach (T val in values)
            builder.Append(val.ToString() + seperation);

        return builder.ToString();
    }

    public static void IncreaseDataCount(ref Hashtable data, string name, int count)
    {
        if (data != null && data[name] != null)
        {
            int newCount = (int)data[name];
            newCount += count;
            data[name] = newCount;
        }
        else
            data[name] = count;
    }

    //Parsing data safely
    public static string ToUnicode(string S)
    {
        //UnicodeEncoding unicode = new UnicodeEncoding();
        //Byte[] encodedBytes = unicode.GetBytes(S);    

        //// Decode bytes back to string.
        //// Notice Pi and Sigma characters are still present.
        //String decodedString = unicode.GetString(encodedBytes);
        //Debug.Log(decodedString);
        //return decodedString;

        string strFinal = System.Text.RegularExpressions.Regex.Unescape(S);
        return strFinal;
        //return BytesToString(new UnicodeEncoding().GetBytes(S));
    }

    private static string BytesToString(byte[] Bytes)
    {
        MemoryStream MS = new MemoryStream(Bytes);
        StreamReader SR = new StreamReader(MS);
        string S = SR.ReadToEnd();
        SR.Close();
        return S;
    }

    public static string ParseString(Dictionary<string, object> data, string name)
    {
        if (data != null && data.ContainsKey(name) && data[name] != null)
        {
            if (data[name].GetType() != typeof(string))
            {
                Debug.Log("Invalid type string and parse with : " + data[name].GetType());
                return "";
            }

            //print((data[name].GetType()));
            string strFinal = ((String)data[name]).Replace("&#x0040;", "@");
            return strFinal;
        }

        return "";
    }

    public static string ParseString(Hashtable data, string name)
    {
        if (data != null && data[name] != null)
        {
            if (data[name].GetType() != typeof(string))
            {
                Debug.Log("Invalid type string and parse with : " + data[name].GetType());
                return "";
            }

            //print((data[name].GetType()));
            string strFinal = ((String)data[name]).Replace("&#x0040;", "@");
            return strFinal;
        }

        return "";
    }

    public static Hashtable ParseHashtable(Hashtable data, string name)
    {
        if (data != null && data[name] != null)
        {
            if (data[name].GetType() != typeof(Hashtable))
            {
                return new Hashtable();
            }

            return (Hashtable)data[name];
        }

        return new Hashtable();
    }

    public static Hashtable ParseHashtable(Dictionary<string, object> data, string name)
    {
        if (data != null && data.ContainsKey(name) && data[name] != null)
        {
            if (data[name].GetType() != typeof(Dictionary<string, object>))
            {

                return new Hashtable();
            }

            return new Hashtable((Dictionary<string, object>)data[name]);
        }

        return new Hashtable();
    }

    public static ArrayList ParseArrayList(Hashtable data, string name)
    {
        if (data != null && data[name] != null)
        {
            if (data[name].GetType() != typeof(ArrayList))
            {
                return new ArrayList();
            }

            return (ArrayList)data[name];
        }

        return new ArrayList();
    }

    public static int ParseInt(Hashtable data, string name)
    {
        int val = 0;
        if (data != null && data[name] != null)
        {
            if (int.TryParse(data[name].ToString(), out val))
                return int.Parse(data[name].ToString());
        }

        return 0;
    }

    public static long ParseLong(Hashtable data, string name)
    {
        long val = 0;
        if (data != null && data[name] != null)
        {
            if (long.TryParse(data[name].ToString(), out val))
                return long.Parse(data[name].ToString());
        }

        return 0;
    }

    public static int ParseInt(string value)
    {
        int val = 0;
        if (int.TryParse(value, out val))
            return int.Parse(value);

        return 0;
    }

    public static float ParseFloat(Hashtable data, string name)
    {
        float val = 0.0f;
        if (data != null && data[name] != null)
        {
            if (float.TryParse(data[name].ToString(), out val))
                return val;
        }

        return val;
    }

    public static float ParseFloat(string name)
    {
        float val = 0.0f;
        if (float.TryParse(name, out val))
            return val;

        return val;
    }

    public static double ParseDouble(Hashtable data, string name)
    {
        double val = 0.0;
        if (data != null && data[name] != null)
        {
            if (double.TryParse(data[name].ToString(), out val))
                return double.Parse(data[name].ToString());
        }

        return 0.0;
    }

    public static double ParseDouble(string name)
    {
        double val = 0.0;
        if (double.TryParse(name, out val))
            return val;

        return 0.0;
    }

    public static bool ParseBool(Hashtable data, string name)
    {
        bool val = false;
        if (data != null && data[name] != null)
        {
            if (bool.TryParse(data[name].ToString(), out val))
                return bool.Parse(data[name].ToString());
        }

        return false;
    }

    public static DateTime ParseDate(Hashtable data, string name)
    {
        DateTime dateVal;
        if (data != null && data.ContainsKey(name))
        {
            if (DateTime.TryParse((String)data[name], out dateVal))
                return DateTime.Parse((String)data[name]);
        }

        return DateTime.UtcNow;
    }

    public static DateTime ParseDate(string strDateTime)
    {
        DateTime dateVal;
        if (DateTime.TryParse(strDateTime, out dateVal))
            return DateTime.Parse(strDateTime);

        return DateTime.UtcNow;
    }

    public static DateTime ParseDateFromString(string name)
    {
        DateTime dateVal;
        if (DateTime.TryParse(name, out dateVal))
            return DateTime.Parse(name);

        return DateTime.UtcNow;
    }

    public static DateTime ParseDateFromTick(long tick)
    {
        return new DateTime(tick);
    }

    public static string GetTimeFromTick(long tick)
    {
        int minutes = (int)(tick / 60f);
        int hours = (int)(minutes / 60f);
        int seconds = (int)(tick % 60f);
        //int fraction = (int)((tick * 10) % 10);

        //return string.Format("{0} hours : {1} minutes and {2} seconds", Mathf.Abs(hours), Mathf.Abs(minutes), Mathf.Abs(seconds));
        return string.Format("{0} : {1} : {2} sec(s)", Mathf.Abs(hours), Mathf.Abs(minutes), Mathf.Abs(seconds));
    }

    //Check time span
    public static TimeSpan CalculateTimeSpan(DateTime d1, DateTime d2)
    {
        TimeSpan timeSpan = d2 - d1;
        //Debug.Log( string.Format("There're {0} days between {1} and {2}", timeSpan.TotalDays, d1.ToString(), d2.ToString()) );
        return timeSpan;
    }
    public static Hashtable ConvertDictToHash(Dictionary<string, object> data)
    {
        Hashtable hash = new Hashtable();
        foreach (KeyValuePair<string, object> pair in data)
            hash.Add(pair.Key, pair.Value);

        return hash;
    }

    public static ArrayList ConvertListToArrayList(List<object> dataList)
    {
        ArrayList arrayList = new ArrayList();
        foreach (object obj in dataList)
            arrayList.Add(obj);

        return arrayList;
    }

    public static bool IsValidEmailAddress(string s)
    {
        if (string.IsNullOrEmpty(s))
            return false;
        else
        {
            var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return regex.IsMatch(s) && !s.EndsWith(".");
        }
    }

    //Parse
    //Parse
#if PARSE

        public static string ParseString(ParseObject data, string name)
        {
            string strFinal = data.Get<string>(name);
            if (strFinal == null)
                strFinal = "";

            strFinal = strFinal.Replace("&#x0040;", "@");
            return strFinal;
        }

        public static double ParseDouble(ParseObject data, string name)
        {
            if (data != null && data.ContainsKey(name) && data[name] != null)
            {
                double result = 1.0;
                if (double.TryParse(data[name].ToString(), out result))
                    return result;
                else
                    Debug.Log("Invalid type double and parse with : " + data[name].GetType());
            }

            return 0.0;
        }

        public static int ParseInt(ParseObject data, string name)
        {
            int val = 0;
            if (data != null && data.ContainsKey(name) && data[name] != null)
            {
                if (data[name].GetType() != typeof(int) && data[name].GetType() != typeof(Int64))
                {
                    Debug.Log("Invalid type int and parse with : " + data[name].GetType());
                    return 0;
                }

                //print((data[name].GetType()));
                int newVal = data.Get<int>(name);
                return newVal;
            }

            return 0;
        }

        public static ParseFile ParseFile(ParseObject data, string name)
        {
            if (data != null && data.ContainsKey(name) && data[name] != null)
                return data.Get<ParseFile>(name);

            return null;
        }

        public static byte[] ParseByteArray(ParseObject data, string name)
        {
            if (data != null && data.ContainsKey(name) && data[name] != null)
            {
                if (data[name].GetType() != typeof(byte[]))
                {
                    Debug.Log("Invalid type byte array and parse with : " + data[name].GetType());
                    return null;
                }

                return null;
            }

            return (byte[])data[name];
        }

        public static bool ParseBool(ParseObject data, string name)
        {
            if (data != null && data.ContainsKey(name) && data[name] != null)
            {
                if (data[name].GetType() != typeof(bool))
                {
                    Debug.Log("Invalid type bool and parse with : " + data[name].GetType());
                    return false;
                }

                bool strFinal = ((bool)data[name]);
                return strFinal;
            }

            return false;
        }

        public static DateTime ParseDate(ParseObject data, string name)
        {
            if (data != null && data.ContainsKey(name))
            {
                if (data[name].GetType() == typeof(DateTime))
                    return (DateTime)data[name];
            }

            return DateTime.UtcNow;
        }

        public static Hashtable ParseHashObject(ParseObject data, string name)
        {
            if (data != null && data.ContainsKey(name) && data[name] != null)
            {
                if (data[name].GetType() != typeof(Hashtable))
                {
                    Debug.Log("Invalid type string and parse with : " + data[name].GetType());
                }
                else
                    return (Hashtable)data[name];
            }

            return new Hashtable();
        }

        public static Dictionary<string, object> ParseDictionary(ParseObject data, string name)
        {
            if (data != null && data.ContainsKey(name) && data[name] != null)
            {
                if (data[name].GetType() != typeof(Dictionary<string, object>))
                {
                    Debug.Log("Invalid type string and parse with : " + data[name].GetType());
                }
                else
                    return (Dictionary<string, object>)data[name];
            }

            return new Dictionary<string, object>();
        }


        public static IList<object> ParseObjectList(ParseObject data, string name)
        {
            if (data != null && data.ContainsKey(name))
            {
                if (data[name].GetType() != typeof(List<object>))
                {
                    Debug.Log("Invalid type hashtable and parse with : " + data[name].GetType());
                }
                else
                {
                    return (IList<object>)data[name];
                }
            }

            //return (IList<object>)data[name];
            IList<object> newList = new List<object>();
            return newList;
        }


        public static ArrayList ParseArrayList(ParseObject data, string name)
        {
            if (data != null && data[name] != null)
            {
                if (data[name].GetType() != typeof(ArrayList))
                {
                    Debug.Log("Invalid type hashtable and parse with : " + data[name].GetType());
                    return new ArrayList();
                }

                return (ArrayList)data[name];
            }

            return new ArrayList();
        }

        public static string ParseFileURL(ParseObject data, string name)
        {
            if (data != null && data.ContainsKey(name) && data[name] != null)
            {
                if (data[name].GetType() != typeof(ParseFile))
                {
                    Debug.Log("Invalid type string and parse with : " + data[name].GetType());
                    return "";
                }

                //print((data[name].GetType()));
                ParseFile file = (ParseFile)data[name];
                return file.Url.OriginalString;
            }

            return "";
        }

        public static string ParseString(ParseUser data, string name)
        {
            if (data != null && data.ContainsKey(name) && data[name] != null)
            {
                if (data[name].GetType() != typeof(string))
                {
                    Debug.Log("Invalid type string and parse with : " + data[name].GetType());
                    return "N/A";
                }

                //print((data[name].GetType()));
                string strFinal = ((String)data[name]).Replace("&#x0040;", "@");
                return strFinal;
            }

            return "N/A";
        }

        public static double ParseDouble(ParseUser data, string name)
        {
            if (data != null && data.ContainsKey(name) && data[name] != null)
            {
                double result = 1.0;
                if (double.TryParse(data[name].ToString(), out result))
                    return result;
                else
                    Debug.Log("Invalid type double and parse with : " + data[name].GetType());
            }

            return 0.0;
        }
#endif




}