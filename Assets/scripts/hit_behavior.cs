using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hit_behavior : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "damageable" || (collision.gameObject.GetComponent<ExtraTags>() != null && collision.gameObject.GetComponent<ExtraTags>().hasTag("damageable")))
        {
            collision.transform.GetComponent<health>().doDamage(25);
            Destroy(transform.gameObject);

            Debug.Log("HIT" + collision.gameObject);
        }
    }
}
