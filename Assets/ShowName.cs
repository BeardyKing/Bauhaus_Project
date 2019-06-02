using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowName : MonoBehaviour
{
    public TextMesh tm;
    int count = 5;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (count < 0) {

        }
        if (count < -1) {
            tm.text = gameObject.name;
        }
        if (count > -2) {
            count--;       
        }
    }
}
