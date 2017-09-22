using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDispatcher : MonoBehaviour {

    private Thrusters thrusters;
    private Rigidbody rb;
    private Vector3 hyperDst;
    private ShipMeta ship;

    [SerializeField]
    private Camera currentCamera;

    [SerializeField]
    [Range(0,1)]
    private float throttle;

    private void Awake()
    {
        thrusters = GetComponent<Thrusters>();
        rb = GetComponent<Rigidbody>();
        ship = GetComponent<ShipMeta>();
        hyperDst = Vector3.zero;
        currentCamera = Camera.current;
    }

    void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Camera.current != null)
        {
            currentCamera = Camera.current;
        }
        if (hyperDst != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, currentCamera.transform.position) < 100.0f)
            {
                if (ship.flybySound.enabled && !ship.flybySound.isPlaying)
                {
                    ship.flybySound.Play();
                }
            }
            if (Vector3.Distance(hyperDst, transform.position) < 30.0f)
            {
                rb.velocity = transform.forward * throttle * ship.absoluteMaxSpeed;
                hyperDst = Vector3.zero;
            } else
            {
                rb.velocity = transform.forward * 500.0f;
            }
        }
        else
        {
            ship.engineSound.volume = throttle;
            if (!ship.engineSound.isPlaying)
            {
                ship.engineSound.Play();
            }
            //rb.AddForce(transform.forward * throttle * ship.absoluteMaxSpeed);
            rb.velocity = transform.forward * throttle * ship.absoluteMaxSpeed;
        }
	}

    public void hyperIn(Vector3 dst)
    {
        transform.rotation = Quaternion.LookRotation(dst);
        hyperDst = dst;
    }

    public void disableFlybySound()
    {
        ship.flybySound.enabled = false;
    }

    public void enableFlybySound()
    {
        ship.flybySound.enabled = true;
    }

    public void setThrottle(float _setting)
    {
        throttle = _setting;
    }
}
