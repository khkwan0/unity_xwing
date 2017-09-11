using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directional_control : MonoBehaviour {

    // Use this for initialization
    private Rigidbody rb;

    [SerializeField]
    private float _turnSpeed;

    private Vector3 _pitch;
    private Vector3 _yaw;
    private Vector3 _roll;
    private Vector3 _direction;

    public float maxTurnSpeed;

    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        this.setFullTurnSpeed();
        _direction = new Vector3(0.0f, 0.0f);
    }
	
	// Update is called once per frame
	void Update () {
        _direction = _pitch + _yaw + _roll;

        rb.AddRelativeTorque(_direction * _turnSpeed * maxTurnSpeed);
    }

    public void setFullTurnSpeed()
    {
        _turnSpeed = 1.0f;
    }

    public void setOneThirdTurnSpeed()
    {
        _turnSpeed = 0.33f;
    }

    public void setTurnSpeed(float __turnSpeed)
    {
        _turnSpeed = __turnSpeed;
    }

    public float getTurnSpeed()
    {
        return _turnSpeed;
    }

    public void turnTowardsTarget(GameObject target)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), 2.0f);
    }
    public void stopTurning()
    {
        _yaw = new Vector3(0.0f, 0.0f, 0.0f);
        _pitch = new Vector3(0.0f, 0.0f, 0.0f);
        _roll = new Vector3(0.0f, 0.0f, 0.0f);
    }
    public void yawRight(float _amt = 1.0f)
    {
        _yaw = transform.up * _amt;
    }

    public void yawLeft(float _amt = 1.0f)
    {
        _yaw = -1.0f * transform.up * _amt;
    }

    public void pitchUp(float _amt = 1.0f)
    {
        _pitch = transform.right * -1.0f * _amt;
    }

    public void pitchDown(float _amt = 1.0f) {
        _pitch = transform.right * _amt;
    }

    public void rollRight(float _amt = 1.0f)
    {
        _roll = transform.forward * _amt;
    }

    public void rollLeft(float _amt = 1.0f)
    {
        _roll = transform.forward * -1.0f * _amt;
    }

}
