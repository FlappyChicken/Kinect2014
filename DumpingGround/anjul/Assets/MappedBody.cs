using Windows.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class MappedBody
    {
        public MappedBody(Body initialBody)
        {
            if (initialBody == null)
            {
                throw new ArgumentNullException("initalBody", "Cannot be null");
            }
            this.InternalBody = initialBody;
        }

        private Body InternalBody { get; set; }


        public static int JointCount { 
            get{
                return Body.JointCount;
            }
        }
        public IDictionary<JointType, Joint> UnmappedJoints {
            get
            {
                return InternalBody.Joints;
            }
        }

        public ulong TrackingId
        {
            get
            {
                return InternalBody.TrackingId;
            }
        }

        public IDictionary<JointType, Joint> Joints
        {
            get
            {
                return InternalBody.Joints.Select(j =>

                    new KeyValuePair<JointType, Joint>(
                        j.Key, 
                        this.MapJoint(j.Value)
                    )
                ).ToDictionary(pair => pair.Key, pair => pair.Value);
            }
        }


        private Joint MapJoint(Joint unmappedJoint)
        {
 

            //mappedJoint.Position.X = this.InternalBody.Joints[JointType.SpineBase].Position.X;

            var newX = unmappedJoint.Position.X -this.InternalBody.Joints[JointType.SpineBase].Position.X;
            var newY = unmappedJoint.Position.Y -this.InternalBody.Joints[JointType.SpineBase].Position.Y;
            var newZ = unmappedJoint.Position.Z - this.InternalBody.Joints[JointType.SpineBase].Position.Z;

            CameraSpacePoint newPosition = new CameraSpacePoint()
            {
                X = newX,
                Y = newY,
                Z = newZ
            };
            

            Joint mappedJoint = new Joint()
            {
                JointType = unmappedJoint.JointType,
                Position = newPosition,
                TrackingState = unmappedJoint.TrackingState
            };

            return unmappedJoint;
        }


    }
