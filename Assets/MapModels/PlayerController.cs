using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int stepSize;
    public int jumpSize;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position += transform.forward * stepSize;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -90f);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position += transform.forward * -stepSize;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Rotate(Vector3.up, 90f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position += transform.up * jumpSize;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            transform.position += transform.up * -jumpSize;
        }
    }
}
