using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class displayName : MonoBehaviour
{
     public string nameValue;
    public TextMeshPro textElement;
    public TextMeshProUGUI UIName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nameValue = storeName.theName;
        textElement.text = nameValue;
        UIName.text = nameValue;
    }
}
