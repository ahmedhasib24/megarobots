#if PHOTON
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PhotonUtility : MonoBehaviour 
{
    public static string ParseString(ExitGames.Client.Photon.Hashtable data, string name)
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

    public static int ParseInt(ExitGames.Client.Photon.Hashtable data, string name)
    {
        int val = 0;
        if (data != null && data[name] != null)
        {
            if (int.TryParse(data[name].ToString(), out val))
                return int.Parse(data[name].ToString());
        }

        return 0;
    }

    public static float ParseFloat(ExitGames.Client.Photon.Hashtable data, string name)
    {
        float val = 0.0f;
        if (data != null && data[name] != null)
        {
            if (float.TryParse(data[name].ToString(), out val))
                return val;
        }

        return val;
    }

    public static double ParseDouble(ExitGames.Client.Photon.Hashtable data, string name)
    {
        double val = 0.0;
        if (data != null && data[name] != null)
        {
            if (double.TryParse(data[name].ToString(), out val))
                return double.Parse(data[name].ToString());
        }

        return 0.0;
    }

    public static bool ParseBool(ExitGames.Client.Photon.Hashtable data, string name)
    {
        bool val = false;
        if (data != null && data[name] != null)
        {
            if (bool.TryParse(data[name].ToString(), out val))
                return bool.Parse(data[name].ToString());
        }

        return false;
    }

    public static int[] ParseIntArray(ExitGames.Client.Photon.Hashtable data, string name)
    {
        if (data != null && data[name] != null)
        {
            return (int[])data[name];
        }

        return null;
    }

    //Get or Set the index of player's properties
    public static T GetPlayerProperties<T>(PhotonPlayer player, string propertiesName)
    {
        object strVal = null;
        if (typeof(T) == typeof(string))
            strVal = "";
        else if (typeof(T) == typeof(int))
            strVal = 0;
        else if (typeof(T) == typeof(long))
            strVal = 0;
        else if (typeof(T) == typeof(double))
            strVal = 0.0;
        else if (typeof(T) == typeof(float))
            strVal = 0.0f;
        else if (typeof(T) == typeof(bool))
            strVal = false;
        else if (typeof(T) == typeof(int[]))
            strVal = new int[20];
        else if (typeof(T) == typeof(string[]))
            strVal = new string[20];
        else if (typeof(T) == typeof(Hashtable))
            strVal = new Hashtable();
        else
            strVal = null;

        if (player == null)
        {
            Debug.Log("PhotonPlayer is null");
            return (T)strVal;
        }

        ExitGames.Client.Photon.Hashtable properties = player.CustomProperties;

        if (properties.ContainsKey(propertiesName))
        {
            strVal = properties[propertiesName];
            //Debug.Log("Money : " + strVal);
            try
            {
                return (T)strVal;
            }
            catch (Exception ex)
            {
                Debug.Log("Exception : " + ex.Message);
                Debug.Log("Type should be : " + strVal.GetType());
                throw new Exception();
            }
        }
        else
            return (T)strVal;
    }

    //public static void IncreasePlayerProperties<T>(PhotonPlayer player, string propertiesName, object amount)
    //{
    //    T oldVal = PhotonUtility.GetPlayerProperties<T>(player, propertiesName);
    //    //oldVal += (dynamic)amount;
    //    object newAmount = (dynamic)oldVal + amount;
    //    SetPlayerProperties(player, propertiesName, newAmount);

    //    //ExitGames.Client.Photon.Hashtable properties = player.customProperties;

    //    //if (properties.ContainsKey(propertiesName))
    //    //{
    //    //    dynamic strVal = (dynamic)properties[propertiesName];
    //    //    properties[propertiesName] = strVal + amount;
    //    //}
    //    //else
    //    //{
    //        //properties[propertiesName] = amount;
    //    //}

    //    //player.SetCustomProperties(properties);
    //}

    public static PhotonPlayer GetPhotonPlayerFromID(string userid)
    {
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            if (player.UserId == userid)
                return player;
        }

        return null;
    }

    public static void SetPlayerProperties(PhotonPlayer player, string propertiesName, object val)
    {
        if (player != null)
        {
            ExitGames.Client.Photon.Hashtable properties = player.CustomProperties;
            properties[propertiesName] = val;
            player.SetCustomProperties(properties);
        }
    }

    public static void ClearPlayerProperties(PhotonPlayer player, List<string> excludedKeys = null)
    {
        //Need to set null to each entry to reset player's custom properties
        ExitGames.Client.Photon.Hashtable properties = player.customProperties;
        List<string> keys = new List<string>();

        //slot_index
        foreach (DictionaryEntry entry in properties)
        {
            string strKey = entry.Key.ToString();
            keys.Add(strKey);
        }

        foreach (string strKey in keys)
        {
            if (excludedKeys == null || !excludedKeys.Contains(strKey))
                properties[strKey] = null;
        }

        player.SetCustomProperties(properties);
    }

    public static void AddToPlayerProperties(PhotonPlayer player, string propertiesName, string val, string seperation = ",")
    {
        if (player != null)
        {
            ExitGames.Client.Photon.Hashtable properties = player.CustomProperties;
            properties[propertiesName] += val + seperation;
            player.SetCustomProperties(properties);
        }
    }

    public static void SetPlayerPropertiesArray(PhotonPlayer player, string[] propertiesName, object[] val)
    {
        if (player != null)
        {
            ExitGames.Client.Photon.Hashtable properties = player.CustomProperties;

            for(int i=0; i<propertiesName.Length; i++)
                properties[propertiesName[i]] = val[i];

            player.SetCustomProperties(properties);
        }
    }

    //Get or Set the index of room's properties
    //public static object GetRoomProperties<T>(string propertiesName)
    public static T GetRoomProperties<T>(string propertiesName)
    {
        object strVal = null;

        if (typeof(T) == typeof(string))
            strVal = "";
        else if (typeof(T) == typeof(int))
            strVal = 0;
        else if (typeof(T) == typeof(double))
            strVal = 0.0;
        else if (typeof(T) == typeof(float))
            strVal = 0.0f;
        else if (typeof(T) == typeof(bool))
            strVal = false;
        else if (typeof(T) == typeof(bool[]))
            strVal = new bool[6];
        else if (typeof(T) == typeof(int[]))
            strVal = new int[6];
        else if (typeof(T) == typeof(string[]))
            strVal = new string[0];
        else if (typeof(T) == typeof(Hashtable))
            strVal = new Hashtable();
        else
            strVal = null;

        if (PhotonNetwork.room == null)
        {
            Debug.Log("You're not connected to photon room!");
            return (T)strVal;
        }

        ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.room.CustomProperties;

        if (properties.ContainsKey(propertiesName))
            strVal = properties[propertiesName];

        try
        {
            return (T)strVal;
        }
        catch (Exception ex)
        {
            Debug.Log("Exception : " + ex.Message);
            Debug.Log("Type should be : " + strVal.GetType());
            throw new Exception();
        }
    }

    public static T GetRoomProperties<T>(RoomInfo room, string propertiesName)
    {
        if (room == null)
        {
            Debug.Log("You're not connected to photon network!");
        }

        ExitGames.Client.Photon.Hashtable properties = room.CustomProperties;
        object strVal = null;

        if (typeof(T) == typeof(string))
            strVal = "";
        else if (typeof(T) == typeof(int))
            strVal = 0;
        else if (typeof(T) == typeof(double))
            strVal = 0.0;
        else if (typeof(T) == typeof(float))
            strVal = 0.0f;
        else if (typeof(T) == typeof(bool))
            strVal = false;
        else if (typeof(T) == typeof(int[]))
            strVal = new int[20];
        else if (typeof(T) == typeof(string[]))
            strVal = new string[20];
        else if (typeof(T) == typeof(Hashtable))
            strVal = new Hashtable();
        else
            strVal = null;

        if (properties.ContainsKey(propertiesName))
            strVal = properties[propertiesName];

        return (T)strVal;
    }

    //public static T GetRoomProperties<T>(string propertiesName)
    //{
    //    ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.room.customProperties;
    //    if (properties.ContainsKey(propertiesName))
    //        return (T)properties[propertiesName];

    //    Type typeParameterType = typeof(T);
    //    if (typeParameterType == typeof(int))
    //        return 0;
    //}

    public static object GetRoomProperties(RoomInfo roomInfo, string propertiesName)
    {
        ExitGames.Client.Photon.Hashtable properties = roomInfo.CustomProperties;
        if (properties.ContainsKey(propertiesName))
            return properties[propertiesName];

        return null;
    }

    public static void SetRoomProperties(string propertiesName, object val)
    {
        if (PhotonNetwork.room == null)
        {
            Debug.Log("You're not connected to photon room!");
            return;
        }

        //if (propertiesName == "IsDistributingCards")
        //    Debug.Log("Setting Distributing Card : " + val);
        
        ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.room.CustomProperties;
        properties[propertiesName] = val;
        PhotonNetwork.room.SetCustomProperties(properties);
    }

    public static int IncreaseRoomProperties(string propertiesName, int val)
    {
        if (PhotonNetwork.room == null)
            Debug.Log("You're not connected to photon room!");

        int newVal = GetRoomProperties<int>(propertiesName);
        newVal += val;
        SetRoomProperties(propertiesName, newVal);
        return newVal;
    }

    public static void SetRoomPropertiesArray(string[] propertiesName, object[] val)
    {
        if (PhotonNetwork.room == null)
        {
            Debug.Log("You're not connected to photon room!");
            return;
        }

        ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.room.CustomProperties;

        for (int i = 0; i < propertiesName.Length; i++)
            properties[propertiesName[i]] = val[i];

        PhotonNetwork.room.SetCustomProperties(properties);
    }

    public static bool ComparePhotonPlayers(PhotonPlayer player01, PhotonPlayer player02)
    {
        return (player01.UserId == player02.UserId);
    }

    public static void AddOfflineGameEvent(ArrayList winnerList)
    {
        AddOfflineGameEvent((PhotonPlayer)winnerList[0]);
    }

    public static void AddGameEvent(PhotonPlayer dealer, PhotonPlayer player, int amountCredits, int amountGems)
    {
        //Add to game events if the player disconnected for some reasons
        string dealerID = GetPlayerProperties<string>(dealer, PhotonEnums.Player.PlayerID);
        string dealerCardTypes = GetPlayerProperties<string>(dealer, PhotonEnums.Player.RaceComplete);

        string playerID = GetPlayerProperties<string>(player, PhotonEnums.Player.PlayerID);
        string playerCardTypes = GetPlayerProperties<string>(player, PhotonEnums.Player.RaceComplete);

        //PR_GameEvent.AddGameEvent(GlobalVariables.gameType.ToString(), dealerID, dealerCardTypes, "", playerID, playerCardTypes, "", amountCredits, amountGems, PR_Player.Player.isVIP());
    }

    //TODO : Remove Offline game event for disconnected player in authorative approach
    public static void AddOfflineGameEvent(PhotonPlayer dealer)
    {
        ////Add to game events if the player disconnected for some reasons
        //string[] playerIDsArray = GetRoomProperties<string[]>(PhotonEnums.Room.PlayerIDs);
        //string[] cardTypesArray = GetRoomProperties<string[]>(PhotonEnums.Room.CardTypes);
        //string[] cardPointsArray = GetRoomProperties<string[]>(PhotonEnums.Room.CardPoints);
        //int[] betMoneysArray = GetRoomProperties<int[]>(PhotonEnums.Room.BetMoneys);

        //string winnerID = GetPlayerProperties<string>(dealer, PhotonEnums.Player.PlayerID);
        //string winnerCardTypes = AvaterCardManager.instance.GetPlayerCardTypes(dealer);
        //string winnerCardPoints = AvaterCardManager.instance.GetPlayerCardPoints(dealer);

        //for (int i = 0; i < playerIDsArray.Length; i++)
        //{
        //    string playerID = playerIDsArray[i];
        //    string cardTypes = cardTypesArray[i];
        //    string cardPoints = cardPointsArray[i];

        //    if (betMoneysArray.Length > 0 && i < betMoneysArray.Length)
        //    {
        //        int betMoney = betMoneysArray[i];

        //        if(GlobalVariables.bIsCoins)
        //            PR_GameEvent.AddGameEvent(GlobalVariables.gameType.ToString(), winnerID, winnerCardTypes, winnerCardPoints, playerID, cardTypes, cardPoints, -betMoney, 0, PR_Player.Player.isVIP());
        //        else
        //            PR_GameEvent.AddGameEvent(GlobalVariables.gameType.ToString(), winnerID, winnerCardTypes, winnerCardPoints, playerID, cardTypes, cardPoints, 0, -betMoney, PR_Player.Player.isVIP());
        //    }
        //}

        ////Reset disconnect players
        //playerIDsArray = new string[] { };
        //cardTypesArray = new string[] { };
        //cardPointsArray = new string[] { };
        //betMoneysArray = new int[] { };

        //SetRoomProperties(PhotonEnums.Room.PlayerIDs, playerIDsArray);
        //SetRoomProperties(PhotonEnums.Room.CardTypes, cardTypesArray);
        //SetRoomProperties(PhotonEnums.Room.CardPoints, cardPointsArray);
        //SetRoomProperties(PhotonEnums.Room.BetMoneys, betMoneysArray);
    }

    public static void AddDisconnectedPlayer(PhotonPlayer disconnectedPlayer)
    {
        ////Unactive players doesn't have to count
        //bool bActive = GetPlayerProperties<bool>(disconnectedPlayer, PhotonEnums.Player.Active);
        //if (!bActive)
        //    return;

        ////

        //string disconnectedPlayerID = GetPlayerProperties<string>(disconnectedPlayer, PhotonEnums.Player.PlayerID);
        //string disconnectedCardTypes = AvaterCardManager.instance.GetPlayerCardTypes(disconnectedPlayer);
        //string disconnectedCardPoints = AvaterCardManager.instance.GetPlayerCardPoints(disconnectedPlayer);
        //int disconnectedBetMoney = GetPlayerProperties<int>(disconnectedPlayer, PhotonEnums.Player.CurBetMoney);

        //string[] playerIDs = GetRoomProperties<string[]>(PhotonEnums.Room.PlayerIDs);
        //string[] cardTypes = GetRoomProperties<string[]>(PhotonEnums.Room.CardTypes);
        //string[] cardPoints = GetRoomProperties<string[]>(PhotonEnums.Room.CardPoints);
        //int[] betMoneys = GetRoomProperties<int[]>(PhotonEnums.Room.BetMoneys);
        //if (betMoneys.Length > 0 && betMoneys[0] == 0)
        //    betMoneys = new int[0];

        //List<string> playerIDsList = new List<string>(playerIDs);
        //List<string> cardTypesList = new List<string>(cardTypes);
        //List<string> cardPointsList = new List<string>(cardPoints);
        //List<int> betMoneysList = new List<int>(betMoneys);

        //if (!playerIDsList.Contains(disconnectedPlayerID))
        //{
        //    playerIDsList.Add(disconnectedPlayerID);
        //    cardTypesList.Add(disconnectedCardTypes);
        //    cardPointsList.Add(disconnectedCardPoints);
        //    betMoneysList.Add(disconnectedBetMoney);
        //}

        //string[] newPlayerIDs = playerIDsList.ToArray();
        //string[] newCardTypes = cardTypesList.ToArray();
        //string[] newCardPoints = cardPointsList.ToArray();
        //int[] newBetMoneys = betMoneysList.ToArray();

        ////Note:Having problems with offline game event features which causes all the peers to disconnect from the network
        ////The cause is because the first value became null while getting string[] values from room properties
        //SetRoomProperties(PhotonEnums.Room.PlayerIDs, newPlayerIDs);
        //SetRoomProperties(PhotonEnums.Room.CardTypes, newCardTypes);
        //SetRoomProperties(PhotonEnums.Room.CardPoints, newCardPoints);
        //SetRoomProperties(PhotonEnums.Room.BetMoneys, newBetMoneys);
    }

}
#endif