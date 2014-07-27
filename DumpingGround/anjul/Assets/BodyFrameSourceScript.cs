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
	MappedBody PlayerBody = null;
	CoordinateMapper coordinateMapper;
	public Light DirLight;
	public Light PtLight;
	public SplashScreenHider splash;
	public FlappyBird Flappy = new FlappyBird ();

	GameObject baseGameObject = null;

	public bool Started
	{ get; set; }

//	public bool NobodyHere {
//		get; set;
//	}

	public Vector3 GetPlayerPosition()
	{
		return new Vector3 (0.0f, 0.5f, -90.0f);
	}

	public Vector3 GetPlayerScale()
	{
		return new Vector3 (10.0f, 10.0f, 10.0f);
	}

	public MappedBody GetLatestPlayerBody()
	{
		return PlayerBody;
	}

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
		
		//FrameDescription fd = sensor.DepthFrameSource.FrameDescription;

		Collided = false;

		//InitBones();
		baseGameObject = this.CreateBodyObject(0);
		sensor.Open();

		maxSpeed = 0.5f;
		curSpeed = 0.0f;

		Started = false;

		//NobodyHere = true;

		ResetState ();

	}

	private float maxSpeed;
	private float curSpeed;

	public float GetAnimationSpeed()
	{
		if (Collided)
						return 0.0f;
				else
						return curSpeed;
		}

	float timeWhenReset;

	void ResetState()
	{
		DirLight.intensity = 0.4f;
		RenderSettings.fogColor = new Color(1.0f, 1.0f, 1.0f);
		PtLight.intensity = 2.4f;
		timeWhenReset = Time.realtimeSinceStartup;
	}

	// Update is called once per frame
	void Update () {
		if (Collided) {
			Started = false;
			DirLight.intensity *= 0.95f;
			RenderSettings.fogColor = RenderSettings.fogColor * 0.95f + new Color(0.0f ,0.0f ,0.0f);
			PtLight.intensity *= 0.95f;

//			if(DirLight.intensity < 0.01f)
//			{
//				ResetState();
//			}
		}


		if (!Started && (Time.realtimeSinceStartup - timeWhenReset) > 3.0f) {
			splash.Hide();
			Started = true;
		}


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
								//RefreshBodyObject(ubody, baseGameObject);
								try {
									if(Started) 
									curSpeed = (curSpeed * 0.95f + maxSpeed * 0.05f);
									var bodyscale = GetPlayerScale();
									var bodytrans = GetPlayerPosition();
									PlayerBody = new MappedBody (ubody, 
						                       bodytrans.x, bodytrans.y, bodytrans.z,
						                       bodyscale.x, bodyscale.y, bodyscale.z, Flappy.Height);
									Flappy.evolve(PlayerBody);

								} catch {
									return;
								}
								RefreshBodyObject(PlayerBody, baseGameObject);
					//NobodyHere = false;
							} 
				else
				{
					//NobodyHere = true;
					curSpeed = curSpeed * 0.9f;
					//HideBodyObject(PlayerBody, baseGameObject);
				}
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

		body.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

		for (JointType jt = JointType.SpineBase; jt <= JointType.ThumbRight; jt++)
		{
			GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			
//			LineRenderer lr = jointObj.AddComponent<LineRenderer>();
//			lr.SetVertexCount(2);
//			lr.material = BoneMaterial;
//			lr.SetWidth(1.0f, 1.0f);
//			//lr.transform.parent = body.transform;
			
			jointObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			jointObj.name = jt.ToString();
			jointObj.transform.parent = body.transform;
		}
		
		return body;
	}

	private void RefreshBodyObject(MappedBody body, GameObject bodyObject)
	{
		//Debug.Log( body.Joints.Select ((joint) => joint.Value.Position.Y).Min ().ToString());
		
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
			
			//			LineRenderer lr = jointObj.GetComponent<LineRenderer>();
			//			if(targetJoint.HasValue)
			//			{
			//				lr.SetPosition(0, jointObj.localPosition);
			//				lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
			//				//lr.transform.parent = bodyObject.transform;
			//				lr.SetColors(new Color(1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f));
			//			}
			//			else
			//			{
			//				lr.enabled = false;
			//			}
		}
	}


	public bool Collided {
				get;
				set;
		}



	public static Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
	{
		return new Vector3(joint.Position.X , joint.Position.Y , joint.Position.Z );
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
