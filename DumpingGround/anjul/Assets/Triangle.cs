using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    public class Triangle
    {
        IEnumerable<Vector3> vertices;
        public Triangle(Vector3 first, Vector3 second, Vector3 third)
        {
            var pointList = new List<Vector3>();

            pointList.Add(first);
            pointList.Add(second);
            pointList.Add(third);
            vertices = pointList.AsEnumerable();
        }

        public bool IsInTriangle(Vector3 point)
        {
            bool inTriangle=false;
            Vector3[] vert = vertices.ToArray();
            
            //iterate over all indices
            
            //this condition must be true for all edges for the point to be inside
            bool compareEdge0 = checkConditionTriangle(point,vert[0],vert[1]);
            bool compareEdge1 = checkConditionTriangle(point,vert[1],vert[2]);
            bool compareEdge2 = checkConditionTriangle(point, vert[2], vert[0]);
            

            if (compareEdge0 && compareEdge1 && compareEdge2)
                    inTriangle = true;
            else
                    inTriangle= false; //if condition not met then break
                             
            return inTriangle;

        }
        private bool checkConditionTriangle(Vector3 point, Vector3 vert0, Vector3 vert1)
        {
            //assume right handedness => +1 else -1
            int RtHanded = IsRightHanded();

            return (RtHanded * ((point.y - vert0.y) * (vert1.x - vert0.x) - (point.x - vert0.x) * (vert1.y - vert0.y)) >= 0.0);
        }   

        private int IsRightHanded()
        {
            int isRightHanded = +1; //return +1 if rightHanded or -1 if leftHanded
            return isRightHanded;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
       
    }
