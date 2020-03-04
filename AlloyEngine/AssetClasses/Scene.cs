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
using Alloy.User;

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
            LoadScene(path);
        }

        #region Saving
        protected override void Save()
        {
            SaveScene(Path);
        }
        public void SaveScene(string path)
        {
            XMLAbstraction xml = new XMLAbstraction("Scene");

            //IDs
            var iDsNode = xml.AddNode("IDs");
            iDsNode.AddNode("CurrentID", currentID.ToString());
            var deletedIDsNode = iDsNode.AddNode("DeletedIDs");
            foreach (int i in deletedIDs)
                deletedIDsNode.AddNode("ID", i.ToString());

            //Hierarchy
            var hierarchyNode = xml.AddNode("Hierarchy");
            foreach (var e in hierarchy.root)
                SaveEntity(e, hierarchyNode);
            xml.Save(Path);
        }

        private void SaveEntity(Tree<Entity>.Branch<Entity> branch, XMLAbstraction.Node parent)
        {
            //Entity Header
            var entityNode = parent.AddNode("Entity");
            entityNode.AddAttribute("active", branch.Value.active.ToString());
            entityNode.AddAttribute("name", branch.Value.name.ToString());
            entityNode.AddAttribute("layer", branch.Value.Layer.ToString());
            entityNode.AddAttribute("id", branch.Value.ID.ToString());

            //Tags
            var tagsNode = entityNode.AddNode("Tags");
            foreach (string t in branch.Value.tags)
                tagsNode.AddNode("Tag", t);

            //Transform
            var transformNode = entityNode.AddNode("Transform");
            //Position
            TypeSerialization.SerializeVector3(branch.Value.transform.position, transformNode.AddNode("Position").node, transformNode.xml);
            //Rotation
            TypeSerialization.SerializeQuaternion(branch.Value.transform.rotation, transformNode.AddNode("Rotation").node, transformNode.xml);
            //Scale
            TypeSerialization.SerializeVector3(branch.Value.transform.scale, transformNode.AddNode("Scale").node, transformNode.xml);

            //Components
            var componentsNode = entityNode.AddNode("Components");
            foreach (Component c in branch.Value.components)
                SaveComponent(c, componentsNode);

            //Child Entities
            var childrenNode = entityNode.AddNode("Children");
            if (branch.BranchCount > 0)
                foreach (var b in branch.Branches)
                    SaveEntity(b, childrenNode);
        }

        private void SaveComponent(Component c, XMLAbstraction.Node parentNode)
        {
            var componentNode = parentNode.AddNode("Component");
            componentNode.AddAttribute("enabled", c.enabled.ToString());
            componentNode.AddAttribute("name", c.GetType().FullName);

            //Fields
            var fieldsNode = componentNode.AddNode("Fields");
            foreach(var f in c.GetType().GetFields())
            {
                if (f.Name != "transform" && f.Name != "enabled")
                {
                    var fieldNode = fieldsNode.AddNode("Field");
                    fieldNode.AddAttribute("name", f.Name);
                    fieldNode.AddAttribute("type", f.FieldType.FullName);
                    TypeSerialization.SerializeField(f, c, fieldNode.node, fieldNode.xml);
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

            entity.Init();
        }

        private void LoadComponent(XmlNode componentNode, Entity e)
        {
            bool enabled = Convert.ToBoolean(componentNode.Attributes["enabled"].Value);
            Type type = UserCodeDatabase.GetType(componentNode.Attributes["name"].Value);
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
        protected override void SaveRequiredAssetIDs(out List<int> requiredAtomicAssets, out List<int> requiredComposedAssets)
        {
            requiredAtomicAssets = new List<int>();
            requiredComposedAssets = new List<int>();
        }

        public void Update()
        {
            
            foreach(var e in hierarchy.root)
            {
                e.Value.Update();
            }
        }
    }
}
