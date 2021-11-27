
using UnityEngine;
using UnityEngine.UI;
public class MicrophoneButton : MonoBehaviour
{
  public Sprite onImage;
  public Sprite offImage;
  Button button;
  bool clicked = true;

  void Start()
  {
    button = GetComponent<Button>();
    button.onClick.AddListener(OnToggle);
  }
  public void OnToggle()
  {
    if (Storage.isMicrophoneEnabled == true)
    {
      button.image.sprite = offImage;
      Storage.isMicrophoneEnabled = false;
    }
    else
    {
      button.image.sprite = onImage;
      Storage.isMicrophoneEnabled = true;
    }

  }
}
