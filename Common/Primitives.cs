using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsPractice.Common
{
    /// <summary>
    /// Represents 3D point
    /// </summary>
    public class Point3D
    {
        public float X,Y,Z;
        public Point3D(float x,float y,float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Point3D rotate(float pitch, float roll, float yaw)
        {
            float cosa = MathF.Cos(yaw);
            float sina = MathF.Sin(yaw);

            float cosb = MathF.Cos(pitch);
            float sinb = MathF.Sin(pitch);

            float cosc = MathF.Cos(roll);
            float sinc = MathF.Sin(roll);

            float Axx = cosa * cosb;
            float Axy = cosa * sinb * sinc - sina * cosc;
            float Axz = cosa * sinb * cosc + sina * sinc;

            float Ayx = sina * cosb;
            float Ayy = sina * sinb * sinc + cosa * cosc;
            float Ayz = sina * sinb * cosc - cosa * sinc;

            float Azx = -sinb;
            float Azy = cosb * sinc;
            float Azz = cosb * cosc;


            float px = this.X;
            float py = this.Y;
            float pz = this.Z;

            X = Axx * px + Axy * py + Axz * pz;
            Y = Ayx * px + Ayy * py + Ayz * pz;
            Z = Azx * px + Azy * py + Azz * pz;
            
            return this;
        }
    }

    public class Line2D
    {
        public PointF start;
        public PointF end;
        public Line2D(PointF start, PointF end)
        {
            this.start = start;
            this.end = end;
        }

        public PointF Center()
        {
            return new PointF(
                (this.start.X + this.end.X) / 2,
                (this.start.Y + this.end.Y) / 2
         );
        }

        public Line2D Rotate(PointF ass)
        {
            return this;
        }
    }

    public class Line3D
    {
        public Point3D start;
        public Point3D end;
        public Line3D(Point3D start, Point3D end)
        {
            this.start = start;
            this.end = end;
        }

        public Point3D Center()
        {
            return new Point3D(
                (this.start.X + this.end.X) / 2,
                (this.start.Y + this.end.Y) / 2,
                (this.start.Z + this.end.Z) / 2
            );
        }

        public Line3D RotateAround(Point3D point, float angle)
        {
            return this;
        }
    }

    /// <summary>
    /// Represents a cube in 3d space (TODO)
    /// </summary>
    public class Cube3D
    {
        //public Point3D start;
        //public Point3D end;
        
        //public Cube3D(Point3D start, Point3D end)
        //{
        //    this.start = start;
        //    this.end = end;
        //}

        //public Point3D Center()
        //{
        //    return new Point3D(
        //        (this.start.X + this.end.X) / 2,
        //        (this.start.Y + this.end.Y) / 2,
        //        (this.start.Z + this.end.Z) / 2
        //    );
        //}

        //public Cube3D RotateAround(Point3D point, float angle)
        //{
        //    return this;
        //}
    }

}
