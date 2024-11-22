using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized*10;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
