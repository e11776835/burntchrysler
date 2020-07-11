    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed, startSpeed, accelerationFactor, maxSpeed;
    Rigidbody myRigidBody;
    Vector3 change, prevChange;

    // Start is called before the first frame update
    void Start()
    {
        speed = startSpeed;
        myRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        prevChange = change;
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.z = Input.GetAxisRaw("Vertical");

        if (change != Vector3.zero)
        {
            Move();
        }
    }

    private void Move()
    {
        speed *= accelerationFactor;
        if (speed > maxSpeed) speed = maxSpeed;

        myRigidBody.MovePosition(
            transform.position + change.normalized * speed * Time.deltaTime
        );
    }
}
