using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oxygen : MonoBehaviour
{
    float BOUND = 0.25f;
    float STEP = 0.02f;
    float originalY;

    Collider m_collider;
    GameObject player;

    void Start()
    {
        player = GameObject.Find("FallGuy");
        originalY = transform.position.y;
        m_collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        FloatY();
    }

    void FloatY()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + STEP, transform.position.z);
        if (transform.position.y < originalY - BOUND || transform.position.y > originalY + BOUND)
        {
            STEP *= -1.0f;
        }
    }
    
}
