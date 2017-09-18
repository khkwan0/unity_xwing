using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hit_behavior : MonoBehaviour {

    public int damage;

    [SerializeField]
    private GameObject shooter;

    private void Start()
    {
        damage = 25;
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "damageable" || (collision.gameObject.GetComponent<ExtraTags>() != null && collision.gameObject.GetComponent<ExtraTags>().hasTag("damageable")))
        {
            collision.transform.GetComponent<health>().doDamage(damage);
            Destroy(transform.gameObject);

            //Debug.Log("HIT" + collision.gameObject);
        }
    }

    public void setShooter(GameObject _shooter)
    {
        shooter = _shooter;
    }

    public GameObject getShooter()
    {
        return shooter;
    }
}
