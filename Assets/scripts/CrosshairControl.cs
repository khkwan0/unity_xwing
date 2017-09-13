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
	void Start () {

        normal = transform.Find("crosshairs_n").GetComponent<RawImage>();
        target = transform.Find("crosshairs_t").GetComponent<RawImage>();
        played = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void enableNormal()
    {
        normal.enabled = true;
        target.enabled = false;
        played = false;
    }

    public void enabledTarget()
    {
        normal.enabled = false;
        target.enabled = true;
        if (s_target != null && !played)
        {
            s_target.Play();
            played = true;
        }
    }
}
