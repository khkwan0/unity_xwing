using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrusters : MonoBehaviour {

    public AudioSource engineFullThrottleSound;
    private float throttle;
    private Rigidbody rb;

    [Range(0, 1)]
    public float _maxEngineVolume;

    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private float _maxThrust; // max force - determined by maxspeed and mass of rb
    [SerializeField]
    private float speed;
    [SerializeField]
    private float dragForce;

    private float _availableThrusters; // 0-1, dependent on power distribution -- shields, weapons

    [SerializeField]
    private bool inHyper;
    [SerializeField]
    private Vector3 hyperDest;

    public float hyperSpeed;
	void Awake () {
        _availableThrusters = 1.0f;
        rb = gameObject.GetComponent<Rigidbody>();
        _maxSpeed = this.calcMaxSpeed();
        _maxThrust = this.calcMaxForce(_maxSpeed);
        inHyper = false;
        hyperSpeed = 999.0f;
	}

    private float calcMaxForce(float _maxSpeed)
    {
        return GetComponent<Rigidbody>().mass * _maxSpeed;
    }

    public void setPowerDrain(float _amt)
    {
        _availableThrusters = _amt;  // to determine formula later
    }

    public float calcMaxSpeed()
    {
        return GetComponent<ShipMeta>().absoluteMaxSpeed * _availableThrusters;
    }

    void FixedUpdate () {
        //    rb.AddForce(transform.forward * throttle * (_maxThrust + 2.0f * throttle));
        if (!inHyper)
        {
            speed = rb.velocity.magnitude;
            dragForce = speed * (1 - Time.deltaTime * rb.drag);
            rb.AddForce(transform.forward * throttle * (_maxThrust + speed - dragForce));
            if (throttle > 0.0f)
            {
                if (!engineFullThrottleSound.isPlaying)
                {
                    engineFullThrottleSound.Play();
                }
            }
            else
            {
                engineFullThrottleSound.Stop();
            }
        }
        if (inHyper)
        {
            engineFullThrottleSound.Stop();
            rb.velocity = transform.forward * hyperSpeed;
            //rb.AddForce(transform.forward * hyperSpeed);
            if (Vector3.Distance(transform.position, hyperDest) < 10.0f)
            {
                endHyper();
            }
        }
    }

    private void Update()
    {

    }

    public void setFullThrottle()
    {
        throttle = 1.0f;
        engineFullThrottleSound.volume = _maxEngineVolume * throttle;
    }

    public void setNonThrottle()
    {
        throttle = 0.0f;
    }

    public void setOneThirdThrottle()
    {
        throttle = 0.333f;
        engineFullThrottleSound.volume = _maxEngineVolume * throttle;
    }

    public void setTwoThirdsThrottle()
    {
        throttle = 0.6666f;
        engineFullThrottleSound.volume = _maxEngineVolume * throttle;
    }

    public void setHalfThrottle()
    {
        throttle = 0.5f;
        engineFullThrottleSound.volume = _maxEngineVolume * throttle;
    }

    public void setDirectThrottle(float _amt)
    {
        throttle = _amt;
        engineFullThrottleSound.volume = _maxEngineVolume * throttle;
    }

    public void setSpeed(float _targetSpeed)
    {
        throttle = _targetSpeed / _maxThrust;
        if (throttle > 1.0f)
        {
            throttle = 1.0f;
        }
    }

    public void startHyper(Vector3 _dst)
    {
        rb.velocity = transform.forward * hyperSpeed;
        hyperDest = _dst;
        inHyper = true;
    }

    public void endHyper()
    {
        rb.velocity = transform.forward * 0.0f;
        inHyper = false;
    }

    public bool isInHyper()
    {
        return inHyper;
    }

    public void stopEngineSound()
    {

    }
}
