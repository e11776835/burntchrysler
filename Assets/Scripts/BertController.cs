using System;
using System.Collections;
using TMPro;
using UnityEngine;
/// <summary>
/// This script is attached to the player and controls the player's movement
/// </summary>
public class BertController : MonoBehaviour
{
    [Tooltip("Min and max value of the player's position.z value ")]
    public Vector2 ZLocationMinMax;
    public float
        Force,
        BoostForce,
        StartSpeed = 0,
        MaxSpeed = 10,
        BoostSpeedMaxSpeedMultiplier = 1.2f,
        MaxZSpeed = 10, 
        Friction = 10;
    public KeyCode BoostKey;
    private Vector3 _input;
    private Rigidbody _rb;
    [SerializeField] private Healthbar Fartbar;
    [SerializeField] private DrunkController DrunkController;

    public Healthbar FartJuice;
    public Healthbar FartCooldown;
    public float FartCooldownFactor;
    public int StartingFartJuice;

    // Start is called before the first frame update
    public static BertController Instance { get; private set; }
    private float _currentSpeed = 1;
    void Start()
    {

        if (Instance != null)
        {
            Debug.LogError("Too many berts in scene");
            return;
        }
        Instance = this;
        _rb = GetComponent<Rigidbody>();
        ResetControllers();
        Instance = this;
    }

    private void ResetControllers()
    {
        _rb.velocity = Vector3.left * StartSpeed;
        FartJuice.health = StartingFartJuice;
        DrunkController.Reset();
    }

    private void Update()
    {
        CheckLoseConditions();
    }

    internal void Reset()
    {
        transform.position = Vector3.zero;
    }
    void FixedUpdate()
    {
        Debug.Log("Input: " + _input.z);
        Move(_input, Input.GetKey(BoostKey));
        //if (!GameManagerBehaviour.GameInProgress) return;

        ApplyFriction();
        _input = new Vector3(
            -_currentSpeed,
            0,
            -Input.GetAxisRaw("Vertical"));

        //RestrictZLocation();
        RestrictVelocity();
        
        DrunkController.DrunkMovement(_rb);
    }

    private void CheckLoseConditions()
    {
        if(!GameManagerBehaviour.Instance.GameInProgress)
        {
            return;
        }
        // LOSE CONDITION: fartmeter is filled up, shitting your pants
        if (FartJuice.health >= FartJuice.maximumHealth)
        {
            GameManagerBehaviour.EndGame("You shat yourself! Make sure to fart from time to time. Don't worry, nobody will know it's you.");
        }

        // LOSE CONDITION: if you fart too quick 
        if (FartCooldown.health >= FartCooldown.maximumHealth)
        {
            GameManagerBehaviour.EndGame("You shat yourself! Don't fart too hard; for once Icarus taught us not to fly too close to the sun.");
        }

        //LOSE CONDITION: if you lose balance
        if (LostBalance())
        {
            Debug.Log("Lost balance!");
            GameManagerBehaviour.EndGame("You lost balance! Maybe you should drink less, you boozebag.");
        }

        //LOSE CONDITION: if you stop running
        if (_rb.velocity.sqrMagnitude < .1f)
        {
            GameManagerBehaviour.EndGame("You got too tired!");
        }
    }

    private void ApplyFriction()
    {
        Debug.LogWarning("Warning: This code is written at 4:20 am and is most likely completely retarded. pls rewrite");
        var newVel = _rb.velocity * (1 - Friction * Time.deltaTime);
        _rb.velocity = newVel;
    }

    private void Move(Vector3 direction, bool boost)
    {
        // Debug.Log($"Moving: {direction}");
        float boostFactor = 1;
        

        // if fartbar is lower than 5/100, no boost possible
        if (boost && FartJuice.health >= 5)
        {
            boostFactor = BoostForce;
            FartJuice.SetHealth(FartJuice.health - 1); // deplete fartmeter by boosting
            FartCooldown.SetHealth(FartCooldown.health + FartCooldownFactor); // fill up cooldown bar by boosting
        }

        Vector3 movementForce = direction * Force * Time.deltaTime;
        movementForce = Vector3.Scale(movementForce, new Vector3(boostFactor, 0, 1));
        
        _rb.AddForce(movementForce, ForceMode.Force);
    }

    private void RestrictZLocation()
    {
        var pos = transform.position;
        pos.z = Mathf.Clamp(transform.position.z, ZLocationMinMax.x, ZLocationMinMax.y);
        transform.position =  pos;
    }
    private bool LostBalance() 
    {
        Debug.Log("DEBUGGING BALANCE:\nPos z: " + transform.position.z + " Vel z: " + _rb.velocity.z + "Z min/max: " + ZLocationMinMax.x + "/" + ZLocationMinMax.y);
        return transform.position.z < ZLocationMinMax.x || transform.position.z > ZLocationMinMax.y;

    }

    private void RestrictVelocity()
    {
        Vector3 vel = _rb.velocity;
        vel.x = Mathf.Clamp(vel.x, -MaxSpeed, 0);
        vel.z = Mathf.Clamp(vel.z, -MaxZSpeed, MaxZSpeed);
        _rb.velocity = vel;
    }
}