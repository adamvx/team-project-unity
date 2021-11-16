using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class Api
{
  private static readonly string BASE_URL = "http://161.35.216.12:5677/";

  public static IEnumerator CreateRoom(System.Action<Room> callback)
  {
    string uri = BASE_URL + "create";

    UnityWebRequest uwr = UnityWebRequest.Get(uri);
    yield return uwr.SendWebRequest();

    if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
    {
      Debug.Log("Error While Sending: " + uwr.error);
      yield break;
    }

    var room = JsonUtility.FromJson<Room>(uwr.downloadHandler.text);
    callback(room);

  }
}
