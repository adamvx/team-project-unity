using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
  private static readonly string HTTP = "http://";
  private static readonly string WS = "ws://";
  private static readonly string BASE_URL = "161.35.216.12:5677/";

  public void QuitGame()
  {
    Application.Quit();
  }

  public void CreateConference()
  {
    if (Storage.userName.Length == 0)
    {
      return;
    }
    StartCoroutine(CreateAndJoinRoom());
  }

  public void JoinConference()
  {
    if (Storage.userName.Length == 0 || Storage.link.Length == 0)
    {
      return;
    }
    LoadScene(1);
  }

  public void LoadScene(int level)
  {
    SceneManager.LoadScene(level);
  }

  public void SetUserName(string s)
  {
    Storage.userName = s;
  }
  public void SetLink(string s)
  {
    Storage.link = s;
  }

  public IEnumerator CreateAndJoinRoom()
  {
    string uri = HTTP + BASE_URL + "create";

    UnityWebRequest uwr = UnityWebRequest.Get(uri);
    yield return uwr.SendWebRequest();

    if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
    {
      Debug.Log("Error While Sending: " + uwr.error);
      yield break;
    }

    var room = JsonUtility.FromJson<Room>(uwr.downloadHandler.text);
    Storage.link = WS + BASE_URL + room.id;
    LoadScene(1);
  }

}