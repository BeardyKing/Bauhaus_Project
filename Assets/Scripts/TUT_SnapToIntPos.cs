using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_SnapToIntPos : MonoBehaviour
{
    private Vector3 currentPos;
    private Vector3 snappedPos;

    private void Awake()
    {
        currentPos = transform.position;
        snappedPos.x = Mathf.RoundToInt(currentPos.x);
        snappedPos.y = Mathf.RoundToInt(currentPos.y);
        snappedPos.z = 0;
        transform.position = snappedPos;
    }
}
