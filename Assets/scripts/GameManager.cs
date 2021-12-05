using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour, ServerSocketHandler
{
    private static readonly string HTTP = "http://";
    private static readonly string WS = "ws://";
    private static readonly string BASE_URL = "161.35.216.12:5677/";

    private ServerSocket socket;

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

    public void StartDataStream()
    {
        if (Storage.link.Length > 0)
        {
            socket = new ServerSocket(Storage.link, this);
            SendDataTask().Start();
        }
    }

    public async Task SendDataTask()
    {
        uint x = 0;
        while (true)
        {
            var fullPath = Path.Combine(Application.streamingAssetsPath, "file" + x + ".drc");
            var data = File.ReadAllBytes(fullPath);
            socket.Send(data);
            if (x >= 120)
            {
                x = 0;
            }
            else
            {
                x++;
            }
            await Task.Delay(33);
        }
    }

    public void onData(byte[] data)
    {
        Debug.Log("Data has been recieved");
        //TODO: treba po prijati dat treba data preposlat na usera do jeho skriotu a tam ich namapovat na mesh
    }

    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
        if (level == 1)
        {
            StartDataStream();
        }
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