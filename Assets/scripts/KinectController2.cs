using UnityEngine;
using Microsoft.Azure.Kinect.Sensor;
using System.Threading.Tasks;
using System;
using System.IO;

public class KinectController2 : MonoBehaviour
{
  Device kinect;
  int depthWidth;
  int depthHeight;
  int num;
  Mesh mesh;
  Vector3[] vertices;
  Color32[] colors;
  int[] indeces;
  Texture2D texture;
  Transformation transformation;

  int nearClip = 1;
  public int rangeOfVisibility = 2200;

  void Start()
  {
    try
    {
      InitKinect();
      InitMesh();
      KinectLoop();
    }
    catch (Exception) { }

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
    //Get the width and height of the Depth image and calculate the number of all points
    depthWidth = kinect.GetCalibration().DepthCameraCalibration.ResolutionWidth;
    depthHeight = kinect.GetCalibration().DepthCameraCalibration.ResolutionHeight;
    num = depthWidth * depthHeight;

    //Instantiate mesh
    mesh = new Mesh();
    mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

    //Allocation of vertex and color storage space for the total number of pixels in the depth image
    vertices = new Vector3[num];
    colors = new Color32[num];
    texture = new Texture2D(depthWidth, depthHeight);
    Vector2[] uv = new Vector2[num];
    Vector3[] normals = new Vector3[num];
    indeces = new int[6 * (depthWidth - 1) * (depthHeight - 1)];

    //Initialization of uv and normal 
    int index = 0;
    for (int y = 0; y < depthHeight; y++)
    {
        for (int x = 0; x < depthWidth; x++)
        {
            uv[index] = new Vector2(((float)(x + 0.5f) / (float)(depthWidth)), ((float)(y + 0.5f) / ((float)(depthHeight))));
            normals[index] = new Vector3(0, -1, 0);
            index++;
        }
    }

    //Allocate a list of point coordinates, colors, and points to be drawn to mesh
    mesh.vertices = vertices;
    mesh.uv = uv;
    mesh.normals = normals;

    gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = texture;
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
        BGRA[] colorArray = colorImage.GetPixels<BGRA>().ToArray();

        //Getting vertices of point cloud
        Image depthImage = transformation.DepthImageToPointCloud(capture.Depth);
        Short3[] PointCloud = depthImage.GetPixels<Short3>().ToArray();

        int triangleIndex = 0;
        int pointIndex = 0;
        int topLeft, topRight, bottomLeft, bottomRight;
        int tl, tr, bl, br;
        for (int y = 0; y < depthHeight; y++)
        {
                    for (int x = 0; x < depthWidth; x++)
                    {

                        vertices[pointIndex].x = PointCloud[pointIndex].X * 0.01f;
                        vertices[pointIndex].y = -PointCloud[pointIndex].Y * 0.01f;
                        vertices[pointIndex].z = PointCloud[pointIndex].Z * 0.01f;

                        colors[pointIndex].a = 255;
                        colors[pointIndex].b = colorArray[pointIndex].B;
                        colors[pointIndex].g = colorArray[pointIndex].G;
                        colors[pointIndex].r = colorArray[pointIndex].R;

                        /*if (PointCloud[pointIndex].Z > rangeOfVisibility || PointCloud[pointIndex].Z == 0){
                            vertices[pointIndex].x = 0;
                            vertices[pointIndex].y = 0;
                            vertices[pointIndex].z = 0;
                            colors[pointIndex].a = 0;
                            continue;
                        }*/

                        if (x != (depthWidth - 1) && y != (depthHeight - 1))
                        {
                            topLeft = pointIndex;
                            topRight = topLeft + 1;
                            bottomLeft = topLeft + depthWidth;
                            bottomRight = bottomLeft + 1;
                            tl = PointCloud[topLeft].Z;
                            tr = PointCloud[topRight].Z;
                            bl = PointCloud[bottomLeft].Z;
                            br = PointCloud[bottomRight].Z;

                            if (tl > nearClip && tr > nearClip && bl > nearClip)
                            {
                                indeces[triangleIndex++] = topLeft;
                                indeces[triangleIndex++] = topRight;
                                indeces[triangleIndex++] = bottomLeft;
                            }
                            else
                            {
                                indeces[triangleIndex++] = 0;
                                indeces[triangleIndex++] = 0;
                                indeces[triangleIndex++] = 0;
                            }

                            if (bl > nearClip && tr > nearClip && br > nearClip)
                            {
                                indeces[triangleIndex++] = bottomLeft;
                                indeces[triangleIndex++] = topRight;
                                indeces[triangleIndex++] = bottomRight;
                            }
                            else
                            {
                                indeces[triangleIndex++] = 0;
                                indeces[triangleIndex++] = 0;
                                indeces[triangleIndex++] = 0;
                            }
                        }
                        pointIndex++;
                    }
                }

                texture.SetPixels32(colors);
                texture.Apply();

                mesh.vertices = vertices;

                mesh.triangles = indeces;
                mesh.RecalculateBounds();

                var result = Draco.Encoder.DracoEncoder.EncodeMesh(mesh);
                Debug.Log(result);

                var writePath = Path.Combine(Application.streamingAssetsPath, "res.drc.bytes");
                for (var submesh = 0; submesh < result.Length; submesh++)
                {
                    File.WriteAllBytes(writePath, result[submesh].data.ToArray());
                    result[submesh].Dispose();
                }
            }
    }
  }

  void OnDestroy()
  {
    try
    {
      kinect.StopCameras();
    }
    catch (Exception) { }

  }
}
