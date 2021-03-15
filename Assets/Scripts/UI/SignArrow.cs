using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignArrow : MonoBehaviour
{
    float BOUND = 0.25f;
    float STEP = 0.02f;
    float originalY;

    void Start()
    {
        originalY = transform.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        FloatY();
    }

    void FloatY()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + STEP, transform.position.z);
        if(transform.position.y < originalY - BOUND || transform.position.y > originalY + BOUND)
        {
            STEP *= -1.0f;
        }
    }
}
