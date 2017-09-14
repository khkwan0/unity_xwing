using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : MonoBehaviour {

    public KeyCode fire;
    public KeyCode alt_fire;

    public KeyCode k_cockpitCamera;
    public KeyCode k_chaseCamera;
    public KeyCode k_freeCamera;

    public KeyCode k_fullThrottle;
    public KeyCode k_noThrottle;
    public KeyCode k_oneThirdThrottle;
    public KeyCode k_twoThirdsThrottle;

    public Camera cockpitCamera;
    public Camera chaseCamera;
    public Camera freeCamera;
    public Camera targetCamera;

    private GameObject HUD;
    private GameObject _crossHairs;
    private HUDControl hudControl;

    [SerializeField]
    private GameObject __target;

    private Thrusters thrusters;

    void Awake()
    {
        cockpitCamera.enabled = true;
        freeCamera.enabled = false;
        chaseCamera.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start () {
        HUD = GameObject.Find("HUD");
        hudControl = HUD.GetComponent<HUDControl>();
        thrusters = GetComponent<Thrusters>();
        string minutes = Mathf.Floor(Time.fixedTime / 60).ToString("00");
        string seconds = (Time.fixedTime % 60).ToString("00");
        hudControl.appendToLog(new Color(0.7f, 0.1f, 0.9f), minutes+":"+seconds + "Mission started\n");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(k_cockpitCamera))
        {
            chaseCamera.enabled = false;
            freeCamera.enabled = false;
            cockpitCamera.enabled = true;
            hudControl.enabledCrosshairs();
            hudControl.enableRadar();
            hudControl.enableEventLog();
            hudControl.enableBottomHud();
            hudControl.enableToggle();
        }
        if (Input.GetKeyDown(k_chaseCamera))
        {
            chaseCamera.enabled = true;
            freeCamera.enabled = false;
            cockpitCamera.enabled = false;
            hudControl.enabledCrosshairs();
            hudControl.disableRadar();
            hudControl.disableEventLog();
            hudControl.disableBottomHud();
            hudControl.disableToggle();
        }
        if (Input.GetKeyDown(k_freeCamera))
        {
            chaseCamera.enabled = false;
            freeCamera.enabled = true;
            cockpitCamera.enabled = false;
            hudControl.disableCrosshairs();
            hudControl.disableEventLog();
            hudControl.disableRadar();
            hudControl.disableBottomHud();
            hudControl.disableToggle();
        }
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