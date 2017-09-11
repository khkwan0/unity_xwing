using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiefighter_ai : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private float _time;

    private directional_control dirControl;
    private Thrusters thrusters;
    private WeaponsSystem weapons;

    [SerializeField]
    private GameObject target;

    private GameObject aiCollider;

    [SerializeField]
    private float dotProduct;

    [SerializeField]
    private float distanceToTarget;

    enum __state
    {
        idle,
        attack,
        flee,
        evade
    }

    [SerializeField]
    private __state _state;

    [SerializeField]
    float timeout;
    
	void Start () {
        //gameObject.GetComponent<Thrusters>().setFullThrottle();
        dirControl = gameObject.GetComponent<directional_control>();
        thrusters = gameObject.GetComponent<Thrusters>();
        weapons = gameObject.GetComponent<WeaponsSystem>();
     
    }

    private void OnTriggerStay(Collider other)
    {
        timeout = 3.0f;
        _state = __state.evade;
    }

    // Update is called once per frame
    void Update () {
        determineWhatToDo();
    }

    public void setTarget(GameObject _target)
    {
        target = _target;
    }
    void determineWhatToDo()
    {
        if (target)
        {
            distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        }
        if (_state == __state.attack)
        {
            thrusters.setOneThirdThrottle();
            dirControl.stopTurning();
            dirControl.turnTowardsTarget(target);
            dotProduct = Vector3.Dot(transform.forward, target.transform.forward);
            if (Mathf.Abs(dotProduct) > 0.98 && distanceToTarget< 300.0f)
            {
                weapons.shootSingle();
            }
        }
        if (_state == __state.evade)
        {
           
            thrusters.setOneThirdThrottle();
            dirControl.yawRight();
            dirControl.pitchUp();
       
            this.setTarget(GameObject.Find("player_xwing"));

            if (timeout <= 0.0f)
            {
                _state = __state.attack;
            }
            else
            {
                timeout -= Time.fixedDeltaTime;
            }
        }

    }
}
