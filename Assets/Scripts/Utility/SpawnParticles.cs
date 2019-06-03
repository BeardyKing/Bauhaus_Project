using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticles : MonoBehaviour
{
    private PlayerManager pm;
    private GameObject triangle;
    private Vector3 endOfTrailPos;

    void Start()
    {
        pm = FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        triangle = GameObject.Find("Triangle");
    }

    void Update()
    {
        //testing
        GetEndOfTrailPos();
        Debug();
    }

    void GetEndOfTrailPos()
    {
        endOfTrailPos.x = pm.allPositions[pm.length - 1][1];
        endOfTrailPos.y = -pm.allPositions[pm.length - 1][0];
        endOfTrailPos.z = -1;
    }

    public void PlayerMakesValidTurn()
    {
        //debug
        triangle.transform.localScale = new Vector3(3, 3, 1);
    }

    private void Debug()
    {
        triangle.transform.position = endOfTrailPos;
        if (triangle.transform.localScale.x > 1.2f)
        {
            triangle.transform.localScale *= 0.8f;
        }
        else
        {
            triangle.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
