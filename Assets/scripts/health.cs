using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour {

    public int hp;
    public int shields;
    public int maxShields;
    public int maxHp;

    public AudioSource explosionSfx;

    public GameObject explosionGfx;
    private bool dead = false;
    private float timeLeft = 2.2f;

    private GameObject _driver;
	
	// Update is called once per frame
	void Update () {
        if (dead)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0.0f)
            {
                Destroy(transform.gameObject);
            }
        }
	}

    public void doDamage(int amt)
    {
        int remaining = 0;
        if (shields > 0)
        {
            if (amt>shields)
            {
                remaining = amt - shields;
                shields = 0;
            } else
            {
                shields -= amt;
            }
        } else
        {
            if (remaining > 0)
            {
                hp -= remaining;
            } else
            {
                hp -= amt;
            }
            if (hp<=0)
            {
                hp = 0;
                dead = true;
                explosionSfx.Play();
                Collider[] colliders = transform.gameObject.GetComponents<Collider>();
                foreach(Collider collider in colliders)
                {
                    collider.enabled = false;
                }
                gameObject.GetComponent<ShipMeta>().deTarget();
                GameObject.FindGameObjectsWithTag("driver")[0].GetComponent<Driver>().destroyNotify(gameObject);
                GameObject.Find("HUD").GetComponent<Radar>().deRegister(gameObject);
                GameObject.Find("HUD").GetComponent<HUDControl>().clearReticles();
                Destroy(transform.Find("model").transform.gameObject);
                explosionGfx.GetComponent<ParticleSystem>().Play(true);
            }
        }
    }
}
