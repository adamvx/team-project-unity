using UnityEngine;
using Draco;
using System.Threading.Tasks;
using System;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Player : MonoBehaviour
{

  private DracoMeshLoader draco = new DracoMeshLoader();
  private byte[] data = Array.Empty<byte>();
  private bool isUpdate = false;

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

  async Task handleData(byte[] data)
  {
    try
    {
      var mesh = await draco.ConvertDracoMeshToUnity(data);
      if (mesh != null)
      {
        GetComponent<MeshFilter>().mesh = mesh;
      }
    }
    catch (Exception e)
    {
      Debug.Log(e.Message);
    }

  }


}
