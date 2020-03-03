using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using OpenTK;
using OpenTK.Graphics;
using Alloy.Assets;

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

        public static void SerializeAsset(Asset asset, XmlNode parentNode, XmlDocument xml)
        {
            XmlAttribute idAtt = xml.CreateAttribute("id");
            idAtt.Value = asset.ID.ToString();
            parentNode.Attributes.Append(idAtt);
        }
        public static void SerializeAssetArray(Asset[] assets, XmlNode parentNode, XmlDocument xml)
        {
            foreach(Asset a in assets)
            {
                XmlNode assetNode = xml.CreateElement("Asset");
                assetNode.InnerText = a.ID.ToString();
                parentNode.AppendChild(assetNode);
            }
        }
        public static void SerializeAssetField(FieldInfo field, object valueSource, XmlNode parentNode, XmlDocument xml)
        {
            Asset asset = field.GetValue(valueSource) as Asset;
            SerializeAsset(asset, parentNode, xml);
        }
        public static void SerializeAssetArrayField(FieldInfo field, object valueSource, XmlNode parentNode, XmlDocument xml)
        {
            SerializeAssetArray(field.GetValue(valueSource) as Asset[], parentNode, xml);
        }
        public static Asset DeserializeAsset(XmlNode node)
        {
            int id = Convert.ToInt32(node.Attributes["id"].Value);
            AssetDatabase.Load(id);
            return AssetDatabase.GetAsset(id);
        }
        public static void DeserializeAssetField(XmlNode node, FieldInfo field, object target)
        {
            field.SetValue(target, DeserializeAsset(node));
        }
        public static T[] DeserializeAssetArray<T>(XmlNode node) where T : Asset
        {
            List<T> assets = new List<T>();
            foreach(XmlNode c in node.ChildNodes)
            {
                int id = Convert.ToInt32(c.InnerText);
                AssetDatabase.Load(id);
                assets.Add(AssetDatabase.GetAsset<T>(id));
            }
            return assets.ToArray();
        }
        public static void DeserializeAssetArrayField<T>(XmlNode node, FieldInfo field, object target) where T : Asset
        {
            field.SetValue(target, DeserializeAssetArray<T>(node));
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
                else if (t.IsSubclassOf(typeof(Asset)))
                    SerializeAsset(value as Asset, parentNode, xml);
                else if (t == typeof(Asset[]))
                    SerializeAssetArray(value as Asset[], parentNode, xml);
                else if (t == typeof(Model[]))
                    SerializeAssetArray(value as Model[], parentNode, xml);
                else if (t == typeof(Shader[]))
                    SerializeAssetArray(value as Shader[], parentNode, xml);
                else if (t == typeof(Scene[]))
                    SerializeAssetArray(value as Scene[], parentNode, xml);
                else if (t == typeof(Texture[]))
                    SerializeAssetArray(value as Texture[], parentNode, xml);
                else if (t == typeof(Material[]))
                    SerializeAssetArray(value as Material[], parentNode, xml);
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
                else if (type.IsSubclassOf(typeof(Asset)))
                    return DeserializeAsset(node);
                else if (type == typeof(Asset[]))
                    return DeserializeAssetArray<Asset>(node);
                else if (type == typeof(Model[]))
                    return DeserializeAssetArray<Model>(node);
                else if (type == typeof(Shader[]))
                    return DeserializeAssetArray<Shader>(node);
                else if (type == typeof(Scene[]))
                    return DeserializeAssetArray<Scene>(node);
                else if (type == typeof(Texture[]))
                    return DeserializeAssetArray<Texture>(node);
                else if (type == typeof(Material[]))
                    return DeserializeAssetArray<Material>(node);
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
