using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.Kinect.Sensor;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System;
using WebSocketSharp;
using Draco;

public class KinectController : MonoBehaviour
{
  public int visibleTo = 4000;
  public int visbleFrom = 400;
  Device kinect;
  int num;
  Mesh mesh;
  Vector3[] vertices;
  Color32[] colors;

  int[] indices;
  Transformation transformation;

  void Start()
  {
    try
    {
      InitKinect();
      InitMesh();
      KinectLoop();
    }
    catch (Exception e)
    {

    }

  }

  void InitKinect()
  {
    kinect = Device.Open();
    kinect.StartCameras(new DeviceConfiguration
    {
      ColorFormat = ImageFormat.ColorBGRA32,
      ColorResolution = ColorResolution.R720p,
      DepthMode = DepthMode.NFOV_2x2Binned,
      SynchronizedImagesOnly = true,
      CameraFPS = FPS.FPS30
    });
    transformation = kinect.GetCalibration().CreateTransformation();
  }

  void InitMesh()
  {
    int width = kinect.GetCalibration().DepthCameraCalibration.ResolutionWidth;
    int height = kinect.GetCalibration().DepthCameraCalibration.ResolutionHeight;

    num = width * height;
    mesh = new Mesh();
    mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

    vertices = new Vector3[num];
    colors = new Color32[num];
    indices = new int[num];

    //Initialization of index list
    for (int i = 0; i < num; i++)
    {
      indices[i] = i;
    }


    //Allocate a list of point coordinates, colors, and points to be drawn to mesh
    mesh.vertices = vertices;
    mesh.colors32 = colors;
    mesh.SetIndices(indices, MeshTopology.Points, 0);

    gameObject.GetComponent<MeshFilter>().mesh = mesh;
  }

  private async void KinectLoop()
  {
    while (true)
    {
      using (Capture capture = await Task.Run(() => kinect.GetCapture()).ConfigureAwait(true))
      {
        //Getting color information
        Image colorImage = transformation.ColorImageToDepthCamera(capture);
        BGRA[] colorPixels = colorImage.GetPixels<BGRA>().ToArray();

        //Getting vertices of point cloud
        Image depthImage = transformation.DepthImageToPointCloud(capture.Depth);
        Short3[] depthPixels = depthImage.GetPixels<Short3>().ToArray();

        for (int i = 0; i < num; i++)
        {
          if (depthPixels[i].Z > visibleTo || depthPixels[i].Z < visbleFrom)
          {
            vertices[i].x = 0;
            vertices[i].y = 0;
            vertices[i].z = 0;
            colors[i].a = 0;
            continue;
          }

          vertices[i].x = depthPixels[i].X * 0.01f;
          vertices[i].y = -depthPixels[i].Y * 0.01f;
          vertices[i].z = depthPixels[i].Z * 0.01f;

          colors[i].b = colorPixels[i].B;
          colors[i].g = colorPixels[i].G;
          colors[i].r = colorPixels[i].R;
          colors[i].a = 255;
        }

        if (mesh == null) return;

        mesh.vertices = vertices;
        mesh.colors32 = colors;

        mesh.RecalculateBounds();


      }
    }
  }

  void OnDestroy()
  {
    kinect.StopCameras();
  }
}
