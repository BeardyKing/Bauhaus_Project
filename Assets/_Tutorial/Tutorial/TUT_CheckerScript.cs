using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_CheckerScript : MonoBehaviour
{
    private GameObject plyr;
    private GameObject bufferChecker;
    private TUT_PlayerController pC;
    private Vector3 offset;
    private Vector3 bufferOffset;
    private GameObject[] boundaryPositions;
    private GameObject[] roadPositions;
    public GameObject[] enemies;
    private string bufferDir = "none";
    private bool bufferActive;
    private Vector3 prevOffset;
    private Vector3 bufferHoldingPosition = new Vector3(100, 100, 0);
    private GameObject tutorialManagerObj;
    private TUT_TutorialManager tM;

    [Range(0.05f, 1f)]
    public float bufferLength = 0.1f;

    void Start()
    {
        plyr = GameObject.Find("TUT_Player");
        pC = plyr.GetComponent<TUT_PlayerController>();
        bufferChecker = GameObject.Find("TUT_BufferChecker");
        boundaryPositions = GameObject.FindGameObjectsWithTag("Boundary");
        roadPositions = GameObject.FindGameObjectsWithTag("Road");
        bufferChecker.transform.position = bufferHoldingPosition;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        tutorialManagerObj = GameObject.Find("TUT_TutorialManagerObj");
        tM = tutorialManagerObj.GetComponent<TUT_TutorialManager>();
    }

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (plyr != null)
        {
            if (pC.hasInput == false)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    if ((pC.prevDir != "right") || (pC.trailObjList.Count == 0))
                    {
                        pC.hasInput = true;
                        pC.moveDir = "left";
                        bufferDir = "left";
                        bufferActive = true;
                        Invoke("ResetBufferActive", bufferLength);

                        //send checker ///////////////////////////////////////////////////////////
                        prevOffset = offset;
                        offset = new Vector3(-1, 0, 0);
                        bufferOffset = offset;
                        transform.position = plyr.transform.position + offset;
                        bufferChecker.transform.position = plyr.transform.position + bufferOffset;
                        //////////////////////////////////////////////////////////////////////////

                        for (int i = 0; i < boundaryPositions.Length; i++)
                        {
                            if (transform.position == boundaryPositions[i].transform.position)
                            {
                                if (pC.prevDir == "left" || pC.prevDir == "none")
                                {
                                    pC.moveDir = "none";
                                }
                                else
                                {
                                    //ignore input;
                                    pC.moveDir = pC.prevDir;
                                    offset = prevOffset;
                                }
                            }
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    if ((pC.prevDir != "left") || (pC.trailObjList.Count == 0))
                    {
                        pC.hasInput = true;
                        pC.moveDir = "right";
                        bufferDir = "right";
                        bufferActive = true;
                        Invoke("ResetBufferActive", bufferLength);

                        //send checker ///////////////////////////////////////////////////////////
                        prevOffset = offset;
                        offset = new Vector3(1, 0, 0);
                        bufferOffset = offset;
                        transform.position = plyr.transform.position + offset;
                        bufferChecker.transform.position = plyr.transform.position + bufferOffset;
                        //////////////////////////////////////////////////////////////////////////

                        for (int i = 0; i < boundaryPositions.Length; i++)
                        {
                            if (transform.position == boundaryPositions[i].transform.position)
                            {
                                if (pC.prevDir == "right" || pC.prevDir == "none")
                                {
                                    pC.moveDir = "none";
                                }
                                else
                                {
                                    //ignore input;
                                    pC.moveDir = pC.prevDir;
                                    offset = prevOffset;
                                }
                            }
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    if ((pC.prevDir != "down") || (pC.trailObjList.Count == 0))
                    {
                        pC.hasInput = true;
                        pC.moveDir = "up";
                        bufferDir = "up";
                        bufferActive = true;
                        Invoke("ResetBufferActive", bufferLength);

                        //send checker ///////////////////////////////////////////////////////////
                        prevOffset = offset;
                        offset = new Vector3(0, 1, 0);
                        bufferOffset = offset;
                        transform.position = plyr.transform.position + offset;
                        bufferChecker.transform.position = plyr.transform.position + bufferOffset;
                        //////////////////////////////////////////////////////////////////////////

                        for (int i = 0; i < boundaryPositions.Length; i++)
                        {
                            if (transform.position == boundaryPositions[i].transform.position)
                            {
                                if (pC.prevDir == "up" || pC.prevDir == "none")
                                {
                                    pC.moveDir = "none";
                                }
                                else
                                {
                                    //ignore input;
                                    pC.moveDir = pC.prevDir;
                                    offset = prevOffset;
                                }
                            }
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    if ((pC.prevDir != "up") || (pC.trailObjList.Count == 0))
                    {
                        pC.hasInput = true;
                        pC.moveDir = "down";
                        bufferDir = "down";
                        bufferActive = true;
                        Invoke("ResetBufferActive", bufferLength);

                        //send checker ///////////////////////////////////////////////////////////
                        prevOffset = offset;
                        offset = new Vector3(0, -1, 0);
                        bufferOffset = offset;
                        transform.position = plyr.transform.position + offset;
                        bufferChecker.transform.position = plyr.transform.position + bufferOffset;
                        //////////////////////////////////////////////////////////////////////////

                        for (int i = 0; i < boundaryPositions.Length; i++)
                        {
                            if (transform.position == boundaryPositions[i].transform.position)
                            {
                                if (pC.prevDir == "down" || pC.prevDir == "none")
                                {
                                    pC.moveDir = "none";
                                }
                                else
                                {
                                    //ignore input;
                                    pC.moveDir = pC.prevDir;
                                    offset = prevOffset;
                                }
                            }
                        }
                    }
                }
            }

            //move checker ///////////////////////////////////////
            if (plyr != null)
            {
                transform.position = plyr.transform.position + offset;
            }
            //////////////////////////////////////////////////////

            for (int i = 0; i < boundaryPositions.Length; i++)
            {
                if (boundaryPositions[i] != null)
                {
                    if (transform.position == boundaryPositions[i].transform.position)
                    {
                        pC.moveDir = "none";
                    }
                }
            }

            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                {
                    if (transform.position == enemies[i].transform.position)
                    {
                        pC.moveDir = "none";
                        pC.prevDir = "none";
                        bufferActive = false;
                        //sB.TriggerShake();
                    }
                }
            }

            for (int i = 0; i < tM.blueTrailObjList.Count; i++)
            {
                if (tM.blueTrailObjList[i] != null)
                {
                    if (transform.position == tM.blueTrailObjList[i].transform.position)
                    {
                        pC.moveDir = "none";
                        pC.prevDir = "none";
                        bufferActive = false;
                        //sB.TriggerShake();
                    }
                }
            }

            for (int i = 0; i < tM.yellowTrailObjList.Count; i++)
            {
                if (tM.yellowTrailObjList[i] != null)
                {
                    if (transform.position == tM.yellowTrailObjList[i].transform.position)
                    {
                        pC.moveDir = "none";
                        pC.prevDir = "none";
                        bufferActive = false;
                        //sB.TriggerShake();
                    }
                }
            }

            if (bufferActive == true)
            {
                bufferChecker.transform.position = plyr.transform.position + bufferOffset;

                for (int i = 0; i < roadPositions.Length; i++)
                {
                    if (bufferChecker.transform.position == roadPositions[i].transform.position)
                    {
                        pC.moveDir = bufferDir;
                        transform.position = plyr.transform.position;
                        offset = bufferOffset;
                        bufferChecker.transform.position = bufferHoldingPosition;
                    }
                }
            }
            else
            {
                bufferChecker.transform.position = bufferHoldingPosition;
                bufferDir = "none";
            }
        }
    }

    void ResetBufferActive()
    {
        bufferActive = false;
        bufferDir = "none";
    }
}
