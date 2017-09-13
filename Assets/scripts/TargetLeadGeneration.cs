using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLeadGeneration : MonoBehaviour {

    // Use this for initialization

    [SerializeField]
    private GameObject target;
    [SerializeField]
    private float distance;

    [SerializeField]
    private float speed;

    [SerializeField]
    private GameObject targetLead;

    [SerializeField]
    private Vector3 _pos;

    public float projectileSpeed;

	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        target = transform.Find("HUD").GetComponent<HUDControl>().getTarget();
        if (target != null)
        {
            speed = target.GetComponent<Rigidbody>().velocity.magnitude;
            distance = Vector3.Distance(target.transform.position, transform.position);
            _pos = targetLead.transform.forward * speed * distance / projectileSpeed;
            targetLead.transform.localPosition = _pos;
        }
	}
}
