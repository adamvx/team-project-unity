using UnityEngine;

public class ToggleMenu : MonoBehaviour
{

  public GameObject panel;
  void Start()
  {
    panel.SetActive(false);
  }
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Tab))
    {
      panel.SetActive(true);
    }

    else if (Input.GetKeyUp(KeyCode.Tab))
    {
      panel.SetActive(false);
    }
  }
}
