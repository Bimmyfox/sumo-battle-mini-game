using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody rb;
    private float forwardInput;
    private GameObject focalPoint;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();    
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        forwardInput = Input.GetAxis("Vertical");
        rb.AddForce(focalPoint.transform.forward * speed * forwardInput);
    }
}
