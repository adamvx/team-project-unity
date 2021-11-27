using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class Api : MonoBehaviour
{
    private static readonly string HTTP = "http://";
    private static readonly string WS = "ws://";
    private static readonly string BASE_URL = "161.35.216.12:5677/";

    public static string FinalLink;
    public IEnumerator CreateAndJoinRoomImpl()
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
        JoinRoom(room.id);
    }

    public void CreateAndJoinRoom()
    {
        StartCoroutine(CreateAndJoinRoomImpl());
    }

    public void JoinRoom(string code)
    {
        FinalLink = WS + BASE_URL + code;
        SceneManager.LoadScene(1);
    }

    public void JoinRoom()
    {
        FinalLink = WS + BASE_URL + storeName.theLink;
        SceneManager.LoadScene(1);
    }
}