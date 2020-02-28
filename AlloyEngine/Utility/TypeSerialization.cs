using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using OpenTK;

namespace Alloy.Utility
{
    class TypeSerialization
    {
        //Primitive Types
        public static void SerializePrimitiveType(object value, XmlNode parentNode, XmlDocument xml)
        {
            XmlAttribute valueAtt = xml.CreateAttribute("value");
            valueAtt.Value = value.ToString();
            parentNode.Attributes.Append(valueAtt);
        }
        public static void SerializePrimitiveTypeField(FieldInfo field, object valueSource, XmlNode parentNode, XmlDocument xml)
        {
            SerializePrimitiveType(field.GetValue(valueSource), parentNode, xml);
        }

        //Vector2
        public static void SerializeVector2(Vector2 vec, XmlNode parentNode, XmlDocument xml)
        {
            XmlAttribute xAtt = xml.CreateAttribute("x");
            xAtt.Value = vec.X.ToString();
            parentNode.Attributes.Append(xAtt);
            XmlAttribute yAtt = xml.CreateAttribute("y");
            yAtt.Value = vec.Y.ToString();
            parentNode.Attributes.Append(yAtt);
        }
        public static void SerializeVector2Field(FieldInfo field, object valueSource, XmlNode parentNode, XmlDocument xml)
        {
            Vector2? vec = field.GetValue(valueSource) as Vector2?;
            SerializeVector2(vec.Value, parentNode, xml);
        }

        //Vector3
        public static void SerializeVector3(Vector3 vec, XmlNode parentNode, XmlDocument xml)
        {
            XmlAttribute xAtt = xml.CreateAttribute("x");
            xAtt.Value = vec.X.ToString();
            parentNode.Attributes.Append(xAtt);
            XmlAttribute yAtt = xml.CreateAttribute("y");
            yAtt.Value = vec.Y.ToString();
            parentNode.Attributes.Append(yAtt);
            XmlAttribute zAtt = xml.CreateAttribute("z");
            zAtt.Value = vec.Z.ToString();
            parentNode.Attributes.Append(zAtt);
        }
        public static void SerializeVector3Field(FieldInfo field, object valueSource, XmlNode parentNode, XmlDocument xml)
        {
            Vector3? vec = field.GetValue(valueSource) as Vector3?;
            SerializeVector3(vec.Value, parentNode, xml);
        }

        //Vector4
        public static void SerializeVector4(Vector4 vec, XmlNode parentNode, XmlDocument xml)
        {
            XmlAttribute xAtt = xml.CreateAttribute("x");
            xAtt.Value = vec.X.ToString();
            parentNode.Attributes.Append(xAtt);
            XmlAttribute yAtt = xml.CreateAttribute("y");
            yAtt.Value = vec.Y.ToString();
            parentNode.Attributes.Append(yAtt);
            XmlAttribute zAtt = xml.CreateAttribute("z");
            zAtt.Value = vec.Z.ToString();
            parentNode.Attributes.Append(zAtt);
            XmlAttribute wAtt = xml.CreateAttribute("w");
            wAtt.Value = vec.W.ToString();
            parentNode.Attributes.Append(wAtt);
        }
        public static void SerializeVector4Field(FieldInfo field, object valueSource, XmlNode parentNode, XmlDocument xml)
        {
            Vector4? vec = field.GetValue(valueSource) as Vector4?;
            SerializeVector4(vec.Value, parentNode, xml);
        }

        //Quatnernion
        public static void SerializeQuaternion(Quaternion quat, XmlNode parentNode, XmlDocument xml)
        {
            XmlAttribute xAtt = xml.CreateAttribute("x");
            xAtt.Value = quat.X.ToString();
            parentNode.Attributes.Append(xAtt);
            XmlAttribute yAtt = xml.CreateAttribute("y");
            yAtt.Value = quat.Y.ToString();
            parentNode.Attributes.Append(yAtt);
            XmlAttribute zAtt = xml.CreateAttribute("z");
            zAtt.Value = quat.Z.ToString();
            parentNode.Attributes.Append(zAtt);
            XmlAttribute wAtt = xml.CreateAttribute("w");
            wAtt.Value = quat.W.ToString();
            parentNode.Attributes.Append(wAtt);
        }
        public static void SerializeQuaternionField(FieldInfo field, object valueSource, XmlNode parentNode, XmlDocument xml)
        {
            Quaternion? quat = field.GetValue(valueSource) as Quaternion?;
            SerializeQuaternion(quat.Value, parentNode, xml);
        }

        //General Field
        public static void SerializeType(object value, XmlNode parentNode, XmlDocument xml)
        {
            Type t = value.GetType();
            if (t.IsPrimitive)
                SerializePrimitiveType(value, parentNode, xml);
            else
            {
                if (t == typeof(Vector2))
                    SerializeVector2((value as Vector2?).Value, parentNode, xml);
                else if (t == typeof(Vector3))
                    SerializeVector3((value as Vector3?).Value, parentNode, xml);
                else if (t == typeof(Vector4))
                    SerializeVector4((value as Vector4?).Value, parentNode, xml);
                else if (t == typeof(Quaternion))
                    SerializeQuaternion((value as Quaternion?).Value, parentNode, xml);
            }
        }
        public static void SerializeField(FieldInfo field, object valueSource, XmlNode parentNode, XmlDocument xml)
        {
            SerializeType(field.GetValue(valueSource), parentNode, xml);
        }
    }
}
