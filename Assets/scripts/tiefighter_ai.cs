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

    [SerializeField]
    private Vector3 _targetLead;

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
        timeout = 0.0f;
        _state = __state.evade;
    }

    // Update is called once per frame
    void FixedUpdate () {
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
            //dirControl.stopTurning();
            _targetLead = FindInterceptVector(transform.position, 320.0f, target.transform.position, target.transform.GetComponent<Rigidbody>().velocity);
            dirControl.turnTowardsTarget(_targetLead, 1.0f);
            dotProduct = Vector3.Dot(_targetLead.normalized, transform.forward);
            if (Mathf.Abs(dotProduct) > 0.999 && distanceToTarget< 450.0f)
            {
                weapons.shootSingle();
            }
        }
        if (_state == __state.evade)
        {
            /*
            thrusters.setOneThirdThrottle();
            dirControl.yawRight();
            dirControl.pitchUp();
            */
       
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
}
