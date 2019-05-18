using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TUT_TutorialManager : MonoBehaviour
{
    //misc
    private GameObject plyr;
    private TUT_PlayerController pC;
    private GameObject blueEnemy;
    private GameObject yellowEnemy;
    private Vector3 plyrPos;
    private GameObject[] firstBlockCapture;
    private GameObject[] blueBlockCapture;
    private GameObject[] yellowBlockCapture;
    private int step = -10;
    private int counter;
    private int counter1;
    private string blueMoveDir = "up";
    private string yellowMoveDir = "down";
    private string yellowNextMoveDir = "none";
    private bool block1IsCaptured;
    private bool block2IsCaptured;
    private bool block3IsCaptured;
    private Vector3 yellowStopsAt;
    public int sceneToLoad;
    public GameObject exitEffectPS_prefab;
    private GameObject exitEffectPS;
    private Color fadeToPlayerRed;
    private float fadeSpeed = 0.1f;
    bool freezeYellow;

    //colors
    private Color playerRed = 			new Color(1f	, 0f	, 0.09f	, 1);
	private Color enemyBlue =           new Color(0f    , 0.33f , 1f    , 1);
	private Color enemyYellow =         new Color(1f    , 0.86f , 0f    , 1);

	private Color playerTrailRed =      new Color(0.67f , 0.12f , 0.11f , 1);
    private Color enemyTrailBlue = 		new Color(0.12f	, 0.25f	, 0.60f	, 1);
    private Color enemyTrailYellow = 	new Color(0.93f	, 0.73f	, 0f	, 1);

    //for capture effect
    public GameObject captureEffectPrefab;
    private GameObject captureEffect;
    private Vector3 capturePos1 = new Vector3(-15.5f, 1.5f, 0);
    private Vector3 capturePos2 = new Vector3(-9.5f, 1.5f, 0);
    private Vector3 capturePos3 = new Vector3(5.5f, 1.5f, 0);

    //for blue enemy trails
    public GameObject blueTrailPrefab;
    private GameObject blueTrailObj;
    public List<GameObject> blueTrailObjList = new List<GameObject>();

    //for yellow enemy trails
    public GameObject yellowTrailPrefab;
    private GameObject yellowTrailObj;
    public List<GameObject> yellowTrailObjList = new List<GameObject>();

    //for anim
    private GameObject helper;
    public GameObject bubblePrefab;
    private GameObject bubble;
    private Vector3 bubbleOffset = new Vector3(6.7f, 5f, 0f);
    private Vector3 helperPos = new Vector3(-30, -18, 0);
    private Vector3 helperPosHidden;
    private Animator helperAnim;
    public GameObject arrowsPrefab;
    private GameObject arrows;
    public GameObject surroundPrefab;
    private GameObject surround;
    private bool hideHelper;
    private int helperCounter;

    void Start()
    {
        plyr = GameObject.Find("TUT_Player");
        pC = plyr.GetComponent<TUT_PlayerController>();
        firstBlockCapture = GameObject.FindGameObjectsWithTag("Capture 1");
        blueEnemy = GameObject.Find("TUT_Enemy (B)");
        blueBlockCapture = GameObject.FindGameObjectsWithTag("Capture 2");
        yellowEnemy = GameObject.Find("TUT_Enemy (Y)");
        yellowBlockCapture = GameObject.FindGameObjectsWithTag("Capture 3");
        helper = GameObject.Find("TUT_helper");
        helperAnim = helper.GetComponent<Animator>();
        helperPosHidden = helper.transform.position;
    }

    void Update()
    {
        GetPlayerPos();
        Steps();
        CheckOptionalCapture();
        WhenHideHelperTrue();
    }

    void Steps()
    {
        if (step == -10)
        {
            if (helper.transform.position.y < helperPos.y)
            {
                helper.transform.transform.Translate(Vector3.up * 0.02f);
            }
            else
            {
                helper.transform.position = helperPos;
                counter++;
                if (counter > 120)
                {
                    if (bubble == null)
                    {
                        bubble = Instantiate(bubblePrefab, helper.transform.position + bubbleOffset, Quaternion.identity);
                    }
                }
                if (counter > 150)
                {
                    counter = 0;
                    step = -9;
                }
            }
        }
        if (step == -9)
        {
            helperAnim.SetInteger("AnimState", 1);
            arrows = Instantiate(arrowsPrefab, bubble.transform.position, Quaternion.identity);
            step = -8;
        }
        if (step == -8)
        {
            if (plyrPos == new Vector3(-13, -3, 0))
            {
                if (arrows != null)
                {
                    Destroy(arrows);
                }
                if (surround == null)
                {
                    surround = Instantiate(surroundPrefab, bubble.transform.position, Quaternion.identity);
                }

                step = 1;
            }
        }

        if (step == 1)
        {
            BlueEnemyCapturesArea2();
        }
        if (step == 2)
        {
            BlueEnemyCapturesArea2();
            FlashCapture1();
            CheckForCapture1();
        }
        if (step == 3)
        {
            BlueEnemyGoesToNextPos();
        }
        if (step == 4)
        {
            FlashCapture2();
            CheckForCapture2();
        }
        if (step == 5)
        {

        }
        if (step == 6)
        {
            BlueEnemyGoesToNextPos();
        }
        if (step == 7)
        {
            YellowEnemyGoesToNextPos();
            CheckExit();
        }
        if (step == 8)
        {
            ExitEffectPart2();

            //Tutorial is over
            Invoke("LoadGame", 5);
        }

        //print("step = " + step);
    }

    void SetHideHelperTrue()
    {
        hideHelper = true;
    }

    void WhenHideHelperTrue()
    {
        if (helper != null && hideHelper == true)
        {
            if (helper.transform.position.y > helperPosHidden.y)
            {
                helper.transform.transform.Translate(Vector3.down * 0.02f);
            }
            else
            {
                Destroy(helper);
            }
        }
    }

    void CheckOptionalCapture()
    {
        CheckForCapture3();
    }

    void GetPlayerPos()
    {
        if (plyr != null)
        {
            plyrPos = plyr.transform.position;
        }
    }

    void FlashCapture1()
    {
        counter1++;
        if (counter1 > 30)
        {
            counter1 = 0;
            Color temp = firstBlockCapture[0].GetComponent<SpriteRenderer>().color;

            if (temp.a != 0)
            {
                temp.a = 0;

                for (int i = 0; i < firstBlockCapture.Length; i++)
                {
                    firstBlockCapture[i].GetComponent<SpriteRenderer>().color = temp;
                }
            }
            else
            {
                temp.a = 0.5f;

                for (int i = 0; i < firstBlockCapture.Length; i++)
                {
                    firstBlockCapture[i].GetComponent<SpriteRenderer>().color = temp;
                }
            }
        }
    }

    void FlashCapture2()
    {
        counter++;
        if (counter > 30)
        {
            counter = 0;
            Color temp = blueBlockCapture[0].GetComponent<SpriteRenderer>().color;

            if (temp == enemyBlue)
            {
                temp = playerTrailRed;
                temp.a = 0.5f;

                for (int i = 0; i < blueBlockCapture.Length; i++)
                {
                    blueBlockCapture[i].GetComponent<SpriteRenderer>().color = temp;
                }
            }
            else
            {
                temp = enemyBlue;
                temp.a = 1f;

                for (int i = 0; i < blueBlockCapture.Length; i++)
                {
                    blueBlockCapture[i].GetComponent<SpriteRenderer>().color = temp;
                }
            }
        }
    }

    void CheckForCapture1()
    {
        if (block1IsCaptured == false)
        {
            if (pC.trailObjList.Count > 0)
            {
                if (plyrPos == pC.trailObjList[0].transform.position)
                {
                    counter = 0;
                    captureEffect = Instantiate(captureEffectPrefab, capturePos1, Quaternion.identity);

                    for (int i = 0; i < firstBlockCapture.Length; i++)
                    {
                        firstBlockCapture[i].GetComponent<SpriteRenderer>().color = playerRed;
                    }
                    block1IsCaptured = true;

                    step = 3;
                }
            }
        }
    }

    void CheckForCapture2()
    {
        if (block2IsCaptured == false)
        {
            if (pC.trailObjList.Count > 0)
            {
                if (plyrPos.x > -12 && plyrPos.x < -7)
                {
                    if (plyrPos == pC.trailObjList[0].transform.position)
                    {
                        counter = 0;
                        captureEffect = Instantiate(captureEffectPrefab, capturePos2, Quaternion.identity);

                        for (int i = 0; i < blueBlockCapture.Length; i++)
                        {
                            blueBlockCapture[i].GetComponent<SpriteRenderer>().color = playerRed;
                        }
                        block2IsCaptured = true;

                        //hidehelper
                        helperAnim.SetInteger("AnimState", 0);
                        Invoke("SetHideHelperTrue", 3);
                        if (bubble != null)
                        {
                            Destroy(bubble);
                            Destroy(surround);
                        }

                        step = 6;
                    }
                }
            }
        }
    }

    void CheckForCapture3()
    {
        if (block3IsCaptured == false)
        {
            if (pC.trailObjList.Count > 0)
            {
                if (plyrPos.x > 3 && plyrPos.x < 8)
                {
                    if (plyrPos == pC.trailObjList[0].transform.position)
                    {
                        counter = 0;
                        captureEffect = Instantiate(captureEffectPrefab, capturePos3, Quaternion.identity);

                        for (int i = 0; i < yellowBlockCapture.Length; i++)
                        {
                            yellowBlockCapture[i].GetComponent<SpriteRenderer>().color = playerRed;
                        }
                        block3IsCaptured = true;
                    }
                }
            }
        }
    }

    void BlueEnemyCapturesArea2()
    {
        BlueLogic();

        counter++;
        if (counter > pC.interval)
        {
            counter = 0;
            BlueSetNewPos();
        }
    }

    void BlueEnemyGoesToNextPos()
    {
        BlueLogic();

        counter++;
        if (counter > pC.interval)
        {
            counter = 0;
            BlueSetNewPos();
        }
    }

    void BlueLogic()
    {
        if (step == 1)
        {
            if (blueEnemy.transform.position == new Vector3(-11, 3, 0))
            {
                blueMoveDir = "right";
            }
            else if (blueEnemy.transform.position == new Vector3(-8, 3, 0))
            {
                blueMoveDir = "down";
            }
            else if (blueEnemy.transform.position == new Vector3(-8, 0, 0))
            {
                blueMoveDir = "left";
            }
            if (blueTrailObjList.Count > 0)
            {
                if (blueEnemy.transform.position == blueTrailObjList[0].transform.position)
                {
                    step = 2;

                    for (int i = 0; i < blueBlockCapture.Length; i++)
                    {
                        blueBlockCapture[i].GetComponent<SpriteRenderer>().color = enemyBlue;
                    }
                }
            }
        }
        if (step == 2)
        {
            if (blueEnemy.transform.position == new Vector3(-11, 0, 0))
            {
                blueMoveDir = "up";
            }
            if (blueEnemy.transform.position == new Vector3(-11, 3, 0))
            {
                blueMoveDir = "right";
            }
            if (blueEnemy.transform.position == new Vector3(-8, 3, 0))
            {
                blueMoveDir = "down";
            }
            if (blueEnemy.transform.position == new Vector3(-8, 0, 0))
            {
                blueMoveDir = "left";
            }
        }
        if (step == 3)
        {
            if (blueEnemy.transform.position == new Vector3(-8, 0, 0))
            {
                blueMoveDir = "left";
            }
            if (blueEnemy.transform.position == new Vector3(-11, 0, 0))
            {
                blueMoveDir = "up";
            }
            if (blueEnemy.transform.position == new Vector3(-11, 3, 0))
            {
                blueMoveDir = "right";
            }
            if (blueEnemy.transform.position == new Vector3(-6, 3, 0))
            {
                blueMoveDir = "none";
                Invoke("ClearBlueTrail", 0.2f);
                step = 4;
            }
        }
        if (step == 6)
        {
            if (blueEnemy.transform.position == new Vector3(-6, 3, 0))
            {
                blueMoveDir = "down";
            }
            if (blueEnemy.transform.position == new Vector3(-6, -3, 0))
            {
                blueMoveDir = "right";
            }
            if (blueEnemy.transform.position == new Vector3(-3, -3, 0))
            {
                blueMoveDir = "up";
            }
            if (blueEnemy.transform.position == new Vector3(-3, 3, 0))
            {
                blueMoveDir = "right";
            }
            if (blueEnemy.transform.position == new Vector3(-1, 3, 0))
            {
                blueMoveDir = "down";
            }
            if (blueEnemy.transform.position == new Vector3(-1, -3, 0))
            {
                blueMoveDir = "none";
                Invoke("ClearBlueTrail", 0.2f);
                step = 7;
            }
        }
    }

    void BlueSetNewPos()
    {
        SpawnBlueTrailHere();
        RemoveBlueTrailHere();

        if (blueMoveDir == "left")
        {
            blueEnemy.transform.Translate(Vector2.left);
        }
        else if (blueMoveDir == "right")
        {
            blueEnemy.transform.Translate(Vector2.right);
        }
        else if (blueMoveDir == "up")
        {
            blueEnemy.transform.Translate(Vector2.up);
        }
        else if (blueMoveDir == "down")
        {
            blueEnemy.transform.Translate(Vector2.down);
        }
    }

    void SpawnBlueTrailHere()
    {
        blueTrailObj = Instantiate(blueTrailPrefab, blueEnemy.transform.position, Quaternion.identity);
        blueTrailObjList.Add(blueTrailObj);
    }

    void RemoveBlueTrailHere()
    {
        if (blueTrailObjList.Count > 12)
        {
            Destroy(blueTrailObjList[0]);
            blueTrailObjList.Remove(blueTrailObjList[0]);
        }
    }

    void ClearBlueTrail()
    {
        GameObject[] destroyThese = GameObject.FindGameObjectsWithTag("Blue Enemy Trail");
        for (int i = 0; i < destroyThese.Length; i++)
        {
            Destroy(destroyThese[i]);
        }
        blueTrailObjList.Clear();
    }

    void YellowEnemyGoesToNextPos()
    {
        if (plyrPos == new Vector3(12, -2, 0))
        {
            freezeYellow = true;
        }
        if (freezeYellow == false)
        {
            YellowLogic();

            counter++;
            if (counter > pC.interval)
            {
                counter = 0;
                YellowSetNewPos();
            }
        }
        else
        {
            yellowMoveDir = "none";
            ClearYellowTrail();
        }
    }

    void YellowLogic()
    {
        yellowStopsAt = new Vector3(12, -3, 0);

        if (plyrPos == new Vector3(12, -3, 0))
        {
            yellowStopsAt = new Vector3(12, -2, 0);
        }

        for (int i = 0; i < pC.trailObjList.Count; i++)
        {
            if (pC.trailObjList[i] != null)
            {
                if (pC.trailObjList[i].transform.position == new Vector3(12, -3, 0))
                {
                    yellowStopsAt = new Vector3(12, -2, 0);
                }
            }
        }

        if (yellowEnemy.transform.position == yellowStopsAt)
        {
            if (yellowMoveDir == "down")
            {
                yellowNextMoveDir = "up";
                yellowMoveDir = "none";
                Invoke("ClearYellowTrail", 0.2f);
                Invoke("YellowTurnAround", 2);
            }
        }
        else if (yellowEnemy.transform.position == new Vector3(12, 3, 0))
        {
            if (yellowMoveDir == "up")
            {
                yellowNextMoveDir = "down";
                yellowMoveDir = "none";
                Invoke("ClearYellowTrail", 0.2f);
                Invoke("YellowTurnAround", 1);
            }
        }
    }

    void YellowSetNewPos()
    {
        SpawnYellowTrailHere();
        RemoveYellowTrailHere();

        if (yellowMoveDir == "up")
        {
            yellowEnemy.transform.Translate(Vector2.up);
        }
        else if (yellowMoveDir == "down")
        {
            yellowEnemy.transform.Translate(Vector2.down);
        }
    }

    void SpawnYellowTrailHere()
    {
        yellowTrailObj = Instantiate(yellowTrailPrefab, yellowEnemy.transform.position, Quaternion.identity);
        yellowTrailObjList.Add(yellowTrailObj);
    }

    void RemoveYellowTrailHere()
    {
        if (yellowTrailObjList.Count > 6)
        {
            Destroy(yellowTrailObjList[0]);
            yellowTrailObjList.Remove(yellowTrailObjList[0]);
        }
    }

    void ClearYellowTrail()
    {
        GameObject[] destroyThese = GameObject.FindGameObjectsWithTag("Yellow Enemy Trail");
        for (int i = 0; i < destroyThese.Length; i++)
        {
            Destroy(destroyThese[i]);
        }
        yellowTrailObjList.Clear();
    }

    void YellowTurnAround()
    {
        yellowMoveDir = yellowNextMoveDir;
    }

    void CheckExit()
    {
        if (plyrPos == new Vector3(19, 5, 0))
        {
            ExitEffectPart1();
        }
    }

    void ExitEffectPart1()
    {
        //exitEffectPS = Instantiate(exitEffectPS_prefab, plyrPos, Quaternion.identity);
        //Destroy(exitEffectPS, 5);

        GameObject[] roadBlocks = GameObject.FindGameObjectsWithTag("Road");
        GameObject[] boundaryBlocks = GameObject.FindGameObjectsWithTag("Boundary");

        for (int i = 0; i < boundaryBlocks.Length; i++)
        {
            Destroy(boundaryBlocks[i]);
        }
        for (int i = 0; i < roadBlocks.Length; i++)
        {
            roadBlocks[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
        for (int i = 0; i < blueBlockCapture.Length; i++)
        {
            Destroy(blueBlockCapture[i]);
        }
        for (int i = 0; i < yellowBlockCapture.Length; i++)
        {
            Destroy(yellowBlockCapture[i]);
        }
        for (int i = 0; i < pC.trailObjList.Count; i++)
        {
            Destroy(pC.trailObjList[i]);

        }
        for (int i = 0; i < blueTrailObjList.Count; i++)
        {
            Destroy(blueTrailObjList[i]);
        }
        for (int i = 0; i < yellowTrailObjList.Count; i++)
        {
            Destroy(yellowTrailObjList[i]);
        }
        for (int i = 0; i < firstBlockCapture.Length; i++)
        {
            Destroy(firstBlockCapture[i]);
        }

        Destroy(GameObject.Find("TUT_Player"));
        Destroy(GameObject.Find("TUT_Enemy (B)"));
        Destroy(GameObject.Find("TUT_Enemy (Y)"));

        GameObject.Find("TUT_Main Camera").GetComponent<Camera>().backgroundColor = new Color(0, 0, 0, 1);
        fadeToPlayerRed = new Color(1, 1, 1, 1);
        step = 8;
    }

    void ExitEffectPart2()
    {
        fadeToPlayerRed = Color.Lerp(fadeToPlayerRed, playerRed, fadeSpeed);
        GameObject[] roadBlocks = GameObject.FindGameObjectsWithTag("Road");
        for (int i = 0; i < roadBlocks.Length; i++)
        {
            roadBlocks[i].GetComponent<SpriteRenderer>().color = fadeToPlayerRed;
        }
        fadeSpeed -= 0.01f;
        if (fadeSpeed < 0.025f)
        {
            fadeSpeed = 0.025f;
        }
    }

    void LoadGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
