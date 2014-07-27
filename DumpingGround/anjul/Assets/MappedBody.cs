using Windows.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class MappedBody
    {
        public MappedBody(Body initialBody, float x_center , float y_center , float z_center )
        {
            if (initialBody == null)
            {
                throw new ArgumentNullException("initalBody", "Cannot be null");
            }
            this.InternalBody = initialBody;

            m_xCenter = x_center;
            m_yCenter = y_center;
            m_zCenter = z_center;
            m_centroid = GetCentroid();
  
        }

        private CameraSpacePoint m_centroid;
        private float m_xCenter, m_yCenter, m_zCenter;
        

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

        public CameraSpacePoint GetCentroid()
        {
            var newX = this.InternalBody.Joints[JointType.SpineBase].Position.X;
            var newY = this.InternalBody.Joints[JointType.SpineBase].Position.Y;
            var newZ = this.InternalBody.Joints[JointType.SpineBase].Position.Z;

            CameraSpacePoint centroid = new CameraSpacePoint()
            {
                X = newX,
                Y = newY,
                Z = newZ
            };

            return centroid;
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

            //var newX = unmappedJoint.Position.X -this.InternalBody.Joints[JointType.SpineBase].Position.X;
            //var newY = unmappedJoint.Position.Y -this.InternalBody.Joints[JointType.SpineBase].Position.Y;
            //var newZ = unmappedJoint.Position.Z;// - this.InternalBody.Joints[JointType.SpineBase].Position.Z;
            var newX = unmappedJoint.Position.X - m_centroid.X;
            var newY = unmappedJoint.Position.Y - m_centroid.Y;
            var newZ = unmappedJoint.Position.Z - m_centroid.Z + m_zCenter;

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

            return mappedJoint;
        }


    }


