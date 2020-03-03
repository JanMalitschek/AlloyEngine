using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Alloy
{
    public class Transform
    {
        public Vector3 position = Vector3.Zero;
        public Quaternion rotation = Quaternion.Identity;
        public Vector3 scale = Vector3.One;

        public Transform parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        public Vector3 Right
        {
            get
            {
                return rotation * Vector3.UnitX;
            }
        }
        public Vector3 Up
        {
            get
            {
                return rotation * Vector3.UnitY;
            }
        }
        public Vector3 Forward
        {
            get
            {
                return rotation * Vector3.UnitZ;
            }
        }

        public void Translate(Vector3 offset)
        {
            position += offset;
        }
        public void Translate(float xOffset, float yOffset, float zOffset)
        {
            position += new Vector3(xOffset, yOffset, zOffset);
        }
        
        public void Rotate(Quaternion rotation)
        {
            this.rotation = rotation * this.rotation;
        }
        public void Rotate(float xRot, float yRot, float zRot)
        {
            this.rotation = new Quaternion(new Vector3(xRot, yRot, zRot)) * this.rotation;
        }
        public void Rotate(Vector3 eulerAngles)
        {
            this.rotation = new Quaternion(eulerAngles) * this.rotation;
        }

        public void Scale(Vector3 scale)
        {
            this.scale *= scale;
        }
        public void Scale(float xScale, float yScale, float zScale)
        {
            this.scale *= new Vector3(xScale, yScale, zScale);
        }

        public Matrix4 GetTransformationMatrix()
        {
            var T = Matrix4.CreateTranslation(position);
            var eulerAngles = rotation.ToAxisAngle();
            var Rx = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(eulerAngles.X));
            var Ry = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(eulerAngles.Y));
            var Rz = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(eulerAngles.Z));
            var R = Rz * Ry * Rx;
            var S = Matrix4.CreateScale(scale);
            return T * R * S;
        }
    }
}