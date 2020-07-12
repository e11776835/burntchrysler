using System;
using System.Collections;
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
        BoostSpeedMaxSpeedMultiplier = 1.2f,
        MaxZSpeed = 10, 
        Friction = 10;
    public KeyCode BoostKey;
    [Range(0,1)]
    [SerializeField] private float _drunk;
    private Vector3 _input;
    private Rigidbody _rb;
    [SerializeField] private Healthbar Fartbar;
    [Tooltip("Affects how intense drunk velocity changes will be")]
    [SerializeField] private AnimationCurve DrunkenessVelocityIntensity;
    [Tooltip("Multiplies the animation curve times")]
    [SerializeField] private int DrunkenessVelocityMultiplier;
    [Tooltip("How much time there is between velocity changes")]
    [SerializeField] private AnimationCurve DrunkenessTimeProbability;

    [Tooltip("Multiplies the animation curve times")]
    [SerializeField] private int DrunkenessTimeMultiplier;

    public Healthbar FartJuice;
    public Healthbar FartCooldown;
    public float FartCooldownFactor;
    public int StartingFartJuice;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.left * StartSpeed;
        FartJuice.health = StartingFartJuice;
    }

    void FixedUpdate()
    {

        // LOSE CONDITION: fartmeter is filled up, shitting your pants
        if (FartJuice.health >= FartJuice.maximumHealth)
        {
            GameManager.EndGame();
            
        }

        // LOSE CONDITION: if you fart too quick 
        if (FartCooldown.health >= FartCooldown.maximumHealth)
        {
            Time.timeScale = 0.0f;
        }


        DrunkMovement();
        ApplyFriction();
        _input = new Vector3(
            -1,
            0,
            -Input.GetAxisRaw("Vertical"));

        Move(_input, Input.GetKey(BoostKey));
        RestrictZLocation();
        RestrictVelocity();
    }

    private Coroutine _drunkVelocityCoroutine;
    public float HorizontalDrunkIntensity = 1, VerticalDrunkIntensity = .2f;

    /// <summary>
    /// Changes the velocity at random intervals in random directions
    /// </summary>
    private void DrunkMovement()
    {
        if(_drunkVelocityCoroutine == null)
        {
            _drunkVelocityCoroutine = StartCoroutine(DrunkVelocityChange());
        }
    }

    private IEnumerator DrunkVelocityChange()
    {
        var startTime = Time.time;
        float timeUntilNext = DrunkenessTimeProbability.Evaluate(_drunk) * DrunkenessTimeMultiplier;
        timeUntilNext = UnityEngine.Random.Range(timeUntilNext, DrunkenessTimeMultiplier);
        if(timeUntilNext < 0.1f)
        {
            Debug.LogError("Time between velocity changes too short!");
            yield return null;
        }
        var nextVelocityTime = startTime + timeUntilNext;
        var beforeVel = _rb.velocity;
        var direction = UnityEngine.Random.insideUnitCircle;
        //Velocity will always be on right side of bert (x-)
        direction.x = Mathf.Abs(direction.x) * -1;
        direction = Vector2.Scale(direction, new Vector2(HorizontalDrunkIntensity, VerticalDrunkIntensity));
        Debug.Log($"Changing velocity in direction: {direction}");
        var intensity = DrunkenessVelocityIntensity.Evaluate(_drunk) * DrunkenessVelocityMultiplier;
        direction *= intensity;
        Vector3 newVel = new Vector3(direction.x, 0, direction.y);
        _rb.AddForce(newVel * 100);
        while (Time.time < nextVelocityTime)
        {
            //check next timer every .5 secs
            yield return new WaitForSeconds(.5f);
        }
        _drunkVelocityCoroutine = StartCoroutine(DrunkVelocityChange());
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

    private void RestrictVelocity()
    {
        Vector3 vel = _rb.velocity;
        Debug.Log($"Vel: {vel}");
        vel.x = Mathf.Clamp(vel.x, -MaxSpeed, 0);
        vel.z = Mathf.Clamp(vel.z, -MaxZSpeed, MaxZSpeed);
        _rb.velocity = vel;
    }
}