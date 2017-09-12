﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUDControl : MonoBehaviour {

    private GameObject _crossHairs;
    private GameObject _eventLog;
    private string theText;

    public Camera targetCamera;
    public AudioSource targetSound;

    [SerializeField]
    private GameObject _targetted;
    [SerializeField]
    private GameObject _oldTarget;
    [SerializeField]
    private float _dist;

    [SerializeField]
    private float _scaleFactor;
    private BottomHUDController bottomHud;
    private HUDFeedback hudFeedback;

	// Use this for initialization
	void Awake () {
        _crossHairs = GameObject.Find("Crosshairs");
        _eventLog = GameObject.Find("EventText");
        _oldTarget = null;
        bottomHud = transform.Find("bottom_hud").gameObject.GetComponent<BottomHUDController>();
        hudFeedback = transform.Find("feedback").gameObject.GetComponent<HUDFeedback>();
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

            targetCamera.transform.rotation = Quaternion.LookRotation(_targetted.transform.position - transform.parent.position);
            //Debug.Log(_targetted.transform.position + " --- " + transform.parent.position + " = " + Vector3.Distance(_targetted.transform.position, transform.parent.position));
            _dist = Vector3.Distance(_targetted.transform.position, transform.parent.position);
            _scaleFactor = 10.0f;
            targetCamera.transform.position = transform.parent.position + (targetCamera.transform.forward * _dist) - (targetCamera.transform.forward * _scaleFactor);
            bottomHud.setTarget(_targetted);
        }
	}

    public void setCrossHairPos(Vector3 _pos)
    {
        _crossHairs.GetComponent<RectTransform>().localPosition = _pos;
  
    }

    public void enableCrosshairs(bool _enable)
    {
        _crossHairs.SetActive(_enable);
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
