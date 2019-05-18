using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_ShakeBehaviour : MonoBehaviour
{
    private Transform myTransform;
    private float magnitude = 0f;
    private float decrement;
    public float initial_decrement;
    public float maxM;
    private Vector3 initialPosistion;

    private void Awake()
    {
        initialPosistion = transform.position;
    }

    void Update()
    {
        if(magnitude > 0)
        {
            transform.position = initialPosistion + Random.insideUnitSphere * magnitude;
            magnitude -= Time.deltaTime * decrement;
            decrement -= Time.deltaTime * magnitude;
        }
        else
        {
            magnitude = 0f;
            transform.position = initialPosistion;
        }
    }

    public void TriggerShake()
    {
        initialPosistion = transform.position;
        magnitude = maxM;
        decrement = initial_decrement;
    }
}
