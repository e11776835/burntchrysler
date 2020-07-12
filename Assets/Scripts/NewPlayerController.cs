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
        MaxZSpeed = 10, 
        Friction;
    public KeyCode BoostKey;

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
            // for now, this does nothing apart from stopping time 
            Time.timeScale = 0.0f;
        }

        // LOSE CONDITION: if you fart too quick 
        if (FartCooldown.health >= FartCooldown.maximumHealth)
        {
            Time.timeScale = 0.0f;
        }


        DrunkMovement();
        ApplyFriction();
        _input = new Vector3(
            -Input.GetAxisRaw("Horizontal"),
            0,
            -Input.GetAxisRaw("Vertical"));

        if (_input != Vector3.zero)
        {
            Move(_input, Input.GetKey(BoostKey));
        }
        RestrictZLocation();
        RestrictVelocity();
    }

    private Coroutine _drunkVelocityCoroutine;

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
        Debug.Log("Changing velocity!");
        var startTime = Time.time;
        var duration = 0.2f;
        float timeUntilNext = DrunkenessTimeProbability.Evaluate(_drunk) * DrunkenessTimeMultiplier;
        var nextVelocityTime = startTime + timeUntilNext;
        var beforeVel = _rb.velocity;
        var direction = UnityEngine.Random.insideUnitCircle;
        //Velocity will always be on right side of bert (x-)
        direction.x = Mathf.Abs(direction.x) * -1;
        var intensity = DrunkenessVelocityIntensity.Evaluate(_drunk) * DrunkenessVelocityMultiplier;
        direction *= intensity;
        for (float timer = duration; timer >= 0; timer -= Time.deltaTime)
        {
            _rb.velocity = Vector3.Lerp(beforeVel, direction, Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        while (Time.time < nextVelocityTime)
        {
            //check next timer every .5 secs
            yield return new WaitForSeconds(.5f);
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
        Debug.Log($"Moving: {direction}");
        float boostFactor = 1;

        // if fartbar is lower than 5/100, no boost possible
        if (boost && FartJuice.health >= 5)
        {
            boostFactor = BoostForce;
            FartJuice.SetHealth(FartJuice.health - 1); // deplete fartmeter by boosting
            FartCooldown.SetHealth(FartCooldown.health + FartCooldownFactor); // fill up cooldown bar by boosting
        }

        _rb.AddForce(direction * Force * Time.deltaTime * boostFactor, ForceMode.Acceleration);
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