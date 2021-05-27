using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrentAnimator : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] Animator _anim;
    [SerializeField] float MaxSpeed;

    void Start()
    {
        if (!_anim)
        {
            _anim = GetComponent<Animator>();
        }
        if (!_anim) Debug.LogError("No animator set up on Brent");
        if (!_rb)
        {
            Debug.LogError("Brent's animator velocity reference missing");
        }
    }
    private void Update()
    {
        Vector3 vel = _rb.velocity;
        _anim.transform.LookAt(_anim.transform.position + vel, Vector3.up);
        SetSpeed(vel.sqrMagnitude/MaxSpeed);
    }
    public void SetSpeed(float val)
    {
        _anim.SetFloat("Speed", Mathf.Clamp(val, 0, 1));
    }
}
