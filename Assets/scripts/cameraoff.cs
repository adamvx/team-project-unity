using UnityEngine;
using UnityEngine.UI;
public class cameraoff : MonoBehaviour
{
    public Sprite newcamIMG;
    public Sprite oldcamIMG;
    public Button buton;
    bool clicked = true;
    public void ChangecameraImage()
    {
        if (clicked == true)
        {
            buton.image.sprite = newcamIMG;
            clicked = !clicked;
        }
        else if (clicked == false)
        {
            buton.image.sprite = oldcamIMG;
            clicked = !clicked;
        }

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
