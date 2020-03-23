using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticles : MonoBehaviour
{
    private PlayerManager pm;
    private GameObject triangle;
    private Vector3 endOfTrailPos;
    public GameObject particlePrefab;
    private GameObject particle;

    void Start()
    {
        pm = FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        triangle = GameObject.Find("Triangle");
    }

    void Update()
    {
        if (pm.PlayerPosition != Vector2Int.zero){
            //testing
            GetEndOfTrailPos();
            Debug();
            //print("length = " + pm.MaxLength);
        }
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
        //triangle.transform.localScale = new Vector3(3, 3, 1);
        if (particle == null)
        {
            particle = (Instantiate(particlePrefab, endOfTrailPos, Quaternion.identity));
            Destroy(particle, 0.4f);
        }
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
