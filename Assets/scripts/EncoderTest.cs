using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Draco;

public class EncoderTest : MonoBehaviour
{
    async void Start()
    {
        // Load file into memory
        var fullPath = Path.Combine(Application.streamingAssetsPath, "res.drc.bytes");
        var data = File.ReadAllBytes(fullPath);

        // Convert data to Unity mesh
        var draco = new DracoMeshLoader();
        // Async decoding has to start on the main thread and spawns multiple C# jobs.
        var mesh = await draco.ConvertDracoMeshToUnity(data);

        if (mesh != null)
        {
            // Use the resulting mesh
            GetComponent<MeshFilter>().mesh = mesh;
        }

        var result = Draco.Encoder.DracoEncoder.EncodeMesh(gameObject.GetComponent<MeshFilter>().mesh);
        Debug.Log(result);

        /*var writePath = Path.Combine(Application.streamingAssetsPath, "res.drc.bytes");
        for (var submesh = 0; submesh < result.Length; submesh++)
        {
            File.WriteAllBytes(writePath, result[submesh].data.ToArray());
            result[submesh].Dispose();
        }*/
    }

}
