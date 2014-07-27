using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;


public class JointPair {
	public JointType Item1;
	public JointType Item2;

	public JointPair(JointType item1, JointType item2)
	{
		Item1 = item1;
		Item2 = item2;
	}
}

public class BodyFrameSourceScript : MonoBehaviour {

	KinectSensor sensor;
	BodyFrameReader bdReader;
	IList<Body> bodies = null;
	public Material BoneMaterial;

	CoordinateMapper coordinateMapper;
	IDictionary<Bone, JointPair> bones;

	GameObject baseGameObject = null;

	private Dictionary<JointType, JointType> _BoneMap = new Dictionary<JointType, JointType>()
	{
		{ JointType.FootLeft, JointType.AnkleLeft },
		{ JointType.AnkleLeft, JointType.KneeLeft },
		{ JointType.KneeLeft, JointType.HipLeft },
		{ JointType.HipLeft, JointType.SpineBase },
		
		{ JointType.FootRight, JointType.AnkleRight },
		{ JointType.AnkleRight, JointType.KneeRight },
		{ JointType.KneeRight, JointType.HipRight },
		{ JointType.HipRight, JointType.SpineBase },
		
		{ JointType.HandTipLeft, JointType.HandLeft },
		{ JointType.ThumbLeft, JointType.HandLeft },
		{ JointType.HandLeft, JointType.WristLeft },
		{ JointType.WristLeft, JointType.ElbowLeft },
		{ JointType.ElbowLeft, JointType.ShoulderLeft },
		{ JointType.ShoulderLeft, JointType.SpineShoulder },
		
		{ JointType.HandTipRight, JointType.HandRight },
		{ JointType.ThumbRight, JointType.HandRight },
		{ JointType.HandRight, JointType.WristRight },
		{ JointType.WristRight, JointType.ElbowRight },
		{ JointType.ElbowRight, JointType.ShoulderRight },
		{ JointType.ShoulderRight, JointType.SpineShoulder },
		
		{ JointType.SpineBase, JointType.SpineMid },
		{ JointType.SpineMid, JointType.SpineShoulder },
		{ JointType.SpineShoulder, JointType.Neck },
		{ JointType.Neck, JointType.Head },
	};

	// Use this for initialization
	void Start () {
		sensor = KinectSensor.GetDefault();
		
		bdReader = sensor.BodyFrameSource.OpenReader();
		coordinateMapper = sensor.CoordinateMapper;
		
		FrameDescription fd = sensor.DepthFrameSource.FrameDescription;

		//InitBones();
		baseGameObject = this.CreateBodyObject(0);
		sensor.Open();
	}

	// Update is called once per frame
	void Update () {
        if (bdReader != null)
        {
            var bdFrame = bdReader.AcquireLatestFrame();
            if (bdFrame != null)
            {
							if (bodies == null)
								bodies = new Body[bdFrame.BodyCount];

							bdFrame.GetAndRefreshBodyData (bodies);

							Body ubody = FindFirstBody (bodies, this.coordinateMapper);
							if (ubody!=null){
								RefreshBodyObject(ubody, baseGameObject);
							}

						// MappedBody body;
						// try {
						// 		body = new MappedBody (ubody);
						// } catch {
						// 		return;
						// }
						bdFrame.Dispose();
						bdFrame = null;
					}
        }
	}

	public Body FindFirstBody(IEnumerable<Body> bodies, CoordinateMapper mapper)
	{
		return bodies.FirstOrDefault(
			b => b.Joints.Any(
			j => (j.Value.Position.Z > 0)));
	}

	public GameObject CreateBodyObject(ulong id)
	{
		GameObject body = new GameObject("Body:" + id);

		body.transform.Translate(new Vector3(0, 10, -100));
		
		for (JointType jt = JointType.SpineBase; jt <= JointType.ThumbRight; jt++)
		{
			GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			
			LineRenderer lr = jointObj.AddComponent<LineRenderer>();
			lr.SetVertexCount(2);
			lr.material = BoneMaterial;
			lr.SetWidth(1.0f, 1.0f);
			
			jointObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			jointObj.name = jt.ToString();
			jointObj.transform.parent = body.transform;
		}
		
		return body;
	}

	private void RefreshBodyObject(Body body, GameObject bodyObject)
	{
		for (JointType jt = JointType.SpineBase; jt <= JointType.ThumbRight; jt++)
		{
			Windows.Kinect.Joint sourceJoint = body.Joints[jt];
			Windows.Kinect.Joint? targetJoint = null;
			
			if(_BoneMap.ContainsKey(jt))
			{
				targetJoint = body.Joints[_BoneMap[jt]];
			}
			
			Transform jointObj = bodyObject.transform.FindChild(jt.ToString());
			jointObj.localPosition = GetVector3FromJoint(sourceJoint);
			
			LineRenderer lr = jointObj.GetComponent<LineRenderer>();
			if(targetJoint.HasValue)
			{
				lr.SetPosition(0, jointObj.localPosition);
				lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
				lr.SetColors(new Color(1.0f, 1.0f, 1.0f), new Color(10.0f, 10.0f, 10.0f));
			}
			else
			{
				lr.enabled = false;
			}
		}
	}

	private static Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
	{
		return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
	}

	// public void InitBones()
	// {
	// 	// a bone defined as a line between two joints
	// 	var boneslist = new List<JointPair>();
		
	// 	boneslist = new List<JointPair>();
		
	// 	// Torso
	// 	boneslist.Add(new JointPair(JointType.Head, JointType.Neck));
	// 	boneslist.Add(new JointPair(JointType.Neck, JointType.SpineShoulder));
	// 	boneslist.Add(new JointPair(JointType.SpineShoulder, JointType.SpineMid));
	// 	boneslist.Add(new JointPair(JointType.SpineMid, JointType.SpineBase));
	// 	boneslist.Add(new JointPair(JointType.SpineShoulder, JointType.ShoulderRight));
	// 	boneslist.Add(new JointPair(JointType.SpineShoulder, JointType.ShoulderLeft));
	// 	boneslist.Add(new JointPair(JointType.SpineBase, JointType.HipRight));
	// 	boneslist.Add(new JointPair(JointType.SpineBase, JointType.HipLeft));
		
	// 	// Right Arm
	// 	boneslist.Add(new JointPair(JointType.ShoulderRight, JointType.ElbowRight));
	// 	boneslist.Add(new JointPair(JointType.ElbowRight, JointType.WristRight));
	// 	boneslist.Add(new JointPair(JointType.WristRight, JointType.HandRight));
	// 	boneslist.Add(new JointPair(JointType.HandRight, JointType.HandTipRight));
	// 	boneslist.Add(new JointPair(JointType.WristRight, JointType.ThumbRight));
		
	// 	// Left Arm
	// 	boneslist.Add(new JointPair(JointType.ShoulderLeft, JointType.ElbowLeft));
	// 	boneslist.Add(new JointPair(JointType.ElbowLeft, JointType.WristLeft));
	// 	boneslist.Add(new JointPair(JointType.WristLeft, JointType.HandLeft));
	// 	boneslist.Add(new JointPair(JointType.HandLeft, JointType.HandTipLeft));
	// 	boneslist.Add(new JointPair(JointType.WristLeft, JointType.ThumbLeft));
		
	// 	// Right Leg
	// 	boneslist.Add(new JointPair(JointType.HipRight, JointType.KneeRight));
	// 	boneslist.Add(new JointPair(JointType.KneeRight, JointType.AnkleRight));
	// 	boneslist.Add(new JointPair(JointType.AnkleRight, JointType.FootRight));
		
	// 	// Left Leg
	// 	boneslist.Add(new JointPair(JointType.HipLeft, JointType.KneeLeft));
	// 	boneslist.Add(new JointPair(JointType.KneeLeft, JointType.AnkleLeft));
	// 	boneslist.Add(new JointPair(JointType.AnkleLeft, JointType.FootLeft));
	// 	int i = 0;
	// 	this.bones = boneslist.ToDictionary(
	// 		s => (Bone)i++
	// 		);
	// }

    void OnApplicationQuit()
	{
		
		if (bdReader != null)
		{
			bdReader.Dispose();
			bdReader = null;
		}
        
        if (sensor != null)
        {
            if (sensor.IsOpen)
            {
                sensor.Close();
            }
            
            sensor = null;
        }
    }
}
