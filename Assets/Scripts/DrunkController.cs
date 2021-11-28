using UnityEngine;

public class DrunkController : MonoBehaviour{
    [Header("Current Drunkness")]
    
    [Range(0,1), Tooltip("How drunk the player is (0 - 1)"), SerializeField] 
    private float _drunk;
    [SerializeField] 
    private Vector2 _currentDrunkForce;
    [SerializeField]
    private float _nextDrunkForceChangeTime;
    
    [SerializeField]
    private float MinimumDrunkVelocityChangeTime = 0.5f;

    [Header("Drunkness Settings")]
    
    [Tooltip("Affects how intense drunk velocity changes will be")]
    [SerializeField] private AnimationCurve DrunkenessVelocityIntensity;

    [Tooltip("Multiplies the animation curve times"), SerializeField] 
    private int DrunkenessVelocityMultiplier;
    
    [Tooltip("How much time there is between velocity changes"), SerializeField] 
    private AnimationCurve DrunkenessTimeProbability;

    [Tooltip("Multiplies the animation curve times"), SerializeField] 
    private int DrunkenessTimeMultiplier;
    
    public float HorizontalDrunkIntensity = 1, VerticalDrunkIntensity = .2f;

    /// <summary>
    /// Changes the velocity at random intervals in random directions
    /// </summary>
    public void DrunkMovement(Rigidbody rb)
    {
        if (_drunk <= 0) return;
        ApplyDrunkForce(rb, _currentDrunkForce);
        CheckDrunkForceChange();
    }
    void ApplyDrunkForce(Rigidbody rb, Vector2 drunkForce) =>
        rb.AddRelativeForce(drunkForce.x, 0,  drunkForce.y, ForceMode.Acceleration);

    internal void Reset()
    {
        _drunk = 0;
        _currentDrunkForce = Vector2.zero;
        _nextDrunkForceChangeTime = 0;

        
    }

    private void CheckDrunkForceChange()
    {
        _nextDrunkForceChangeTime -= Time.deltaTime;
        if (_nextDrunkForceChangeTime > 0) return;
        _nextDrunkForceChangeTime = GetNextDrunkForceChangeTime();
        _currentDrunkForce = GetNextDrunkForce();
        
    }

    private float GetNextDrunkForceChangeTime(){

        float timeUntilNext = DrunkenessTimeProbability.Evaluate(_drunk) * DrunkenessTimeMultiplier;
        timeUntilNext = UnityEngine.Random.Range(timeUntilNext, DrunkenessTimeMultiplier);
        if (timeUntilNext < MinimumDrunkVelocityChangeTime)
        {
            timeUntilNext = MinimumDrunkVelocityChangeTime;
        }
        var nextVelocityTime = timeUntilNext;
        return nextVelocityTime;
    }

    private Vector2 GetNextDrunkForce()
    {
        var direction = UnityEngine.Random.insideUnitCircle;
        //Velocity will always be on right side of bert (x-)
        direction.x = Mathf.Abs(direction.x) * -1;
        direction = Vector2.Scale(direction, new Vector2(HorizontalDrunkIntensity, VerticalDrunkIntensity));
        Debug.Log($"Changing velocity in direction: {direction}");
        var intensity = DrunkenessVelocityIntensity.Evaluate(_drunk) * DrunkenessVelocityMultiplier;
        direction *= intensity;
        return direction;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        var drink = other.gameObject.GetComponent<DrinkBehaviour>();
        _drunk += drink.Drunkness;
        Debug.Log($"Drunkness: {_drunk}");
        drink.Drink();
    }
}
