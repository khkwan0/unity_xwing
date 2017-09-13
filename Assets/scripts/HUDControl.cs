using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUDControl : MonoBehaviour {

    private GameObject _eventLog;
    private string theText;

    public Camera targetCamera;
    public AudioSource targetSound;

    public GameObject tempTarget;

    private CrosshairControl crosshairs;

    [SerializeField]
    private GameObject _targetted;
    [SerializeField]
    private GameObject _oldTarget;
    [SerializeField]
    private float _dist;
    [SerializeField]
    private Vector3 _int;
    [SerializeField]
    private Vector3 _forward;
    [SerializeField]
    private float _dot;
    [SerializeField]
    private float _intDist;


    [SerializeField]
    private float _scaleFactor;
    private BottomHUDController bottomHud;
    private HUDFeedback hudFeedback;

	// Use this for initialization
	void Awake () {
        _eventLog = GameObject.Find("EventText");
        _oldTarget = null;
        bottomHud = transform.Find("bottom_hud").gameObject.GetComponent<BottomHUDController>();
        hudFeedback = transform.Find("feedback").gameObject.GetComponent<HUDFeedback>();
        crosshairs = transform.Find("crosshairs").gameObject.GetComponent<CrosshairControl>();
    }
	
    private void enableNormalCrossHairs()
    {
        crosshairs.enableNormal();
    }

    private void enableTargetCrosshairs()
    {
        crosshairs.enabledTarget();
    }

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R))
        {
            if (_targetted != null)
            {
                _oldTarget = _targetted;
            }
            _targetted = GetComponent<Radar>().getClosestEnemy();
            if (_targetted != null && _oldTarget != _targetted)
            {
                if (_oldTarget != null)
                {
                    _oldTarget.GetComponent<ShipMeta>().deTarget();
                }
                _targetted.GetComponent<ShipMeta>().setTarget();

                if (!targetSound.isPlaying)
                {
                    targetSound.Play();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (_targetted != null)
            {
                _targetted.GetComponent<ShipMeta>().deTarget();
                _targetted = null;
                _oldTarget = null;
                targetCamera.enabled = false;
                bottomHud.deTarget();
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (_targetted != null)
            {
                _targetted.GetComponent<ShipMeta>().deTarget();
            }
            _targetted = GetComponent<Radar>().getNextTarget();
            if (_targetted != null)
            {
                _targetted.GetComponent<ShipMeta>().setTarget();
                if (!targetSound.isPlaying)
                {
                    targetSound.Play();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (_targetted != null)
            {
                _targetted.GetComponent<ShipMeta>().deTarget();
            }
            _targetted = GetComponent<Radar>().getPrevTarget();
            if (_targetted != null)
            {
                _targetted.GetComponent<ShipMeta>().setTarget();
                if (!targetSound.isPlaying)
                {
                    targetSound.Play();
                }
            }
        }
        if (_targetted)
        {
            targetCamera.enabled = true;

            // place the target camera
            targetCamera.transform.rotation = Quaternion.LookRotation(_targetted.transform.position - transform.parent.position);
            _dist = Vector3.Distance(_targetted.transform.position, transform.parent.position);
            _scaleFactor = 10.0f;
            targetCamera.transform.position = transform.parent.position + (targetCamera.transform.forward * _dist) - (targetCamera.transform.forward * _scaleFactor);
            bottomHud.setTarget(_targetted);

            // determine lead/intercept point
            _int = FindInterceptVector(transform.parent.position, 320.0f, _targetted.transform.position, _targetted.GetComponent<Rigidbody>().velocity);
            _forward = transform.parent.forward;

            tempTarget.transform.position = _targetted.transform.position + _int;
            _dot = Vector3.Dot(_int.normalized, transform.parent.forward);
            if (_dot > 0.9998f)
            {
                this.enableTargetCrosshairs();
            } else
            {
                this.enableNormalCrossHairs();
            }
            _intDist = Vector3.Distance(_targetted.transform.position, _int);
        }
	}

    private Vector3 FindInterceptVector(Vector3 shotOrigin, float shotSpeed, Vector3 targetOrigin, Vector3 targetVel)
    {

        Vector3 dirToTarget = Vector3.Normalize(targetOrigin - shotOrigin);

        // Decompose the target's velocity into the part parallel to the
        // direction to the cannon and the part tangential to it.
        // The part towards the cannon is found by projecting the target's
        // velocity on dirToTarget using a dot product.
        Vector3 targetVelOrth =
        Vector3.Dot(targetVel, dirToTarget) * dirToTarget;

        // The tangential part is then found by subtracting the
        // result from the target velocity.
        Vector3 targetVelTang = targetVel - targetVelOrth;

        /*
        * targetVelOrth
        * |
        * |
        *
        * ^...7  <-targetVel
        * |  /.
        * | / .
        * |/ .
        * t--->  <-targetVelTang
        *
        *
        * s--->  <-shotVelTang
        *
        */

        // The tangential component of the velocities should be the same
        // (or there is no chance to hit)
        // THIS IS THE MAIN INSIGHT!
        Vector3 shotVelTang = targetVelTang;

        // Now all we have to find is the orthogonal velocity of the shot

        float shotVelSpeed = shotVelTang.magnitude;
        if (shotVelSpeed > shotSpeed)
        {
            // Shot is too slow to intercept target, it will never catch up.
            // Do our best by aiming in the direction of the targets velocity.
            return targetVel.normalized * shotSpeed;
        }
        else
        {
            // We know the shot speed, and the tangential velocity.
            // Using pythagoras we can find the orthogonal velocity.
            float shotSpeedOrth =
            Mathf.Sqrt(shotSpeed * shotSpeed - shotVelSpeed * shotVelSpeed);
            Vector3 shotVelOrth = dirToTarget * shotSpeedOrth;

            // Finally, add the tangential and orthogonal velocities.
            return shotVelOrth + shotVelTang;
        }
    }

    public void appendToLog(Color _textColor, string _text)
    {

        theText += _text;
        _eventLog.GetComponent<Text>().color = _textColor;
        _eventLog.GetComponent<Text>().text = theText;
    }

    public GameObject getTargetted()
    {
        return _targetted;    
    }

    public GameObject getTarget()
    {
        return _targetted;
    }

    public void hudMessage(string _msg)
    {
        hudFeedback.setText(_msg);
    }
}
