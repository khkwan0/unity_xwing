using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CrosshairControl : MonoBehaviour {

    // Use this for initialization
    private RawImage normal;
    private RawImage target;

    public AudioSource s_target;
    private bool played;
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

        normal = transform.Find("crosshairs_n").GetComponent<RawImage>();
        target = transform.Find("crosshairs_t").GetComponent<RawImage>();
        played = false;
        _enabled = true;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void enableNormal()
    {
        if (_enabled)
        {
            normal.enabled = true;
            target.enabled = false;
            played = false;
        } else
        {
            normal.enabled = target.enabled = false;
        }
    }

    public void enabledTarget()
    {
        if (_enabled)
        {
            normal.enabled = false;
            target.enabled = true;
            if (s_target != null && !played)
            {
                s_target.Play();
                played = true;
            }
        } else
        {
            normal.enabled = target.enabled = false;
        }
    }
}
