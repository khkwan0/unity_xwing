using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarField : MonoBehaviour {

    private ParticleSystem.Particle[] points;
    private float starDistanceSqr;
    private float starClipDistanceSqr;

    public Color starColor;
    public int starsMax = 600;
    public float starSize = 0.35f;
    public float starDistance = 60.0f;
    public float starClipDistance = 15f;

    [SerializeField]
    private float _starDistance;
    private Vector3 rootPos;
    private Vector3 oneStarPos;

    void Start() {
        //thisTransform = GetComponent<Transform>();
        starDistanceSqr = starDistance * starDistance;
        starClipDistanceSqr = starClipDistance * starClipDistance;
    }

    private void createStars()
    {
        points = new ParticleSystem.Particle[starsMax];
        for (int i=0; i<starsMax; i++)
        {
            points[i].position = Random.insideUnitSphere * starDistance + transform.position;
            points[i].startColor = new Color(starColor.r, starColor.g, starColor.b, starColor.a);
            points[i].startSize = starSize;
        }        
    }

    void Update()
    {
         if (points == null)
        {
            createStars();
        }
        for (int i=0; i<starsMax; i++)
        {
            if ((points[i].position - transform.position).sqrMagnitude > starDistanceSqr)
            {                    
                points[i].position = Random.insideUnitSphere.normalized * starDistance + transform.position;
            }
            if ((points[i].position - transform.position).sqrMagnitude <= starClipDistanceSqr)
            {
                float percentage = (points[i].position - transform.position).sqrMagnitude / starClipDistanceSqr;
                points[i].startColor = new Color(1, 1, 1, percentage);
                points[i].startSize = percentage * starSize;
            }
        }
        GetComponent<ParticleSystem>().SetParticles(points, points.Length);
    }  
}
