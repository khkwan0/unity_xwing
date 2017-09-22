using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController2 : MonoBehaviour {

    public GameObject general;
    public GameObject cockpitPanels;
    public GameObject crosshairs;
    public GameObject targetting;
    public GameObject bottomHud;

    private bool inCockpit;
    private GameObject currentShip;

    private CrosshairControl _crossHairs;
    private BottomHUDController _bhud;
	// Use this for initialization
	void Start () {
        _crossHairs = crosshairs.GetComponent<CrosshairControl>();
        _bhud = bottomHud.GetComponent<BottomHUDController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setNormalCrosshairs()
    {
        _crossHairs.enableNormal();
    }

    public void setTargetCrosshairs()
    {
        _crossHairs.enabledTarget();
    }

    public void setTarget(GameObject _target)
    {
        _bhud.setTarget(_target);
    }

    public void deTarget()
    {
        _bhud.deTarget();
    }
    public void setOsdMsg(string _text)
    {
        general.GetComponent<HUDFeedback>().setText(_text);
    }

    public void setOsdMsg(string _text, float _timeout)
    {
        general.GetComponent<HUDFeedback>().setText(_text, _timeout);
    }

    public void enableAll()
    {
        this.enableCockpitPanels();
        this.enableTargetting();
        this.enableCrosshairs();
    }

    public void disableAll()  // except for OSD
    {
        this.disableCockpitPanels();
        this.disableCrossHairs();
        this.disableTargetting();
    }

    public void disableCockpitPanels()
    {
        cockpitPanels.SetActive(false);
    }

    public void disableCrossHairs()
    {
        crosshairs.SetActive(false);
    }

    public void disableTargetting()
    {
        targetting.SetActive(false);
    }

    public void enableCockpitPanels()
    {
        cockpitPanels.SetActive(true);
    }

    public void enableTargetting()
    {
        targetting.SetActive(true);
    }

    public void enableCrosshairs()
    {
        crosshairs.SetActive(true);
    }

    public void setShip(GameObject _ship)
    {
        currentShip = _ship;
    }

    public void unsetShip()
    {
        currentShip = null;
    }

    public GameObject getCurrentShip()
    {
        return currentShip;
    }
}
