using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraTags : MonoBehaviour {

    public string[] extraTags;
	// Use this for initialization
	void Awake () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool hasTag(string _tag)
    {
        bool rv = false;
        for (int i=0; i<extraTags.Length; i++)
        {
            if (_tag == extraTags[i])
            {
                rv = true;
                break;
            }
        }
        return rv;
    }

}
