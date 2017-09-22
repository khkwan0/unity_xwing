using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Radar : MonoBehaviour {

    public GameObject frontRadarPanel;
    public GameObject rearRadarPanel;
    public Camera referenceCamera;
    public GameObject blip;
    public GameObject targettedBlip;
    public GameObject reticle;
    public GameObject rearReticle;

    [SerializeField]
    private List<GameObject> friends;
    [SerializeField]
    private List<GameObject> foes;
    private List<GameObject> others = new List<GameObject>();
    private LinkedList<GameObject> all = new LinkedList<GameObject>();

    private LinkedListNode<GameObject> n_all;

    [SerializeField]
    private Vector3 loc;
    [SerializeField]
    private Vector3 _v_scale;
    [SerializeField]
    private Vector3 screenLoc;
    [SerializeField]
    private Vector3 reticlePosition;

    private Color c_foe = new Color(1.0f, 0.0f, 0.0f);
    private Color c_friend = new Color(0.0f, 1.0f, 0.0f);
    private Color c_other = new Color(0.0f, 0.5f, 0.5f);

    [SerializeField]
    private float r_frontWidth;
    [SerializeField]
    private float r_frontHeight;

    private GameObject _targettedBlip;

    public bool _enabled;
    private GameObject _target;
    private bool targetted;
    public Camera targetCamera;

    private HUDController2 hud;

	void Start () {

        // get transform parameters
        r_frontWidth = frontRadarPanel.GetComponent<RectTransform>().rect.width;
        r_frontHeight = frontRadarPanel.GetComponent<RectTransform>().rect.height;

        // determine scale vector from screen to panel(radar display) coordinates.
        _v_scale = new Vector3(r_frontWidth / Screen.width, r_frontHeight / Screen.height, 1.0f);

        reticle.GetComponent<Image>().enabled = false;
        rearReticle.GetComponent<Image>().enabled = false;
        _enabled = true;
        hud = gameObject.GetComponent<HUDController2>();
        _target = null;
    }

    private void drawBlip(GameObject _targetRadar, Color _color, Vector3 _loc, bool _isFront, bool targetted, GameObject _target)
    {
        GameObject _blip = Instantiate(blip);
        _blip.transform.SetParent(_targetRadar.transform);
        _blip.GetComponent<Image>().color = _color;

        if (_isFront) { 
            // adjust for anchors
            _loc.z = 0.0f;
            _loc.y = _loc.y - 200.0f;

            // clamp
            if (_loc.y < -198.0f)
            {
                _loc.y = -198.0f;
            }
            if (_loc.y > -2.0f)
            {
                _loc.y = -2.0f;
            }
            if (_loc.x < 2.0f)
            {
                _loc.x = 2.0f;
            }
            if (_loc.x > 198.0f)
            {
                _loc.x = 198.0f;
            }
        } else
        {
            _loc.z = 0.0f;
            //_loc.y *= -1.0f;
            _loc.y -= 200.0f;            
            _loc.x = _loc.x - 200.0f;
            if (_loc.y < -198.0f)
            {
                _loc.y = -198.0f;
            }
            if (_loc.y > -2.0f)
            {
                _loc.y = -2.0f;
            }
            if (_loc.x < -198.0f)
            {
                _loc.x = -198.0f;
            }
            if (_loc.x > -2.0f)
            {
                _loc.x = -2.0f;
            }
        }
        _blip.transform.localPosition = _loc;
        if (targetted)
        {
            _targettedBlip = Instantiate(targettedBlip);
            _targettedBlip.transform.SetParent(_targetRadar.transform);
            _targettedBlip.transform.localPosition = _loc;
            this.drawIndicator(_target);
        }

    }

    private void clearRadar(GameObject _targetRadar)
    {
        foreach (Transform child in _targetRadar.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void drawIndicator(GameObject ship)
    {
        if (ship.GetComponent<ShipMeta>().isTargetted())
        {

            if (screenLoc.z > 0.0f)
            {
                reticle.GetComponent<Image>().enabled = true;
                rearReticle.GetComponent<Image>().enabled = false;

                // adjust for anchors
                reticlePosition = screenLoc - new Vector3(Screen.width / 2, Screen.height / 2, 0.0f);
                reticlePosition.z = 0.0f;
                reticle.transform.localPosition = reticlePosition;
            } else
            {
                reticle.GetComponent<Image>().enabled = false;
                rearReticle.GetComponent<Image>().enabled = true;
                reticlePosition = screenLoc - new Vector3(Screen.width / 2, Screen.height / 2, 0.0f);
                reticlePosition.z = 0.0f;
                if (reticlePosition.y < 0.0f)
                {
                    rearReticle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    reticlePosition.y = Screen.height / 2.0f;
                } else
                {
                    rearReticle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                    reticlePosition.y = -1.0f * Screen.height / 2.0f;
                }

                rearReticle.transform.localPosition = reticlePosition;
            }
            
            // the target is just outside of the viewport for both front _and_ rear projection
            if (screenLoc.x > Screen.width || screenLoc.x < 0.0f || screenLoc.y > Screen.height || screenLoc.y < 0.0f) { 
                reticle.GetComponent<Image>().enabled = false;
                rearReticle.GetComponent<Image>().enabled = true;

                // adjust for anchors
                reticlePosition = screenLoc - new Vector3(Screen.width / 2, Screen.height / 2, 0.0f);
                if (screenLoc.z > 0.0f)
                {
                    if (screenLoc.y > Screen.height)
                    {
                        rearReticle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        reticlePosition.y = Screen.height / 2.0f;
                    }
                    if (screenLoc.y < 0.0f)
                    {
                        rearReticle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                        reticlePosition.y = -1.0f * Screen.height / 2.0f;
                    }
                    if (screenLoc.x > Screen.width)
                    {
                        rearReticle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                        reticlePosition.x = Screen.width / 2.0f;
                    }
                    if (screenLoc.x < 0.0f)
                    {
                        rearReticle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                        reticlePosition.x = -1.0f * Screen.width / 2.0f;
                    }
                } else
                {
                    if (reticlePosition.x < -1.0f * Screen.width /2.0f)
                    {
                        rearReticle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                        reticlePosition.x = -1.0f * Screen.width/2.0f;
                    }
                    if (reticlePosition.x > Screen.width/2.0f)
                    {
                        rearReticle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                        reticlePosition.x = Screen.width/2.0f;
                    }
                    if (reticlePosition.y > Screen.height / 2.0f)
                    {
                        rearReticle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                        reticlePosition.y = -1.0f * Screen.height / 2.0f;
                    }
                    if (reticlePosition.y < -1.0f * Screen.height /2.0f)
                    {
                        rearReticle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        reticlePosition.y = Screen.height / 2.0f;
                    }
                }
                rearReticle.transform.localPosition = reticlePosition;
            }
        }
        else
        {
            reticle.GetComponent<Image>().enabled = false;
            rearReticle.GetComponent<Image>().enabled = false;
        }
    }

	void Update () {
        clearRadar(frontRadarPanel);
        clearRadar(rearRadarPanel);
        targetted = false;  
        if (_enabled)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _target = this.getClosestEnemy();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                _target = this.getNextTarget();
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _target = this.getPrevTarget();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                _target = null;
            }

            if (_target != null)
            {
                hud.setTarget(_target);
            } else
            {
                hud.deTarget();
            }

            if (_target != null)
            {
                GameObject _targetted = _target;
                float _dist;
                float _scaleFactor;
                Vector3 _int;
                Vector3 _forward;
                float _dot;
                GameObject _ship;

                _ship = hud.getCurrentShip();

                // place the target camera
                targetCamera.enabled = true;
                targetCamera.transform.rotation = Quaternion.LookRotation(_targetted.transform.position - _ship.transform.position);
                _dist = Vector3.Distance(_targetted.transform.position, _ship.transform.position);
                _scaleFactor = 10.0f;
                targetCamera.transform.position = _ship.transform.position + (targetCamera.transform.forward * _dist) - (targetCamera.transform.forward * _scaleFactor);

                //place the chase camera                
                //GameObject.Find("TargetChaseCamera").transform.SetParent(_targetted.transform);
                //_targetted.transform.Find("TargetChaseCamera").transform.localPosition = new Vector3(0.0f, 5.0f, -50.0f);

                // determine lead/intercept point
                _int = FindInterceptVector(_ship.transform.position, 320.0f, _targetted.transform.position, _targetted.GetComponent<Rigidbody>().velocity);
                _forward = _ship.transform.forward;
                _dot = Vector3.Dot(_int.normalized, _forward);
                if (_dot > 0.9998f)
                {
                    hud.setTargetCrosshairs();
                }
                else
                {
                    hud.setNormalCrosshairs();
                }
            }
            else
            {
                hud.setNormalCrosshairs();
            }


            foreach (GameObject foe in foes)
            {
                if (foe != null)
                {
                    loc = referenceCamera.WorldToScreenPoint(foe.transform.position);
                    screenLoc = loc;
                    loc.Scale(_v_scale);
                    if (foe == _target)
                    {
                        targetted = true;
                    }
                    if (loc.z > 0.0f)
                    {
                        drawBlip(frontRadarPanel, c_foe, loc, true, targetted, foe);
                    }
                    else
                    {
                        drawBlip(rearRadarPanel, c_foe, loc, false, targetted, foe);
                    }

                }
            }
            foreach (GameObject friend in friends)
            {
                if (friend != null && !friend.GetComponent<ShipControl>().isPlayerContolled())
                {
                    loc = referenceCamera.WorldToScreenPoint(friend.transform.position);
                    loc.Scale(_v_scale);
                    if (friend == _target) targetted = true;
                    if (loc.z > 0.0f)
                    {
                        drawBlip(frontRadarPanel, c_friend, loc, true, targetted, friend);
                    }
                    else
                    {
                        drawBlip(rearRadarPanel, c_friend, loc, false, targetted, friend);
                    }
                }
            }
        }
	}

    public void register(GameObject _object)
    {
        if (_object.GetComponent<ShipMeta>().foe)
        {
            foes.Add(_object);
        } else if (_object.GetComponent<ShipMeta>().friend)
        {
            friends.Add(_object);
        } else
        {
            others.Add(_object);
        }
        all.AddLast(_object);
        n_all = all.Last;
    }

    public void deRegister(GameObject _object)
    {
        if (_object.GetComponent<ShipMeta>().foe)
        {
            foes.Remove(_object);
        }
        else if (_object.GetComponent<ShipMeta>().friend)
        {
            friends.Remove(_object);
        }
        else
        {
            others.Remove(_object);
        }
        all.Remove(_object);
        n_all = all.Last;
    }

    public GameObject getClosestEnemy()
    {
        GameObject rv = null;
        if (foes.Count > 0)
        {
            rv = foes[0];
            if (foes.Count > 1)
            {
                foreach (GameObject foe in foes)
                {
                    if (foe != rv)
                    {
                        if (Vector3.Distance(transform.parent.transform.position, foe.transform.position) < Vector3.Distance(transform.parent.transform.position, rv.transform.position))
                        {
                            rv = foe;
                        }
                    }
                }
            }
        }
        return rv;
    }

    public GameObject getNextTarget()
    {
        GameObject rv = null;

        n_all = n_all.Next;

        if (n_all == null)
        {
            n_all = all.First;
        }
        if (n_all.Value.GetComponent<ShipControl>().isPlayerContolled()) n_all = n_all.Next;
        if (n_all == null)
        {
            n_all = all.First;
        }
        if (!n_all.Value.GetComponent<ShipControl>().isPlayerContolled())
        {
            rv = n_all.Value;
        }
        return rv;
    }

    public GameObject getPrevTarget()
    {
        GameObject rv = null;

        n_all = n_all.Previous;
        if (n_all == null)
        {
            n_all = all.Last;
        }
        if (n_all.Value.GetComponent<ShipControl>().isPlayerContolled()) n_all = n_all.Previous;
        if (n_all == null)
        {
            n_all = all.Last;
        }
        if (!n_all.Value.GetComponent<ShipControl>().isPlayerContolled())
        {
            rv = n_all.Value;
        }
        return rv;
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
