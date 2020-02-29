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
        private Tree<Entity> hierarchy;
        private int currentID = 0;
        private List<int> deletedIDs = new List<int>();
        public Scene(string path) : base(path)
        {
            hierarchy = new Tree<Entity>();
        }

        #region Saving
        public void SaveScene(string path)
        {
            XmlDocument xml = new XmlDocument();
            XmlNode root = xml.CreateElement("Scene");
            xml.AppendChild(root);

            //IDs
            XmlNode iDsNode = xml.CreateElement("IDs");
            root.AppendChild(iDsNode);
            XmlNode currentIDNode = xml.CreateElement("CurrentID");
            currentIDNode.InnerText = currentID.ToString();
            iDsNode.AppendChild(currentIDNode);
            XmlNode deletedIDsNode = xml.CreateElement("DeletedIDs");
            iDsNode.AppendChild(deletedIDsNode);
            foreach(int i in deletedIDs)
            {
                XmlNode deletedIDNode = xml.CreateElement("ID");
                deletedIDNode.InnerText = i.ToString();
                deletedIDsNode.AppendChild(deletedIDNode);
            }

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
            XmlAttribute idAtt = xml.CreateAttribute("id");
            idAtt.Value = branch.Value.ID.ToString();
            entityNode.Attributes.Append(idAtt);

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
            nameAtt.Value = c.GetType().FullName;
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
        #endregion

        #region Loading
        public void LoadScene(string path)
        {
            hierarchy.Clear();

            XmlDocument xml = new XmlDocument();
            xml.Load(path);

            //IDs
            XmlNode iDsNode = xml.SelectSingleNode("//Scene/IDs");
            XmlNode currentIDNode = iDsNode.SelectSingleNode("CurrentID");
            currentID = Convert.ToInt32(currentIDNode.InnerText);
            XmlNodeList deletedIDsNodes = iDsNode.SelectNodes("DeletedIDs/ID");
            deletedIDs.Clear();
            foreach (XmlNode n in deletedIDsNodes)
                deletedIDs.Add(Convert.ToInt32(n.InnerText));

            //Hierarchy
            XmlNode hierarchyNode = xml.SelectSingleNode("//Scene/Hierarchy");
            XmlNodeList entities = hierarchyNode.SelectNodes("*[local-name()='Entity']");
            foreach (XmlNode e in entities)
                LoadEntity(e, null);
        }

        private void LoadEntity(XmlNode entityNode, Tree<Entity>.Branch<Entity> parent)
        {
            bool active = Convert.ToBoolean(entityNode.Attributes["active"].Value);
            string name = entityNode.Attributes["name"].Value;
            int layer = Convert.ToInt32(entityNode.Attributes["layer"].Value);
            int id = Convert.ToInt32(entityNode.Attributes["id"].Value);
            Entity entity;
            Tree<Entity>.Branch<Entity> currentBranch;
            if (parent != null)
                currentBranch = parent.AddItem(new Entity(id, name));
            else
                currentBranch = hierarchy.AddItem(new Entity(id, name));
            entity = currentBranch.Value;
            entity.SetActive(active);
            entity.Layer = layer;

            //Tags
            XmlNodeList tagNodes = entityNode.SelectNodes("Tags/Tag");
            foreach (XmlNode t in tagNodes)
                entity.tags.Add(t.InnerText);

            //Transform
            entity.transform.position = TypeSerialization.DeserializeVector3(entityNode.SelectSingleNode("Transform/Position"));
            entity.transform.rotation = TypeSerialization.DeserializeQuaternion(entityNode.SelectSingleNode("Transform/Rotation"));
            entity.transform.scale = TypeSerialization.DeserializeVector3(entityNode.SelectSingleNode("Transform/Scale"));

            //Components
            XmlNodeList componentNodes = entityNode.SelectNodes("Components/*[local-name()='Component']");
            foreach (XmlNode c in componentNodes)
                LoadComponent(c, entity);

            //Child Entities
            XmlNodeList childNodes = entityNode.SelectNodes("Children/*[local-name()='Entity']");
            foreach (XmlNode ce in childNodes)
                LoadEntity(ce, currentBranch);
        }

        private void LoadComponent(XmlNode componentNode, Entity e)
        {
            bool enabled = Convert.ToBoolean(componentNode.Attributes["enabled"].Value);
            Type type = Type.GetType(componentNode.Attributes["name"].Value);
            Component c = e.AddComponent(type) as Component;
            c.enabled = enabled;

            //Fields
            XmlNodeList fieldNodes = componentNode.SelectNodes("Fields/*[local-name()='Field']");
            foreach(XmlNode f in fieldNodes)
            {
                FieldInfo field = c.GetType().GetField(f.Attributes["name"].Value);
                TypeSerialization.DeserializeField(f, field, c);
            }
        }
        #endregion

        public Tree<Entity>.Branch<Entity> AddEntity(string name = "New Entity")
        {
            return hierarchy.AddItem(new Entity(GetNewID(), name));
        }
        public Tree<Entity>.Branch<Entity> AddEntity(Tree<Entity>.Branch<Entity> parent, string name = "New Entity")
        {
            if (parent != null)
                return parent.AddItem(new Entity(GetNewID(), name));
            else
                return hierarchy.AddItem(new Entity(GetNewID(), name));
        }

        public int GetNewID()
        {
            if(deletedIDs.Count > 0)
            {
                int id = deletedIDs.Last();
                deletedIDs.RemoveAt(deletedIDs.Count - 1);
                return id;
            }
            return currentID++;
        }

        protected override void SaveMetaData(out List<MetaDataEntry> metaData)
        {
            metaData = new List<MetaDataEntry>();
        }
    }
}
