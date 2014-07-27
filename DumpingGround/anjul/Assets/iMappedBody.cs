using Windows.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

   public interface IMappedBody
    {
        IDictionary<JointType, Joint> Joints { get; }
		IDictionary<JointType, Joint> UnmappedJoints{ get; }

	
    }
