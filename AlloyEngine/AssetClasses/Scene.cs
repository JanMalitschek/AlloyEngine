using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Alloy.Assets;
using Alloy.Components;
using Alloy.Utility;
using System.Xml;
using System.Reflection;

namespace Alloy.Assets
{
    public class Scene : Asset
    {
        public Tree<Entity> hierarchy { get; private set; }
        public Scene(string path) : base(path)
        {
            hierarchy = new Tree<Entity>();
        }

        public void SaveScene(string path)
        {
            XmlDocument xml = new XmlDocument();
            XmlNode root = xml.CreateElement("Scene");
            xml.AppendChild(root);
            //Hierarchy
            XmlNode hierarchyNode = xml.CreateElement("Hierarchy");
            root.AppendChild(hierarchyNode);
            foreach (var e in hierarchy.root)
                SaveEntity(e, hierarchyNode, xml);
            xml.Save(Path);
        }

        private void SaveEntity(Tree<Entity>.Branch<Entity> branch, XmlNode parent, XmlDocument xml)
        {
            //Entity Header
            XmlNode entityNode = xml.CreateElement("Entity");
            parent.AppendChild(entityNode);
            XmlAttribute activeAtt = xml.CreateAttribute("active");
            activeAtt.Value = branch.Value.active.ToString();
            entityNode.Attributes.Append(activeAtt);
            XmlAttribute nameAtt = xml.CreateAttribute("name");
            nameAtt.Value = branch.Value.name.ToString();
            entityNode.Attributes.Append(nameAtt);
            XmlAttribute layerAtt = xml.CreateAttribute("layer");
            layerAtt.Value = branch.Value.Layer.ToString();
            entityNode.Attributes.Append(layerAtt);

            //Tags
            XmlNode tagsNode = xml.CreateElement("Tags");
            entityNode.AppendChild(tagsNode);
            foreach(string t in branch.Value.tags)
            {
                XmlNode tagNode = xml.CreateElement("Tag");
                tagNode.InnerText = t;
                tagsNode.AppendChild(tagNode);
            }

            //Transform
            XmlNode transformNode = xml.CreateElement("Transform");
            entityNode.AppendChild(transformNode);
            //Position
            XmlNode tPositionNode = xml.CreateElement("Position");
            transformNode.AppendChild(tPositionNode);
            TypeSerialization.SerializeVector3(branch.Value.transform.position, tPositionNode, xml);
            //Rotation
            XmlNode tRotationNode = xml.CreateElement("Rotation");
            transformNode.AppendChild(tRotationNode);
            TypeSerialization.SerializeQuaternion(branch.Value.transform.rotation, tRotationNode, xml);
            //Scale
            XmlNode tScaleNode = xml.CreateElement("Scale");
            transformNode.AppendChild(tScaleNode);
            TypeSerialization.SerializeVector3(branch.Value.transform.scale, tScaleNode, xml);

            //Components
            XmlNode componentsNode = xml.CreateElement("Components");
            entityNode.AppendChild(componentsNode);
            foreach (Component c in branch.Value.components)
                SaveComponent(c, componentsNode, xml);

            //Child Entities
            XmlNode childrenNode = xml.CreateElement("Children");
            entityNode.AppendChild(childrenNode);
            if (branch.BranchCount > 0)
                foreach (var b in branch.Branches)
                    SaveEntity(b, childrenNode, xml);
        }

        private void SaveComponent(Component c, XmlNode parentNode, XmlDocument xml)
        {
            XmlNode componentNode = xml.CreateElement("Component");
            parentNode.AppendChild(componentNode);
            XmlAttribute enabledAtt = xml.CreateAttribute("enabled");
            enabledAtt.Value = c.enabled.ToString();
            componentNode.Attributes.Append(enabledAtt);
            XmlAttribute nameAtt = xml.CreateAttribute("name");
            nameAtt.Value = c.GetType().Name;
            componentNode.Attributes.Append(nameAtt);

            //Fields
            XmlNode fieldsNode = xml.CreateElement("Fields");
            componentNode.AppendChild(fieldsNode);
            foreach(var f in c.GetType().GetFields())
            {
                if (f.Name != "transform" && f.Name != "enabled")
                {
                    XmlNode fieldNode = xml.CreateElement("Field");
                    fieldsNode.AppendChild(fieldNode);
                    XmlAttribute fieldNameAtt = xml.CreateAttribute("name");
                    fieldNameAtt.Value = f.Name;
                    fieldNode.Attributes.Append(fieldNameAtt);
                    XmlAttribute fieldTypeAtt = xml.CreateAttribute("type");
                    fieldTypeAtt.Value = f.FieldType.ToString();
                    fieldNode.Attributes.Append(fieldTypeAtt);
                    TypeSerialization.SerializeField(f, c, fieldNode, xml);
                }
            }
        }

        protected override void SaveMetaData(out List<MetaDataEntry> metaData)
        {
            metaData = new List<MetaDataEntry>();
        }
    }
}
