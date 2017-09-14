using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BottomHUDController : MonoBehaviour {

    // Use this for initialization

    private GameObject target;
    private Text shields;
    private Text hull;
    private health t_health;
    private Text distance;
    private Text system;
    private Text cargo;
    private Text speed;
    private string shieldPercent;
    private string cargoText;

	void Start () {
        shields = transform.Find("shields").GetComponent<Text>();
        hull = transform.Find("hull").GetComponent<Text>();
        distance = transform.Find("distance").GetComponent<Text>();
        system = transform.Find("system").GetComponent<Text>();
        cargo = transform.Find("cargo").GetComponent<Text>();
        speed = transform.Find("speed").GetComponent<Text>();

        cargoText = "Cargo: Unknown";
           
	}
	
	// Update is called once per frame
	void Update () {
		if (target)
        {
            if (t_health != null)
            {
                if (shields != null)
                {
                    shields.color = Color.green;
                    if (t_health.maxShields != 0)
                    {
                        shieldPercent = Mathf.Floor(100*t_health.shields / t_health.maxShields).ToString();
                    }
                    else
                    {
                        shieldPercent = "0";
                    }
                    shields.text = "SHDS: " + shieldPercent + "%";
                }
                if (hull != null)
                {
                    hull.color = Color.green;
                    hull.text = "HULL: " + Mathf.Floor(100*t_health.hp / t_health.maxHp).ToString() + "%";
                }
                if (cargo != null)
                {
                    cargo.color = Color.green;
                    cargo.text = cargoText;
                }
                distance.color = Color.green;

                // 3.0f is a scale factor from unity coordinate system to one that feels like MGLT (sublight speeds - Megalight)
                distance.text = "DST: " + (Vector3.Distance(target.transform.position, transform.root.transform.position) / 3.0f/ 100.0f).ToString("F2");

                speed.color = Color.green;
                speed.text = "SPD: " + target.GetComponent<Rigidbody>().velocity.magnitude.ToString("F2");
                
            }
        } else
        {
            shields.text = "";
            hull.text = "";
            distance.text = "";
            cargo.text = "";
            system.text = "";
            speed.text = "";
        }
	}

    public void setTarget(GameObject _target)
    {
        target = _target;
        if (target != null)
        {
            t_health = target.GetComponent<health>();
        }
    }

    public void deTarget()
    {
        target = null;
        t_health = null;
    }

    public void setCargoText(string _text)
    {
        cargoText = _text;
    }
}
