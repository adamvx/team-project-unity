using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storeName : MonoBehaviour
{

    public static string theName;
    public static string theLink;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void readStringNameInput(string s)
    {
        theName = s;

    }
    public void readStringLinkInput(string link)
    {
        theLink = link;

    }

}
