using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public enum Bone
    {
                        // Torso
        Head = 0,       //boneslist    Head,            Neck  
        Neck,           //boneslist    Neck,            SpineShoulder  
        UpperSpine,     //boneslist    SpineShoulder,   SpineMid  
        LowerSpine,		//boneslist    SpineMid,        SpineBase  
        RightShoulder,  //boneslist    SpineShoulder,   ShoulderRight  
        LeftShoulder,   //boneslist    SpineShoulder,   ShoulderLeft  
        RightHip,       //boneslist    SpineBase,       HipRight  
        LeftHip,        //boneslist    SpineBase,       HipLeft  

        				// Right Arm
        RightUpperArm,	//boneslist    ShoulderRight,   ElbowRight  
        RightLowerArm,	//boneslist    ElbowRight,      WristRight  
        RightWrist,		//boneslist    WristRight,      HandRight  
        RightFingers,   //boneslist    HandRight,       HandTipRight  
        RightThumb,		//boneslist    WristRight,      ThumbRight  

        				// Left Arm
        LeftUpperArm,	//boneslist    ShoulderLeft,    ElbowLeft  
        LeftLowerArm,   //boneslist    ElbowLeft,       WristLeft  
        LeftWrist,		//boneslist    WristLeft,       HandLeft  
        LeftFingers,	//boneslist    HandLeft,        HandTipLeft  
        LeftThumb,		//boneslist    WristLeft,       ThumbLeft  

        				// Right Leg
        RightUpperLeg,	//boneslist    HipRight,        KneeRight  
        RightLowerLeg,	//boneslist    KneeRight,       AnkleRight  
        RightFoot,		//boneslist    AnkleRight,      FootRight  

        		        // Left Leg
        LeftUpperLeg,   //boneslist    HipLeft,         KneeLeft  
        LeftLowerLeg,   //boneslist    KneeLeft,        AnkleLeft  
        LeftFoot,       //boneslist    AnkleLeft,       FootLeft  


    }