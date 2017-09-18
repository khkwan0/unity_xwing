using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUDFeedback : MonoBehaviour {

    // Use this for initialization
    private Text text;
    private float timeout;
	void Start () {
        text = GetComponent<Text>();
        text.horizontalOverflow = HorizontalWrapMode.Wrap;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (timeout < 0.0f)
        {
            text.text = "";
        } else
        {
            timeout -= Time.fixedDeltaTime;
        }
	}

    public void setText(string _text)
    {
        text.text = _text;
        timeout = 2.0f;
    }

    public void setText(string _text, float _timeout)
    {
        text.text = _text;
        timeout = _timeout;
    }
}
