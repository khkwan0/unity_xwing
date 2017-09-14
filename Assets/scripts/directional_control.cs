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

    private Vector3 _rollLeft, _rollRight;

    [SerializeField]
    private float maxTurnSpeed;

    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        maxTurnSpeed = gameObject.GetComponent<ShipMeta>().maxTurnSpeed;
        this.setFullTurnSpeed();
        _direction = new Vector3(0.0f, 0.0f);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        _direction = _pitch + _yaw + _roll;
        rb.AddRelativeTorque(_direction * _turnSpeed * maxTurnSpeed);
    }

    private void Update()
    {
        if (gameObject.tag == "Player")
        {
            rb.AddRelativeTorque(Input.GetAxis("Vertical") * _turnSpeed, Input.GetAxis("Horizontal") * _turnSpeed, 0.0f);
            rb.AddRelativeTorque(0.0f, 0.0f, Input.GetAxis("RollLeft") * _turnSpeed);
            rb.AddRelativeTorque(0.0f, 0.0f, -1.0f * Input.GetAxis("RollRight") * _turnSpeed);
        }

    }

    public void setFullTurnSpeed()
    {
        _turnSpeed = 1.0f * maxTurnSpeed;
    }

    public void setOneThirdTurnSpeed()
    {
        _turnSpeed = 0.33f * maxTurnSpeed;
    }

    public void setTurnSpeed(float __turnSpeed)
    {
        _turnSpeed = __turnSpeed;
    }

    public float getTurnSpeed()
    {
        return _turnSpeed;
    }

    public void turnTowardsTarget(GameObject target, float turnTime)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), turnTime);
    }

    public void turnTowardsTarget(Vector3 target, float turnTime)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target), turnTime);
    }
    public void stopTurning()
    {
        _yaw = new Vector3(0.0f, 0.0f, 0.0f);
        _pitch = new Vector3(0.0f, 0.0f, 0.0f);
        _roll = new Vector3(0.0f, 0.0f, 0.0f);
    }

    private void addYaw(float _amt)
    {
        _yaw = transform.up * _amt;
    }

    private void addPitch(float _amt)
    {
        _pitch = transform.right * _amt;
    }

    private void addRollLeft(float _amt)
    {
        _rollLeft = transform.forward * _amt;
    }

    private void addRollRight(float _amt)
    {
        _rollRight = transform.forward * _amt * -1.0f;
    }

    private void computeRoll()
    {
        _roll = _rollLeft + _rollRight;
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
