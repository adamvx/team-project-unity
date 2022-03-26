using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
  public float sensX;
  public float sensY;
  public Transform orientation;
  float xRotation = 0f;
  float yRotation = 0f;

  void Start()
  {
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  void Update()
  {
    float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
    float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;

    yRotation += mouseX;
    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    orientation.rotation = Quaternion.Euler(0, yRotation, 0);
  }
}
