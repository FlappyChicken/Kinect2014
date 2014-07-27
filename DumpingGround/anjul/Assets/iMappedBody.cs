using Windows.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

interface IMappedBody
{
	IDictionary<JointType, Joint> MappedJoints { get; }
	int JointCount { get; }
}
