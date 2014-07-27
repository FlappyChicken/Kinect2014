﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class WallMoveScript : MonoBehaviour {

	public BodyFrameSourceScript BodyScript;
	//public 

	// Use this for initialization
	void Start () {
		//gameObject.transform.position.Set (0.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Translate (new Vector3 (0.0f, 0.0f, -1.0f));
		float new_z = gameObject.transform.position.z;

		if(new_z < -150.0f)
			gameObject.transform.Translate (new Vector3 (0.0f, 0.0f, 250.0f));

		var playerpos = BodyScript.GetPlayerPosition ();

		if (Mathf.Abs (playerpos.z - gameObject.transform.position.z) < 5.0f) {
			var mymesh = gameObject.GetComponent<MeshFilter>().mesh;
			Debug.Log(gameObject.name +  "...triangles: " + mymesh.triangles.Length);
		}
	}
}
