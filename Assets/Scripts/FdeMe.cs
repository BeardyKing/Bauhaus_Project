using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FdeMe : MonoBehaviour{
    int counter;
    void Update(){
        if(StaticData.GameRunning == true){
            counter++;
            if (counter > 8){
                GetComponent<SpriteRenderer>().color = 
                    Vector4.Lerp(
                        GetComponent<SpriteRenderer>().color,  
                        Vector4.zero, 
                        1 * Time.deltaTime);
                if (GetComponent<SpriteRenderer>().color.a < 0.01f){
                    Destroy(gameObject);
                }
            }
        }
    }
}
