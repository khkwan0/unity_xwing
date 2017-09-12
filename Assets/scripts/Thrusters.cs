using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrusters : MonoBehaviour {

    public KeyCode k_fullThrottle;
    public KeyCode k_noThrottle;
    public KeyCode k_oneThirdThrottle;
    public KeyCode k_twoThirdsThrottle;

    public AudioSource engineFullThrottleSound;

    [SerializeField]
    private float throttle;
    private Rigidbody rb;

    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private float _maxThrust; // max force - determined by maxspeed and mass of rb
    [SerializeField]
    private float speed;
    [SerializeField]
    private float dragForce;

    private float _availableThrusters; // 0-1, dependent on power distribution -- shields, weapons

	void Start () {
        _availableThrusters = 1.0f;
        rb = gameObject.GetComponent<Rigidbody>();
        _maxSpeed = this.calcMaxSpeed();
        _maxThrust = this.calcMaxForce(_maxSpeed);   
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

    private void Update()
    {
        if (transform.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(k_fullThrottle))
            {
                this.setFullThrottle();
            }
            if (Input.GetKeyDown(k_noThrottle))
            {
                this.setNonThrottle();
            }
            if (Input.GetKeyDown(k_oneThirdThrottle))
            {
                this.setOneThirdThrottle();
            }
            if (Input.GetKeyDown(k_twoThirdsThrottle))
            {
                this.setTwoThirdsThrottle();
            }
        }
    }
    void FixedUpdate () {
    //    rb.AddForce(transform.forward * throttle * (_maxThrust + 2.0f * throttle));
        speed = rb.velocity.magnitude;
        dragForce = speed * (1 - Time.deltaTime * rb.drag);
        rb.AddForce(transform.forward * throttle * (_maxThrust + speed - dragForce));
    }

    public void setFullThrottle()
    {
        throttle = 1.0f;
    }

    public void setNonThrottle()
    {
        throttle = 0.0f;
    }

    public void setOneThirdThrottle()
    {
        throttle = 0.333f;
    }

    public void setTwoThirdsThrottle()
    {
        throttle = 0.6666f;
    }

    public void setHalfThrottle()
    {
        throttle = 0.5f;
    }

    public void setDirectThrottle(float _amt)
    {
        throttle = _amt;
    }

    public void setSpeed(float _targetSpeed)
    {
        throttle = _targetSpeed / _maxThrust;
        if (throttle > 1.0f)
        {
            throttle = 1.0f;
        }
    }
}
