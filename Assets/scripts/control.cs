using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : MonoBehaviour {

    public KeyCode fire;
    public KeyCode alt_fire;
    public KeyCode k_fullThrottle;
    public KeyCode k_noThrottle;
    public KeyCode k_oneThirdThrottle;
    public KeyCode k_twoThirdsThrottle;
    public KeyCode k_cockpitCamera;
    public KeyCode k_chaseCamera;
    public KeyCode k_freeCamera;

    public float startThrust = 0.0f;
    public float absoluteMaxSpeed = 100.0f;
    public float acceleration = 5.0f;
    public float baseTurnSpeed;
    public float rotateSpeed;

    public Camera cockpitCamera;
    public Camera chaseCamera;
    public Camera freeCamera;
    public Camera targetCamera;

    public AudioSource engineFullThrottleSound;

    [SerializeField]
    private float _thrust;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _targetSpeed;
    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private float _acceleration;
    private float _newThrust;
    private float _thrustDir;
    private float _turnSpeed;
    private GameObject HUD;

    private GameObject _crossHairs;

    private Rigidbody _rb;
    // Use this for initialization

    private void Awake()
    {
        cockpitCamera.enabled = true;
        freeCamera.enabled = false;
        chaseCamera.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start () {
        _thrust = startThrust;
        _maxSpeed = absoluteMaxSpeed;
        _rb = GetComponent<Rigidbody>();
        _turnSpeed = baseTurnSpeed;
        HUD = GameObject.Find("HUD");
        string minutes = Mathf.Floor(Time.fixedTime / 60).ToString("00");
        string seconds = (Time.fixedTime % 60).ToString("00");
        HUD.GetComponent<HUDControl>().appendToLog(new Color(0.7f, 0.1f, 0.9f), minutes+":"+seconds + "Mission started\n");
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (Input.GetKeyDown(k_cockpitCamera))
        {
            chaseCamera.enabled = false;
            freeCamera.enabled = false;
            cockpitCamera.enabled = true;
            HUD.GetComponent<HUDControl>().setCrossHairPos(new Vector3(0.0f, -3.0f, 0.0f));
            HUD.GetComponent<HUDControl>().enableCrosshairs(true);
        }
        if (Input.GetKeyDown(k_chaseCamera))
        {
            chaseCamera.enabled = true;
            freeCamera.enabled = false;
            cockpitCamera.enabled = false;
            HUD.GetComponent<HUDControl>().setCrossHairPos(new Vector3(0.0f, -11.0f, 0.0f));
            HUD.GetComponent<HUDControl>().enableCrosshairs(true);
            if (targetCamera != null)
            {
                targetCamera.enabled = false;                     
            }
        }
        if (Input.GetKeyDown(k_freeCamera))
        {
            chaseCamera.enabled = false;
            freeCamera.enabled = true;
            cockpitCamera.enabled = false;
            HUD.GetComponent<HUDControl>().enableCrosshairs(false);
            if (targetCamera != null)
            {
                targetCamera.enabled = false;
            }
        }
        _rb.AddRelativeTorque(Input.GetAxis("Vertical") * _turnSpeed, Input.GetAxis("Horizontal") * _turnSpeed, 0.0f);
        _rb.AddRelativeTorque(0.0f, 0.0f, Input.GetAxis("RollLeft") * rotateSpeed);
        _rb.AddRelativeTorque(0.0f, 0.0f, -1.0f * Input.GetAxis("RollRight") * rotateSpeed);
        if (Input.GetKey(k_fullThrottle))
        {
            _newThrust = 1.0f;
            engineFullThrottleSound.Play();
            if (_newThrust > _thrust)
            {
                _thrust = _newThrust;
                _thrustDir = 1.0f;
            }
            _turnSpeed = baseTurnSpeed;
            _targetSpeed = _maxSpeed;
        }
        if (Input.GetKeyDown(k_twoThirdsThrottle))
        {
            _newThrust = 0.6666f;
            if (_newThrust > _thrust)
            {
                _thrustDir = 1.0f;
            }
            if (_newThrust < _thrust)
            {
                _thrustDir = -1.0f;
            }
            _turnSpeed = baseTurnSpeed * 1.25f;
            _thrust = _newThrust;
            _targetSpeed = _thrust * _maxSpeed;
        }
        if (Input.GetKeyDown(k_oneThirdThrottle))
        {
            _newThrust = 0.3333f;
            if (_newThrust > _thrust)
            {
                _thrustDir = 1.0f;
            }
            if (_newThrust < _thrust)
            {
                _thrustDir = -1.0f;
            }
            _turnSpeed = baseTurnSpeed * 1.25f;
            _thrust = _newThrust;
            _targetSpeed = _thrust * _maxSpeed;
        }
        if (Input.GetKey(k_noThrottle))
        {
            _thrust = 0.0f;
            _targetSpeed = 0.0f;
            _thrustDir = -1.5f;
            _turnSpeed = baseTurnSpeed;
            engineFullThrottleSound.Stop();
        }
        _speed = calculateSpeed(_thrust, _speed, _targetSpeed);
        _rb.velocity = _rb.transform.forward * _speed;
        if (Input.GetKeyDown(KeyCode.P))
        {

        }
    }

    private float calculateSpeed(float _thrust, float _currentSpeed, float _targetSpeed)
    {
        if (_currentSpeed < _targetSpeed && _thrustDir > 0.0f)
        {
            _acceleration = acceleration * _thrust;
            _currentSpeed = _currentSpeed + _acceleration * Time.deltaTime;
        }
        if (_currentSpeed > _targetSpeed && _thrustDir < 0.0f)
        {
            _acceleration = acceleration * _thrustDir;
            _currentSpeed = _currentSpeed + _acceleration * Time.deltaTime;
        }
        if (_currentSpeed < 0.0f)
        {
            _currentSpeed = 0.0f;
        }
        return _currentSpeed;
    }
}


