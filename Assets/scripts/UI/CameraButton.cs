using UnityEngine;
using UnityEngine.UI;
public class CameraButton : MonoBehaviour
{
  public Sprite onImage;
  public Sprite offImage;
  Button button;

  public void Start()
  {
    button = GetComponent<Button>();
    button.onClick.AddListener(OnToggle);
  }
  public void OnToggle()
  {
    if (Storage.isCameraEnabled == true)
    {
      button.image.sprite = offImage;
      Storage.isCameraEnabled = false;
    }
    else
    {
      button.image.sprite = onImage;
      Storage.isCameraEnabled = true;
    }

  }
}
