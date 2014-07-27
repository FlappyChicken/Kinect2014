using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




    public class VectorFlappy
    {
        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }

        public float Z
        {
            get;
            set;
        }

        public VectorFlappy()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }

        public VectorFlappy(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public VectorFlappy(FloatPoint point1, FloatPoint point2)
        {
            this.X = point1.X - point2.X;
            this.Y = point1.Y - point2.Y;
            this.Z = point1.Z - point2.Z;
        }
        private const string NEGATIVE_MAGNITUDE = "The magnitude of a Vector must be a positive value, (i.e. greater than 0)";

        private const string ZERO_MAGNITUDE = "Magnitude is Zero";

        //private VectorFlappy origin = new VectorFlappy((float)0, (float)0, (float)0);

        public static double Magnitude(VectorFlappy v)
        {
            double mag_value = Math.Sqrt(v.X * v.X+ v.Y * v.Y+ v.Z * v.Z);
            if (mag_value < 0)
            {
                    throw new ArgumentOutOfRangeException("value", mag_value, NEGATIVE_MAGNITUDE);
            }
            return mag_value;
        }

        public static double DotProduct(VectorFlappy v1, VectorFlappy v2)
        {
            double DotProd_value = v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
            if (Magnitude(v1) != 0 && Magnitude(v2) != 0)
                DotProd_value = DotProd_value / (Magnitude(v1) * Magnitude(v2));
            else
                throw new ArgumentOutOfRangeException("value", 0, NEGATIVE_MAGNITUDE);

            return DotProd_value;
        }

        public static double Angle(VectorFlappy v1, VectorFlappy v2)
        {

            return Math.Acos(DotProduct(v1, v2));
            
        }

        public override string ToString()
        {
            return string.Format("({0},{1}, {2})", X.ToString(), Y.ToString(), Z.ToString());
        }


    }




