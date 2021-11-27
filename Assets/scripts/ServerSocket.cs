using UnityEngine;
using WebSocketSharp;

public interface ServerSocketHandler
{
  void onData(byte[] data);
}

public class ServerSocket
{
  private WebSocket client;
  public ServerSocket(string url, ServerSocketHandler handler)
  {

    this.client = new WebSocket(url);

    client.OnOpen += (sender, e) =>
    {
      Debug.Log("Client has connected to room");
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