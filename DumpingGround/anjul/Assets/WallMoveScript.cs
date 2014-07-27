using UnityEngine;
using System.Collections;
using System.Linq;
using Windows.Kinect;
using System;
using System.Collections.Generic;

public class WallMoveScript : MonoBehaviour {

	public BodyFrameSourceScript BodyScript;
	//public 

	private List<JointType> ImportantJoints = new List<JointType>
	{
			JointType.FootLeft, 
			JointType.FootRight, 
			JointType.HandLeft,
			JointType.HandRight,
			JointType.ShoulderRight,
			JointType.Head
	};
	
	// Use this for initialization
	void Start () {
		//gameObject.transform.position.Set (0.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		float speed = BodyScript.GetAnimationSpeed ();
		gameObject.transform.Translate (new Vector3 (0.0f, 0.0f, -speed));
		float new_z = gameObject.transform.position.z;

		if(new_z < -150.0f)
			gameObject.transform.Translate (new Vector3 (0.0f, 0.0f, 250.0f));

		var playerpos = BodyScript.GetPlayerPosition ();

		if (Mathf.Abs (playerpos.z - gameObject.transform.position.z) < 3.0f) {
			var mymesh = gameObject.GetComponent<MeshFilter>().mesh;

			var playerbody = BodyScript.GetLatestPlayerBody();

			bool collided = false;

			if(playerbody != null)
			{

				for (int i = 0; i < mymesh.triangles.Length; i += 3)
				{
					Vector3 p1 = mymesh.vertices[mymesh.triangles[i + 0]];
					Vector3 p2 = mymesh.vertices[mymesh.triangles[i + 1]];
					Vector3 p3 = mymesh.vertices[mymesh.triangles[i + 2]];

					p1 = gameObject.transform.TransformPoint(p1);
					p2 = gameObject.transform.TransformPoint(p2);
					p3 = gameObject.transform.TransformPoint(p3);

					//Debug.Log (p1);

					Triangle t = new Triangle(p2, p1, p3);

//					if(gameObject.name == "wall4")
//					{
//						foreach(var jt in ImportantJoints)
//						{
//							Vector3 pos = BodyFrameSourceScript.GetVector3FromJoint(playerbody.Joints[jt]);
//							if(t.IsInTriangleDebug(pos))
//							{
//								Debug.Log ("1");
//							}
//						}
//						collided = true;
//						BodyScript.Collided = true;
//						break;
//					}

					if(playerbody.Joints.Any (
						j => t.IsInTriangle(
							BodyFrameSourceScript.GetVector3FromJoint(j.Value))))
				    {
						collided = true;
						BodyScript.Collided = true;
						break;
					}
				}
			}
		}
	}
}
