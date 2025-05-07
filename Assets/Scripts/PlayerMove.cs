using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour
{
    private float speed = 5f;
    public VariableJoystick joy;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        // float h = joy.Horizontal;
        // float v = joy.Vertical;
        if (h != 0.0f || v != 0.0f)
        {
            Vector3 dir = h * Vector3.right + v* Vector3.forward;
            transform.rotation = Quaternion.LookRotation(dir);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            GetComponent<Animator>().SetBool("bMove",true);
        }
        else
        {
            GetComponent<Animator>().SetBool("bMove", false);
        }
    }
}
