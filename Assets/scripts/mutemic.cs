
using UnityEngine;
using UnityEngine.UI;
public class mutemic: MonoBehaviour
{
    public Sprite newbttIMG;
    public Sprite oldbttIMG;
    public Button buton;
    bool clicked = true;
    public void ChangeButtonImage()
    {
        if (clicked == true)
        {
            buton.image.sprite = newbttIMG;
            clicked = !clicked;
        }
        else if (clicked == false)
        {
            buton.image.sprite = oldbttIMG;
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
