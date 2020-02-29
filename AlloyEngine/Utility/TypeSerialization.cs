using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using OpenTK;
using OpenTK.Graphics;

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
        public static T DeserializePrimitiveType<T>(XmlNode node)
        {
            return (T)Convert.ChangeType(node.Attributes["value"].Value, typeof(T));
        }
        public static object DeserializePrimitiveType(XmlNode node, Type type)
        {
            return Convert.ChangeType(node.Attributes["value"].Value, type);
        }
        public static void DeserializePrimitiveTypeField<T>(XmlNode node, FieldInfo field, object target)
        {
            field.SetValue(target, DeserializePrimitiveType<T>(node));
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
        public static Vector2 DeserializeVector2(XmlNode node)
        {
            return new Vector2(Convert.ToSingle(node.Attributes["x"].Value), Convert.ToSingle(node.Attributes["y"].Value));
        }
        public static void DeserializeVector2Field(XmlNode node, FieldInfo field, object target)
        {
            field.SetValue(target, DeserializeVector2(node));
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
        public static Vector3 DeserializeVector3(XmlNode node)
        {
            return new Vector3(Convert.ToSingle(node.Attributes["x"].Value), Convert.ToSingle(node.Attributes["y"].Value), Convert.ToSingle(node.Attributes["z"].Value));
        }
        public static void DeserializeVector3Field(XmlNode node, FieldInfo field, object target)
        {
            field.SetValue(target, DeserializeVector3(node));
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
        public static Vector4 DeserializeVector4(XmlNode node)
        {
            return new Vector4(Convert.ToSingle(node.Attributes["x"].Value), Convert.ToSingle(node.Attributes["y"].Value), Convert.ToSingle(node.Attributes["z"].Value), Convert.ToSingle(node.Attributes["w"].Value));
        }
        public static void DeserializeVector4Field(XmlNode node, FieldInfo field, object target)
        {
            field.SetValue(target, DeserializeVector4(node));
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
        public static Quaternion DeserializeQuaternion(XmlNode node)
        {
            return new Quaternion(Convert.ToSingle(node.Attributes["x"].Value), Convert.ToSingle(node.Attributes["y"].Value), Convert.ToSingle(node.Attributes["z"].Value), Convert.ToSingle(node.Attributes["w"].Value));
        }
        public static void DeserializeQuaternionField(XmlNode node, FieldInfo field, object target)
        {
            field.SetValue(target, DeserializeQuaternion(node));
        }

        //Color
        public static void SerializeColor(Color4 col, XmlNode parentNode, XmlDocument xml)
        {
            XmlAttribute rAtt = xml.CreateAttribute("r");
            rAtt.Value = col.R.ToString();
            parentNode.Attributes.Append(rAtt);
            XmlAttribute gAtt = xml.CreateAttribute("g");
            gAtt.Value = col.G.ToString();
            parentNode.Attributes.Append(gAtt);
            XmlAttribute bAtt = xml.CreateAttribute("b");
            bAtt.Value = col.B.ToString();
            parentNode.Attributes.Append(bAtt);
            XmlAttribute aAtt = xml.CreateAttribute("a");
            aAtt.Value = col.A.ToString();
            parentNode.Attributes.Append(aAtt);
        }
        public static void SerializeColorField(FieldInfo field, object valueSource, XmlNode parentNode, XmlDocument xml)
        {
            Color4? col = field.GetValue(valueSource) as Color4?;
            SerializeColor(col.Value, parentNode, xml);
        }
        public static Color4 DeserializeColor(XmlNode node)
        {
            return new Color4(Convert.ToSingle(node.Attributes["r"].Value), Convert.ToSingle(node.Attributes["g"].Value), Convert.ToSingle(node.Attributes["b"].Value), Convert.ToSingle(node.Attributes["a"].Value));
        }
        public static void DeserializeColorField(XmlNode node, FieldInfo field, object target)
        {
            field.SetValue(target, DeserializeColor(node));
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
                else if (t == typeof(Color4))
                    SerializeColor((value as Color4?).Value, parentNode, xml);
            }
        }
        public static void SerializeField(FieldInfo field, object valueSource, XmlNode parentNode, XmlDocument xml)
        {
            SerializeType(field.GetValue(valueSource), parentNode, xml);
        }
        public static object DeserializeType(XmlNode node, Type type)
        {
            if (type.IsPrimitive)
                return DeserializePrimitiveType(node, type);
            else
            {
                if (type == typeof(Vector2))
                    return DeserializeVector2(node);
                else if (type == typeof(Vector3))
                    return DeserializeVector3(node);
                else if (type == typeof(Vector4))
                    return DeserializeVector4(node);
                else if (type == typeof(Quaternion))
                    return DeserializeQuaternion(node);
                else if (type == typeof(Color4))
                    return DeserializeColor(node);
                else
                    return null;
            }
        }
        public static void DeserializeField(XmlNode node, FieldInfo field, object target)
        {
            object value = DeserializeType(node, field.FieldType);
            if(value != null)
                field.SetValue(target, value);
        }
    }
}
