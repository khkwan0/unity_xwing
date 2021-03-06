﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMeta : MonoBehaviour {

    public bool friend;
    public bool foe;

    public float absoluteMaxSpeed;
    public float maxTurnSpeed;

    public string groupName;
    public string callSign;

    public AudioSource hyperInSound;
    public AudioSource engineSound;
    public AudioSource flybySound;

    public string faction; // imp, ra, other...
    
    enum __mission
    {
        defend,
        attack,
        escort,
        guard         
    }

    public GameObject missionTarget;

    [SerializeField]
    private __mission _mission;

    [SerializeField]
    private bool _isTargetted;  
    void Start () {
        _isTargetted = false;
        if (transform.gameObject.name != "player_xwing")
        {
            if (GameObject.Find("HUD").GetComponent<Radar>() != null)
            {
                GameObject.Find("HUD").GetComponent<Radar>().register(gameObject);
            }          
        }
	}
	
	// Update is called once per frame
	void Update () {	
	}

    public void setTarget()
    {
        _isTargetted = true;
    }

    public void deTarget()
    {
        _isTargetted = false;
    }

    public bool isTargetted()
    {
        return _isTargetted;
    }
}