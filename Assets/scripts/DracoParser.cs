using UnityEngine;
using Draco;
using WebSocketSharp;
using System.Threading.Tasks;
using System;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class DracoParser : MonoBehaviour
{

  private WebSocket client = new WebSocket("ws://161.35.216.12:5677");
  private DracoMeshLoader draco = new DracoMeshLoader();
  private byte[] data = Array.Empty<byte>();

  void Start()
  {

    client.OnOpen += (sender, e) =>
    {
      Debug.Log("Client has connected");
    };

    client.OnMessage += (sender, e) =>
    {
      if (!e.IsBinary)
      {
        Debug.Log("DATA IS NOT BINARY");
        return;
      }
      data = e.RawData;

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

  async void Update()
  {
    if (data.Length != 0)
    {
      await handleData(data);
    }
  }

  int sum = 0;
  int count = 0;

  async Task handleData(byte[] data)
  {
    try
    {
      var t0 = DateTime.Now;
      var mesh = await draco.ConvertDracoMeshToUnity(data);
      var t1 = DateTime.Now;
      var diff1 = (t1 - t0).Milliseconds;
      sum = sum + diff1;
      count += 1;
      if (mesh != null)
      {
        GetComponent<MeshFilter>().mesh = mesh;
        Debug.Log("Decompress time: " + diff1 + "ms Avarage time: " + sum / count + "ms");
      }
    }
    catch (Exception e)
    {
      Debug.Log(e.Message);
    }

  }

}
