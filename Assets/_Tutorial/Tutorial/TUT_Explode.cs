using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_Explode : MonoBehaviour
{
    void Update()
    {
        ExplodeMe();
    }
    void ExplodeMe()
    {
        Color temp = GetComponent<SpriteRenderer>().color;

        if (temp.a > 0)
        {
            transform.localScale *= (1.1f + (1 - temp.a));
            temp.a -= 0.2f;
            GetComponent<SpriteRenderer>().color = temp;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
