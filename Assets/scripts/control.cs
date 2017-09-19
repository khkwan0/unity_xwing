using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : MonoBehaviour {

    public KeyCode fire;
    public KeyCode alt_fire;

    public KeyCode k_cockpitCamera;
    public KeyCode k_chaseCamera;
    public KeyCode k_targetChase;

    public KeyCode k_fullThrottle;
    public KeyCode k_noThrottle;
    public KeyCode k_oneThirdThrottle;
    public KeyCode k_twoThirdsThrottle;

    public Camera cockpitCamera;
    public Camera chaseCamera;
    public Camera targetChaseCamera;
    public Camera targetCamera;
    public Camera hyperCamera;

    public AudioSource hyperSpaceSound;

    private GameObject HUD;
    private GameObject _crossHairs;
    private HUDControl hudControl;

    private float hyperChargeTimeout;
    private bool hypering;

    [SerializeField]
    private GameObject __target;

    private Thrusters thrusters;
    private bool throttleControlEnabled;
    private Vector3 frozenPosition;

    void Awake()
    {
        cockpitCamera.enabled = true;
        if (targetChaseCamera != null)
        {
            targetChaseCamera.enabled = false;
        }
        chaseCamera.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        throttleControlEnabled = true;
        hypering = false;
    }

    void Start () {
        HUD = GameObject.Find("HUD");
        hudControl = HUD.GetComponent<HUDControl>();
        thrusters = GetComponent<Thrusters>();
        throttleControlEnabled = true;
    }

    public void disableThrottleControl()
    {
        throttleControlEnabled = false;
    }

    public void enableThrottleControl()
    {
        throttleControlEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hypering)
        {
            if (hyperChargeTimeout < 0.0f)
            {
                thrusters.startHyper(transform.forward * 1000.0f);
                hyperCamera.transform.position = frozenPosition;
            }
            else
            {
                hyperChargeTimeout -= Time.deltaTime;
                hyperCamera.transform.localPosition += new Vector3(0.0f, 0.02f, 0.0f);
                frozenPosition = hyperCamera.transform.position;
            }
        }
        else
        {
            if (Input.GetKeyDown(k_cockpitCamera))
            {
                chaseCamera.enabled = false;
                if (targetChaseCamera != null) targetChaseCamera.enabled = false;
                cockpitCamera.enabled = true;
                hudControl.enabledCrosshairs();
                hudControl.enableRadar();
                hudControl.showEventLog();
                hudControl.enableBottomHud();
                hudControl.enableToggle();
            }
            if (Input.GetKeyDown(k_chaseCamera))
            {
                chaseCamera.enabled = true;
                if (targetChaseCamera != null) targetChaseCamera.enabled = false;
                cockpitCamera.enabled = false;
                hudControl.enabledCrosshairs();
                hudControl.disableRadar();
                hudControl.hideEventLog();
                hudControl.disableBottomHud();
                hudControl.disableToggle();
            }
            if (Input.GetKeyDown(k_targetChase))
            {
                if (targetChaseCamera != null)
                {
                    chaseCamera.enabled = false;
                    targetChaseCamera.enabled = true;
                    cockpitCamera.enabled = false;
                    hudControl.disableCrosshairs();
                    hudControl.hideEventLog();
                    hudControl.disableRadar();
                    hudControl.disableBottomHud();
                    hudControl.disableToggle();
                }
            }
            if (throttleControlEnabled)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    __target = HUD.GetComponent<HUDControl>().getTarget();
                    if (__target != null)
                    {
                        thrusters.setSpeed(__target.GetComponent<Rigidbody>().velocity.magnitude);
                        hudControl.hudMessage("Matching speed with target");
                    }
                }
                if (Input.GetKeyDown(k_fullThrottle))
                {
                    thrusters.setFullThrottle();
                    hudControl.hudMessage("Throttle set to FULL");
                }
                if (Input.GetKeyDown(k_noThrottle))
                {
                    thrusters.setNonThrottle();
                    hudControl.hudMessage("Throttle set to NONE");
                }
                if (Input.GetKeyDown(k_oneThirdThrottle))
                {
                    thrusters.setOneThirdThrottle();
                    hudControl.hudMessage("Throttle set to 1/3");
                }
                if (Input.GetKeyDown(k_twoThirdsThrottle))
                {
                    thrusters.setTwoThirdsThrottle();
                    hudControl.hudMessage("Throttle set to 2/3");
                }
            }
        }
    }

    public void hyperOut()
    {
        chaseCamera.enabled = false;
        if (targetChaseCamera != null)
        {
            targetChaseCamera.enabled = false;
        }
        cockpitCamera.enabled = false;
        hyperCamera.enabled = true;
        
        hudControl.disableCrosshairs();
        hudControl.hideEventLog();
        hudControl.disableRadar();
        hudControl.disableBottomHud();
        hudControl.disableToggle();

        if (hyperSpaceSound != null)
        {
            hyperSpaceSound.Play();
        }
        hyperChargeTimeout = 2.0f;
        hypering = true;
        GetComponent<directional_control>().setHypering(true);
        GameObject.Find("FadeManager").GetComponent<FadeManager>().fade(3.0f, true, 1.25f);
    }
}