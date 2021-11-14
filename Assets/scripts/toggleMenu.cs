using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleMenu : MonoBehaviour
{
    public GameObject pausemenuUI;

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            pausemenuUI.SetActive(true);
        }

        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            pausemenuUI.SetActive(false);
        }
    }
}
