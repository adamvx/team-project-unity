using UnityEngine;
using TMPro;
public class DisplayName : MonoBehaviour
{
  public string nameValue;
  public string linkValue;
  public TextMeshPro textElement;
  public TextMeshProUGUI UIName;
  public TextMeshProUGUI UILink;

  // Update is called once per frame
  void Start()
  {
    textElement.text = Storage.userName;
    UIName.text = Storage.userName;
    UILink.text = Storage.link;
  }
}
