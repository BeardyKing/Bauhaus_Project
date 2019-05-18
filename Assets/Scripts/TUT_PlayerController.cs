using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_PlayerController : MonoBehaviour
{
    private GameObject checker;
    private TUT_CheckerScript cS;
    public string moveDir = "none";
    public string prevDir = "initial";
    public bool hasInput;
    private int counter;
    public int interval = 10;
    public GameObject trailPrefab;
    private GameObject trailObj;
    public List<GameObject> trailObjList = new List<GameObject>();
    private GameObject mainCam;
    private TUT_ShakeBehaviour sB;

    void Start()
    {
        checker = GameObject.Find("TUT_Checker");
        cS = checker.GetComponent<TUT_CheckerScript>();
        mainCam = GameObject.Find("TUT_Main Camera");
        sB = mainCam.GetComponent<TUT_ShakeBehaviour>();
    }

    void Update()
    {
        Interval();
    }

    void Interval()
    {
        if (moveDir != "none")
        {
            counter++;
            if (counter > interval)
            {
                counter = 0;
                SetNewPos();

                hasInput = false;
            }
        }
        else
        {
            counter++;
            if(counter > interval)
            {
                counter = 0;
                ClearTrail();

                hasInput = false;
            }
        }
        //print("moveDir = " + moveDir + "    prevDir = " + prevDir);
    }

    void SetNewPos()
    {
        SpawnTrailHere();
        RemoveTrailHere();

        prevDir = moveDir;

        if (moveDir == "left")
        {
            transform.Translate(Vector2.left);
        }
        else if (moveDir == "right")
        {
            transform.Translate(Vector2.right);
        }
        else if (moveDir == "up")
        {
            transform.Translate(Vector2.up);
        }
        else if (moveDir == "down")
        {
            transform.Translate(Vector2.down);
        }

        hasInput = false;
    }

    void SpawnTrailHere()
    {
        trailObj = Instantiate(trailPrefab, transform.position, Quaternion.identity);
        trailObjList.Add(trailObj);
        //print("count = " + trailObjList.Count);
    }

    void RemoveTrailHere()
    {
        if(trailObjList.Count > 12)
        {
            Destroy(trailObjList[0]);
            trailObjList.Remove(trailObjList[0]);
        }
    }

    public void ClearTrail()
    {
        if (trailObjList.Count > 0)
        {
            sB.TriggerShake();
            GameObject[] destroyThese = GameObject.FindGameObjectsWithTag("Trail");
            for (int i = 0; i < destroyThese.Length; i++)
            {
                Destroy(destroyThese[i]);
            }
            trailObjList.Clear();
        }
    }
}
