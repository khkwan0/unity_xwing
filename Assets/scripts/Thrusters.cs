using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrusters : MonoBehaviour {

    [SerializeField]
    private float throttle;
    // Use this for initialization
    private Rigidbody rb;

    public float _maxSpeed;

	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        rb.AddForce(transform.forward * throttle * _maxSpeed);
		
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
}
