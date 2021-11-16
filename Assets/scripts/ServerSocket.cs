using UnityEngine;
using WebSocketSharp;

public interface ServerSocketHandler
{
  void onData(byte[] data);
}

public class ServerSocket
{
  private static readonly string BASE_URL = "ws://161.35.216.12:5677/";

  private WebSocket client;
  public ServerSocket(string roomId, ServerSocketHandler handler)
  {

    this.client = new WebSocket(BASE_URL + roomId);

    client.OnOpen += (sender, e) =>
    {
      Debug.Log("Client has connected to room id: " + roomId);
    };

    client.OnMessage += (sender, e) =>
    {
      if (!e.IsBinary)
      {
        Debug.Log("DATA IS NOT BINARY");
        return;
      }
      handler.onData(e.RawData);
    };


    client.OnError += (sender, e) =>
      {
        Debug.Log("Client has error" + e.Message);
      };

    client.OnClose += (sender, e) =>
    {
      Debug.Log("Client has closed" + e.Reason);
    };

    client.Connect();
  }
}