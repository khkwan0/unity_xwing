using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverScene2 : MonoBehaviour {

    public GameObject _hud;
    public List<AudioSource> backgroundMusic;
    public List<GameObject> npcs;
    public Camera sceneCamera;
    public GameObject _starField;
    public GameObject _player;

    private HUDController2 hud;
    [SerializeField]
    private float elapsedTime;
    private GameObject player;

    [SerializeField]
    private Vector3 hyperDst;
    [SerializeField]
    private GameObject cameraFocus;
    private GameObject xwing;
    [SerializeField]
    private Vector3 sceneCameraPosition;
    [SerializeField]
    private Vector3 sceneCameraRotation;
    private Vector3 cameraOffset;

    private bool camerafollow;
    private bool cameraKeepEyeOn;

    private GameObject starField;

    private GameObject _ship;
    private List<GameObject> squadron = new List<GameObject>();

    private void Awake()
    {
        hud = _hud.GetComponent<HUDController2>();
    }

    void Start() {
        this.doFade(2.0f, 1.5f, false);
        camerafollow = false;
        cameraKeepEyeOn = false;
        hud.disableAll();
    }

    private void setPlayer(GameObject _ship)
    {
        _ship.GetComponent<ShipControl>().takeOverShip();
        _ship.GetComponent<ShipControl>().alignCameraToCockpit(sceneCamera);
        hud.enableAll();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        elapsedTime = Time.fixedTime;
        if (sceneCamera != null) {
            sceneCameraPosition = sceneCamera.transform.position;
            sceneCameraRotation = sceneCamera.transform.rotation.eulerAngles;
        }
        if (elapsedTime == 0.0f)
        {
            //backgroundMusic[0].Play();
        }	
        if (elapsedTime == 4.0f)
        {
            hyperDst = sceneCamera.transform.position + sceneCamera.transform.forward * 2000.0f;
            xwing = hyperSpawn(npcs[0], sceneCamera.transform.position - sceneCamera.transform.forward * 1000.0f, hyperDst, "Red Leader");
            squadron.Add(xwing);
            cameraFocus = xwing;
        }
        if (elapsedTime == 10.0f)
        {
            this.setPlayer(xwing);
            
                _ship = hyperSpawn(npcs[0], cameraFocus.transform.position + cameraFocus.transform.forward * -1000.0f + cameraFocus.transform.right * -20.0f, cameraFocus.transform.position + cameraFocus.transform.right * -20.0f + cameraFocus.transform.forward * 230.0f, "Red 2");
                //_ship.GetComponent<NPCDispatcher>().setThrottle(0.5f);
                squadron.Add(_ship);
          
        }
        /*
        if (elapsedTime == 8.5f)
        {
            cameraFocus.GetComponent<NPCDispatcher>().disableFlybySound();
            sceneCamera.transform.position = hyperDst + cameraFocus.transform.forward * 50.0f + cameraFocus.transform.up*20.0f + cameraFocus.transform.right * 20.0f;
            cameraKeepEyeOn = true;
            cameraFocus.GetComponent<NPCDispatcher>().setThrottle(0.5f);

        }
        if (elapsedTime == 13.0f)
        {
            cameraKeepEyeOn = false;
            
            //starField = Instantiate(_starField);
            //starField.transform.parent = sceneCamera.transform;
            
            sceneCamera.transform.parent = cameraFocus.transform;
            //sceneCamera.transform.LookAt(transform.right * -0.25f);
            //cameraOffset = transform.right * 5.0f + transform.forward * 2.0f + transform.up;
            sceneCamera.transform.localPosition = transform.right * 3.0f + transform.forward * 5.0f + transform.up;
            sceneCamera.transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
        }
        if (elapsedTime == 16.00f)
        {
            _ship = hyperSpawn(npcs[0], cameraFocus.transform.position + cameraFocus.transform.forward * -1000.0f + cameraFocus.transform.right * -20.0f, cameraFocus.transform.position + cameraFocus.transform.right * -20.0f + cameraFocus.transform.forward * 230.0f, "Red 2");
            _ship.GetComponent<NPCDispatcher>().setThrottle(0.5f);
            squadron.Add(_ship);
        }
        if (elapsedTime == 16.5f)
        {
            _ship = hyperSpawn(npcs[0], cameraFocus.transform.position + cameraFocus.transform.forward * -1000.0f + cameraFocus.transform.up * 5.0f + cameraFocus.transform.right * -40.0f, cameraFocus.transform.position + cameraFocus.transform.up * 5.0f + cameraFocus.transform.right * -40.0f + cameraFocus.transform.forward * 230.0f, "Red 3");
            _ship.GetComponent<NPCDispatcher>().setThrottle(0.5f);
            squadron.Add(_ship);

        }
        if (elapsedTime == 20.0f)
        {
            hud.setOsdMsg("(Radio/Iain) Red 2: Howdy Red Leader!", 5.0f);
        }
        if (elapsedTime == 25.0f)
        {
            hud.setOsdMsg("(Radio/Carlos) Red 3: Red 3 standing by", 5.0f);
        }
        if (elapsedTime == 28.0f)
        {
            hud.setOsdMsg("(Radio/you) Red Leader: Iain did you get rid of that wart?", 4.0f);
        }
        if (elapsedTime == 32.0f)
        {
            hud.setOsdMsg("(Radio/Iain) Red 2: Nope, doctor gave me some pills.\r\nShould clear up in a day or two", 4.0f);
        }
        if (elapsedTime == 35.0f)
        {
            _ship = Instantiate(npcs[1]);
            _ship.transform.position = squadron[0].transform.position + squadron[0].transform.forward * 5000.0f;
            _ship.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
        }
        if (elapsedTime > 38.0f && elapsedTime< 41.0f)
        {

            sceneCamera.transform.rotation = Quaternion.RotateTowards(sceneCamera.transform.rotation, Quaternion.LookRotation(_ship.transform.position - squadron[0].transform.position), 1.0f);
        }
        if (elapsedTime == 45.0f)
        {
            sceneCamera.GetComponent<Animator>().enabled = true; 
        }
        if (elapsedTime == 65.0f)
        {
            player = Instantiate(_player);
            player.transform.position = squadron[0].transform.position;
            player.transform.rotation = squadron[0].transform.rotation;
            sceneCamera.enabled = false;
            sceneCamera.GetComponent<AudioListener>().enabled = false;
            player.GetComponent<Thrusters>().setSpeed(120.0f);
            player.GetComponent<control>().setCockpitView();
            Destroy(squadron[0]);
        }
*/
    }

    private void Update()
    {
        if (cameraKeepEyeOn)
        {
            sceneCamera.transform.LookAt(cameraFocus.transform.position);
        }
        if (camerafollow)
        {
            sceneCamera.transform.position = cameraFocus.transform.position + cameraOffset;
        }
    }

    // fadeInOrOut: false for fadeIn, true for fadeout
    private void doFade(float delay, float duration, bool fadeInOrOut)
    {
        GetComponent<FadeManager>().fade(delay, fadeInOrOut, duration);
    }

    private GameObject hyperSpawn(GameObject _what, Vector3 _start, Vector3 _finish, string callSign)
    {
        GameObject what = Instantiate(_what);
        what.GetComponent<ShipMeta>().callSign = callSign;
        hud.setOsdMsg(callSign + " entered from hyperspace");
        what.transform.position = _start;
        what.GetComponent<NPCDispatcher>().hyperIn(_finish);
        return what;
    }
}
