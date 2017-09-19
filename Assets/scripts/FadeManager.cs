using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour {

    public static FadeManager Instance { set; get; }

    public Image fadeImage;
    private bool isInTransition;
    private float transition;
    private bool isShowing;
    private float duration;
    private float delay;

	void Start () {
		
	}

    private void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update () {
        if (!isInTransition)
            return;
        if (delay > 0.0f)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            transition += (isShowing) ? Time.deltaTime / duration : -Time.deltaTime / duration;
            fadeImage.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transition);
            if (transition > 1 || transition < 0)
            {
                isInTransition = false;
            }
        }
	}

    public void fade(float delay, bool showing, float duration)
    {
        if (!isInTransition)
        {
            isShowing = showing;
            isInTransition = true;
            this.duration = duration;
            transition = isShowing ? 0 : 1;
            this.delay = delay;
        }
    }
}
