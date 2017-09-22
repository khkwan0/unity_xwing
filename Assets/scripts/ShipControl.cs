using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour {

    public Camera sceneCamera;
    public bool triggerTakeOver;
    public GameObject cockpitCameraPos;

    private GameObject HUD;
    private Rigidbody rb;
    private ShipMeta ship;
    private Weapons weapons;
    private HUDController2 hud;
    private BottomHUDController bhud;

    private float _turnSpeed;
    private bool playerControlled;

	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        ship = gameObject.GetComponent<ShipMeta>();
        _turnSpeed = ship.maxTurnSpeed;
        weapons = gameObject.GetComponent<Weapons>();
        HUD = GameObject.Find("HUD");
        hud = HUD.GetComponent<HUDController2>();
        triggerTakeOver = false;
        if (sceneCamera == null)
        {
            sceneCamera = GameObject.Find("sceneCamera").GetComponent<Camera>();
        }
	}

    public void takeOverShip()
    {
        this.setPlayerControlled(true);
        this.alignCameraToCockpitView();
        hud.setShip(gameObject);
    }

    public void setPlayerControlled(bool _flag)
    {
        playerControlled = _flag;
        if (_flag)
        {
            this.setObjectLayer(gameObject.transform.Find("model").gameObject, 9);
            weapons.enableWeapons();

        } else
        {
            this.setObjectLayer(gameObject.transform.Find("model").gameObject, 1);
            weapons.disableWeapons();
        }
    }

    public bool isPlayerContolled()
    {
        return playerControlled;
    }

    public void setObjectLayer(GameObject _obj, int layer)
    {
        for (int i=0; i<_obj.transform.childCount; i++)
        {
            _obj.transform.GetChild(i).gameObject.layer = layer;
            this.setObjectLayer(_obj.transform.GetChild(i).gameObject, layer);
        }
    }

    private void alignCameraToCockpitView()
    {
        sceneCamera.transform.parent = gameObject.transform;
        sceneCamera.transform.position = cockpitCameraPos.transform.position;
    }
    public void alignCameraToCockpit(Camera camera)
    {
        camera.transform.parent = gameObject.transform;
        camera.transform.position = gameObject.transform.Find("cockpitCameraPos").transform.position;
    }

	void Update () {
        if (playerControlled)
        {
            rb.AddRelativeTorque(Input.GetAxis("Vertical") * _turnSpeed, Input.GetAxis("Horizontal") * _turnSpeed, 0.0f);
            rb.AddRelativeTorque(0.0f, 0.0f, Input.GetAxis("RollLeft") * _turnSpeed);
            rb.AddRelativeTorque(0.0f, 0.0f, -1.0f * Input.GetAxis("RollRight") * _turnSpeed);
        }
    }
}
