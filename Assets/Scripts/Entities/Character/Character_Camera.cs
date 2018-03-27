using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Camera : MonoBehaviour {

    public Camera cam;
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos.z = -10;
        cam.transform.position = pos;
	}
}
