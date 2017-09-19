using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour {

    public List<AudioSource> music;

    public List<GameObject> ships;
    private GameObject _ship;

    public GameObject player;

    private HUDControl hudControl;
    private HUDFeedback hudMessage;

    [SerializeField]
    private string minutes;
    [SerializeField]
    private string seconds;

    [SerializeField]
    private float elapsedTime;

    private string msg;

    private shoot weapons;
    private Thrusters thrusters;
    private control _control;
    private int destroyCount;
    [SerializeField]
    private List<float> destroyCountTime;
    private float stepTime;
    private float displayTime;
    private float oldTimeScale;
    private AudioSource backgroundMusic;
    private bool _markTime;
    private float endTime;

    private bool canFinish;
	// Use this for initialization
	void Start () {
        hudControl = player.transform.Find("HUD").GetComponent<HUDControl>();
        hudMessage = GameObject.Find("feedback").GetComponent<HUDFeedback>();
        _control = player.GetComponent<control>();
        weapons = player.GetComponent<shoot>();
        minutes = Mathf.Floor(Time.fixedTime / 60).ToString("00");
        seconds = (Time.fixedTime % 60).ToString("00");
        destroyCount = 0;
        //hudControl.appendToLog(new Color(0.7f, 0.1f, 0.9f), minutes + ":" + seconds + "Mission started\n");
        oldTimeScale = Time.timeScale;
        if (music[0] != null) 
        {
            backgroundMusic = Instantiate(music[0]);
            backgroundMusic.Play();
        }
        canFinish = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        elapsedTime = Time.fixedTime;
        minutes = Mathf.Floor(Time.fixedTime / 60).ToString("00");
        seconds = (Time.fixedTime % 60).ToString("00");
        displayTime = 5.0f;
        if (_markTime)
        {
            endTime = elapsedTime;
            _markTime = false;
        }
        if (elapsedTime == 0.0f)
        {
            hudControl.appendToLog(Color.white, minutes + ":" + seconds + "  Mission started\n");
            weapons.disableWeapons();
            _control.disableThrottleControl();
        }
        if (elapsedTime == 5.0f)
        {
            msg = "Welcome to My Xwing";
        }
        if (elapsedTime == 10.0f)
        {
            msg = "You can use WASD, the cursor keys or a gamepad\r\n to control your Xwing";
        }
        if (elapsedTime == 25.0f)
        {
            msg = "Press 2 for Chase Cam View";
        }
        if (elapsedTime == 30.0f)
        {
            msg = "1 to go back to Cockpit View";
        }
        if (elapsedTime == 35.0f)
        {
            msg = "Here comes a drone target";
            backgroundMusic.Stop();
            backgroundMusic = Instantiate(music[1]);
            backgroundMusic.Play();
        }
        if (elapsedTime == 40.0f)
        {
            msg = "Tie Fighter (Drone) entered area from hyperspace";
            _ship = hyperSpawn(ships[0], new Vector3(-451.0f * 10.0f, 6.3f, 104.0f), new Vector3(451.0f, 6.3f, 104.0f));
        }
        if (elapsedTime == 46.0f)
        {
            msg = "Select the drone by pressing T";
        }
        if (elapsedTime == 50.0f)
        {
            msg = "A targetted object will always be tracked on screen\r\nand on your forward and rear radars\r\nlocated at the top left and top right of\r\nyour HUD";
        }
        if (elapsedTime == 60.0f)
        {
            msg = "Target the drone by changing direction.";
        }
        if (elapsedTime == 70.0f)
        {
            msg = "Your crosshairs will change color when you\r\nhave a good firing solution";
        }
        if (elapsedTime == 80.0f)
        {
            msg = "You are weapons free.\r\nLeft Ctrl to fire";
            weapons.enableWeapons();
        }
        if (destroyCount == 1)
        {
            stepTime = destroyCountTime[0];
            if (elapsedTime == (stepTime + 1.0f)) {
                msg = "Well Done!";
            }
            if (elapsedTime == (stepTime + 5.0f)) {
                backgroundMusic.Stop();
                backgroundMusic.Play();
                msg = "Here are two more drones\r\nDestroy them";
                hyperSpawn(ships[0], new Vector3(-451.0f * 10.0f, 6.3f, 104.0f), new Vector3(451.0f, 6.3f, 104.0f));
            }
            if (elapsedTime == (stepTime + 5.5f))
            {
                hyperSpawn(ships[0], new Vector3(-451.0f * 10.0f, 6.3f, 104.0f), new Vector3(421.0f, 26.3f, 120.0f));
            }
            if (elapsedTime == stepTime + 7.0f)
            {
                msg = "You can select the nearest enemy with R";
            }
        }
        if (destroyCount == 3)
        {
            stepTime = destroyCountTime[2];
            if (elapsedTime == (stepTime + 1.0f))
            {
                msg = "NICE!";
            }
            if (elapsedTime == (stepTime + 4.0f))
            {
                msg = "How about a moving target?";
                backgroundMusic.Stop();
                backgroundMusic.Play();
            }
            if (elapsedTime == (stepTime + 7.0f))
            {
                hyperSpawn(ships[0], new Vector3(-451.0f * 10.0f, 6.3f, 104.0f), new Vector3(451.0f, 6.3f, 104.0f)).GetComponent<Thrusters>().setOneThirdThrottle();
            }
            if (elapsedTime == (stepTime + 9.0f))
            {
                _control.enableThrottleControl();
                msg = "Engines ONLINE";
            }
            if (elapsedTime == (stepTime + 10.0f))
            {
                msg = "Press '[' to set your engines to 1/3 throttle";
            }
            if (elapsedTime == (stepTime + 20.0f))
            {
                msg = "Target the bogey (R or T) and give chase!";
            }
            if (elapsedTime == (stepTime + 25.0f))
            {
                msg = "Backpsace for full throttle\r\n'[' for 1/3 throttle\r\n']'for 2/3 throttle\r\n'\\' for no throttle";
                displayTime = 15.0f;
            }
            if (elapsedTime == (stepTime + 40.0f))
            {
                msg = "If you have targetted the enemy ship (R or T)\r\nYou can match your speed with the target\r\nby pressing ENTER";
            }
        }
        if (destroyCount == 4)
        {
            stepTime = destroyCountTime[destroyCount - 1];
            if (elapsedTime == (stepTime + 1.0f))
            {
                msg = "Nice shooting ace!";
                backgroundMusic.Stop();

                backgroundMusic.Play();
                
            }
            if (elapsedTime == (stepTime + 5))
            {
                msg = "You're going to be the Rebels number one pilot\r\nin no time!";
            }
            if (elapsedTime == (stepTime + 10.0f))
            {
                msg = "Ok, last one...a hostile target";
            }
            if (elapsedTime == (stepTime + 12.0f))
            {
                _ship = hyperSpawn(ships[0], new Vector3(-451.0f * 10.0f, 6.3f, 104.0f), new Vector3(451.0f, 6.3f, 104.0f));
                _ship.GetComponent<Thrusters>().setOneThirdThrottle();
                _ship.GetComponent<tiefighter_ai>().setTarget(player);
                _ship.GetComponent<tiefighter_ai>().setStateAttack();
                msg = "Watch your six.  Good luck";
                backgroundMusic.Stop();
                backgroundMusic.Play();
            }
        }
        if (destroyCount == 5)
        {
            stepTime = destroyCountTime[destroyCount - 1];
            if (elapsedTime == (stepTime + 1.0f))
            {
                msg = "Way to go!";
                backgroundMusic.Stop();
                backgroundMusic = Instantiate(music[2]);
                backgroundMusic.Play();

            }
            if (elapsedTime == (stepTime + 6.0f))
            {
                msg = "You have proved yourself as an X-Wing pilot";
            }
            if (elapsedTime == (stepTime + 15.0f))
            {
                msg = "It is time to join the fight";
            }
            if (elapsedTime == (stepTime + 24.0f))
            {
                msg = "Hyperdrive activated.  Press J to end the mission";
                canFinish = true;
            }
        }
        if (canFinish)
        {
            if (elapsedTime == (endTime + 6.0f))
            {
                msg = "Thanks for playing";
            }
            if (elapsedTime == (endTime + 11.0f))
            {
                msg = "Programmer: Ken";
            }
            if (elapsedTime == (endTime + 16.0f))
            {
                msg = "Press Q to quit";
            }
            if (msg != null)
            {
                displayMessage(msg, displayTime);
                msg = null;
            }
        }
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0.0f)
            {
                Time.timeScale = oldTimeScale;
                if (backgroundMusic != null)
                {
                    backgroundMusic.UnPause();
                }
            } else
            {
                Time.timeScale = 0.0f;
                if (backgroundMusic != null)
                {
                    backgroundMusic.Pause();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.J) && canFinish)
        {
            this.endMission();
        }
        if (Input.GetKeyDown(KeyCode.Q) && canFinish) {
            Application.Quit();
        }
    }

    private void displayMessage(string _msg, float time = 5.0f, bool eventLog = true, bool mainHud = true)
    {
        if (eventLog)
        {
            hudControl.appendToLog(Color.white, minutes + ":" + seconds + "  " + _msg + "\n");
        }
        if (mainHud)
        {
            hudMessage.setText(_msg, time);
        }
    }

    private GameObject hyperSpawn(GameObject _ship, Vector3 origin, Vector3 destination)
    {
        GameObject _newShip = GameObject.Instantiate(_ship, origin, Quaternion.LookRotation(destination - origin, new Vector3(0.0f, 1.0f, 0.0f)));
        _newShip.GetComponent<tiefighter_ai>().hyperTo(destination);
        return _newShip;
    }

    public void destroyNotify(GameObject _go)
    {
        destroyCount++;
        destroyCountTime.Add(Mathf.Floor(Time.fixedTime));
    }

    private void endMission()
    {
        player.GetComponent<control>().hyperOut();
        this.markEndTime();
    }

    public void markEndTime()
    {
        _markTime = true;
    }
}
