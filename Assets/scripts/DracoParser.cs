using UnityEngine;
using Draco;
using System.Threading.Tasks;
using System;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class DracoParser : MonoBehaviour, ServerSocketHandler
{

  private DracoMeshLoader draco = new DracoMeshLoader();
  private byte[] data = Array.Empty<byte>();
  private bool isUpdate = false;

  void Start()
  {

    // StartCoroutine(Api.CreateRoom(room =>
    // {
    //   Debug.Log(room.id);
    // }));

    // new ServerSocket("38b6-74c9-dac4", this);

  }

  public void onData(byte[] data)
  {
    this.data = data;
    isUpdate = true;
  }

  async void Update()
  {
    if (data.Length != 0 && isUpdate == true)
    {
      isUpdate = false;
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
