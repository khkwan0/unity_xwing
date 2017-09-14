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

    [SerializeField]
    private bool _enabled;

    public bool Enabled
    {
        get
        {
            return _enabled;
        }

        set
        {
            _enabled = value;
        }
    }
	void Start () {

        // get transform parameters
        r_frontWidth = frontRadarPanel.GetComponent<RectTransform>().rect.width;
        r_frontHeight = frontRadarPanel.GetComponent<RectTransform>().rect.height;

        // determine scale vector from screen to panel(radar display) coordinates.
        _v_scale = new Vector3(r_frontWidth / Screen.width, r_frontHeight / Screen.height, 1.0f);

        reticle.GetComponent<Image>().enabled = false;
        rearReticle.GetComponent<Image>().enabled = false;
    }

    private void drawBlip(GameObject _targetRadar, Color _color, Vector3 _loc, bool _isFront, bool targetted)
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
	// Update is called once per frame
	void Update () {
        clearRadar(frontRadarPanel);
        clearRadar(rearRadarPanel);
        if (_enabled)
        {
            foreach (GameObject foe in foes)
            {
                drawIndicator(foe);
                if (foe != null)
                {
                    loc = referenceCamera.WorldToScreenPoint(foe.transform.position);
                    screenLoc = loc;
                    loc.Scale(_v_scale);
                    if (loc.z > 0.0f)
                    {
                        drawBlip(frontRadarPanel, c_foe, loc, true, foe.GetComponent<ShipMeta>().isTargetted());
                    }
                    else
                    {
                        drawBlip(rearRadarPanel, c_foe, loc, false, foe.GetComponent<ShipMeta>().isTargetted());
                    }

                }
            }
            foreach (GameObject friend in friends)
            {
                if (friend != null)
                {
                    loc = referenceCamera.WorldToScreenPoint(friend.transform.position);
                    loc.Scale(_v_scale);
                    if (loc.z > 0.0f)
                    {
                        drawBlip(frontRadarPanel, c_friend, loc, true, friend.GetComponent<ShipMeta>().isTargetted());
                    }
                    else
                    {
                        drawBlip(rearRadarPanel, c_friend, loc, false, friend.GetComponent<ShipMeta>().isTargetted());
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
        rv = n_all.Value;
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
        rv = n_all.Value;
        return rv;
    }
}
