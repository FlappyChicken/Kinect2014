using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Kinect;



    class FlapDetection
    {
        public static FlapState GetFlapState(IMappedBody body)
        {
            VectorFlappy LeftArmVector = new VectorFlappy();
            LeftArmVector.X = body.Joints[JointType.ShoulderLeft].Position.X - body.Joints[JointType.ElbowLeft].Position.X;
            LeftArmVector.Y = body.Joints[JointType.ShoulderLeft].Position.Y - body.Joints[JointType.ElbowLeft].Position.Y;
            LeftArmVector.Z = body.Joints[JointType.ShoulderLeft].Position.Z - body.Joints[JointType.ElbowLeft].Position.Z;

            VectorFlappy RightArmVector = new VectorFlappy();
            RightArmVector.X = body.Joints[JointType.ShoulderRight].Position.X - body.Joints[JointType.ElbowRight].Position.X;
            RightArmVector.Y = body.Joints[JointType.ShoulderRight].Position.Y - body.Joints[JointType.ElbowRight].Position.Y;
            RightArmVector.Z = body.Joints[JointType.ShoulderRight].Position.Z - body.Joints[JointType.ElbowRight].Position.Z;

            VectorFlappy SpineRefBaseVector = new VectorFlappy();
            SpineRefBaseVector.X = body.Joints[JointType.Neck].Position.X - body.Joints[JointType.SpineBase].Position.X;
            SpineRefBaseVector.Y = body.Joints[JointType.Neck].Position.Y - body.Joints[JointType.SpineBase].Position.Y;
            SpineRefBaseVector.Z = body.Joints[JointType.Neck].Position.Z - body.Joints[JointType.SpineBase].Position.Z;

            double LeftWingAngle, RightWingAngle;

            try
            {
                LeftWingAngle = VectorFlappy.Angle(SpineRefBaseVector, LeftArmVector);
                RightWingAngle = VectorFlappy.Angle(RightArmVector, SpineRefBaseVector);
            }
            catch(ArgumentOutOfRangeException){
                return FlapState.Down;
            }

            //Console.WriteLine(string.Format("({0},{1})", "LeftArm: ", LeftArmVector.ToString()));
            //Console.WriteLine(string.Format("({0},{1})", "RightArm: ", RightArmVector.ToString()));
            //Console.WriteLine(string.Format("({0},{1})", "SpineRefBase: ", SpineRefBaseVector.ToString()));
            
            if (LeftWingAngle > 1.0 && RightWingAngle > 1.0)
            {
                return FlapState.Up;
            }

			if (LeftWingAngle > 0.6 && RightWingAngle > 0.6) 
			{
				return FlapState.Middle;
			}

            if (LeftWingAngle < 0.6 && RightWingAngle < 0.6 )
            {
                return FlapState.Down;
            }

            return FlapState.Unknown;
        }
    }

