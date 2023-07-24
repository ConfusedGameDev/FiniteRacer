using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arcadeCarController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component of the ball.
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Calculate the forward movement force (along the local Z-axis).
        Vector3 movement = transform.forward * moveSpeed;

        // Apply the movement force to the Rigidbody.
        rb.AddForce(movement, ForceMode.Force);
    }
}
