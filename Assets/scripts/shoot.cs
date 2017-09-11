using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour {

    public GameObject laser;
    public Transform[] bulletSpawn;
    public float speed;
    public AudioSource laserSoundBasic;

    enum _firingConfiguration
    {
        oneCannon,
        twoCannons,
        allCannons
    }

    private int hotCannonIndex;

    private float _shootTimeout;
    [SerializeField]
    private float _laserLifetime = 4.0f;

    private _firingConfiguration firingConfiguration;
    // Use this for initialization
    void Start () {
        firingConfiguration = _firingConfiguration.oneCannon;
	}
     
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetButton("Fire1"))
        {
            if (_shootTimeout <= 0.0f)
            {

                StartCoroutine("Fire");
                //altFire();
                switch (firingConfiguration) {
                    case _firingConfiguration.oneCannon: _shootTimeout = 0.2f; break;
                    case _firingConfiguration.twoCannons: _shootTimeout = 0.5f; break;
                    case _firingConfiguration.allCannons: _shootTimeout = 1.0f; break;
                }
            }
            _shootTimeout -= Time.fixedDeltaTime;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            _shootTimeout = 0.0f;
            switch (firingConfiguration)
            {
                case _firingConfiguration.oneCannon: firingConfiguration = _firingConfiguration.twoCannons;break;
                case _firingConfiguration.twoCannons: firingConfiguration = _firingConfiguration.allCannons;break;
                case _firingConfiguration.allCannons: firingConfiguration = _firingConfiguration.oneCannon;break;
            }
        }
    }
    
    IEnumerator Fire()
    {
        if (firingConfiguration == _firingConfiguration.oneCannon)
        {
            if (!laserSoundBasic.isPlaying)
            {
                laserSoundBasic.Play();
            }
            var _laser = (GameObject)Instantiate(laser, bulletSpawn[hotCannonIndex]);
            _laser.GetComponent<Rigidbody>().AddForce(bulletSpawn[hotCannonIndex].forward * speed);
            hotCannonIndex++;
            if (hotCannonIndex>3)
            {
                hotCannonIndex = 0;
            }
            Destroy(_laser, _laserLifetime);
            yield return null;
        }
        else if (firingConfiguration == _firingConfiguration.twoCannons)
        {
            if (!laserSoundBasic.isPlaying)
            {
                laserSoundBasic.Play();
            }
            if (hotCannonIndex < 2)
            {
                var _laser = (GameObject)Instantiate(laser, bulletSpawn[hotCannonIndex]);
                _laser.GetComponent<Rigidbody>().AddForce(bulletSpawn[hotCannonIndex].forward * speed);
                var _laser2 = (GameObject)Instantiate(laser, bulletSpawn[hotCannonIndex+2]);
                _laser2.GetComponent<Rigidbody>().AddForce(bulletSpawn[hotCannonIndex].forward * speed);
                Destroy(_laser, _laserLifetime);
                Destroy(_laser2, _laserLifetime);
                hotCannonIndex++;
                if (hotCannonIndex > 1)
                {
                    hotCannonIndex = 0;
                }
            } else
            {
                hotCannonIndex = 0;
            }
        }
        else
        {
            int i;

            if (!laserSoundBasic.isPlaying)
            {
                laserSoundBasic.Play();
            }
            for (i = 0; i < 4; i++)
            {
                var _laser = (GameObject)Instantiate(laser, bulletSpawn[i]);
                _laser.GetComponent<Rigidbody>().AddForce(bulletSpawn[i].forward * speed);
                Destroy(_laser, _laserLifetime);
                yield return null;
            }
        }
    }   
    
    private void altFire()
    {
        int i;
        for (i = 0; i < 4; i++)
        {
            var _laser = (GameObject)Instantiate(laser, bulletSpawn[i]);
            _laser.GetComponent<Rigidbody>().AddForce(bulletSpawn[i].forward * speed);
            //Destroy(_laser, 1.5f);
        }
    }
}
