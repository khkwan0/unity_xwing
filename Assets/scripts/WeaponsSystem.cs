using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsSystem : MonoBehaviour {

    public Transform[] muzzles;
    public GameObject laser;

    public float laserSpeed;

    public AudioSource laserSound;

    private float shootTimeout;
    private int hotCannon;
	void Start () {
        hotCannon = 0;
	}

	void Update () {
		
	}

    public void shootSingle()
    {
        if (shootTimeout <= 0.0f)
        {
            GameObject _laser = (GameObject)Instantiate(laser, muzzles[hotCannon]);
            if (laserSound != null)
            {
                laserSound.Play();
            }
            _laser.GetComponent<Rigidbody>().AddForce(muzzles[hotCannon].forward * laserSpeed);
            Destroy(_laser, 4.0f);
            if (hotCannon == 0)
            {
                hotCannon = 1;
            }
            else
            {
                hotCannon = 0;
            }
            shootTimeout = 0.2f;
        } else
        {
            shootTimeout -= Time.fixedDeltaTime;
        }
    }

    public void shootAll()
    {
        for (int i=0; i<muzzles.Length; i++)
        {
            GameObject _laser = Instantiate(laser, muzzles[i]);
            _laser.GetComponent<Rigidbody>().AddForce(muzzles[i].forward * laserSpeed);
            Destroy(_laser, 4.0f);
        }
    }
}
