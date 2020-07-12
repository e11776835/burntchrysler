using System;
using TMPro;
using UnityEngine;

public class NewPlayerController : MonoBehaviour
{
    [Tooltip("Min and max value of the player's position.z value ")]
    public Vector2 ZLocationMinMax;
    public float 
        Force, 
        BoostForce, 
        StartSpeed = 0, 
        MaxSpeed = 10, 
        MaxZSpeed = 10, 
        Friction;
    public KeyCode BoostKey;

    public Vector3 input;
    Rigidbody _rb;
    public Healthbar Fartbar;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.left * StartSpeed;
        Fartbar.health = 100;
    }

    void FixedUpdate()
    {
        ApplyFriction();
        input = new Vector3(
            -Input.GetAxisRaw("Horizontal"),
            0,
            -Input.GetAxisRaw("Vertical"));

        if (input != Vector3.zero)
        {
            Move(input, Input.GetKey(BoostKey));
        }
        RestrictZLocation();
        RestrictVelocity();
    }

    private void ApplyFriction()
    {
        Debug.LogWarning("Warning: This code is written at 4:20 am and is most likely completely retarded. pls rewrite");
        var newVel = _rb.velocity * (1 - Friction * Time.deltaTime);
        _rb.velocity = newVel;
    }

    private void Move(Vector3 direction, bool boost)
    {
        Debug.Log($"Moving: {direction}");
        _rb.AddForce(direction * Force * Time.deltaTime * (boost ? BoostForce : 1), ForceMode.Acceleration);
    }

    private void RestrictZLocation()
    {
        var pos = transform.position;
        pos.z = Mathf.Clamp(transform.position.z, ZLocationMinMax.x, ZLocationMinMax.y);
        transform.position =  pos;
    }

    private void RestrictVelocity()
    {
        Vector3 vel = _rb.velocity;
        Debug.Log($"Vel: {vel}");
        vel.x = Mathf.Clamp(vel.x, -MaxSpeed, 0);
        vel.z = Mathf.Clamp(vel.z, -MaxZSpeed, MaxZSpeed);
        _rb.velocity = vel;
    }
}