using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alloy.Components;

namespace Alloy
{
    class Entity
    {
        public bool active { get; private set; }
        public string name;
        public List<string> tags;

        public Entity()
        {
            transform = new Transform();
            tags = new List<string>();
            components = new List<Component>();
        }

        #region Layers
        public int Layer { get; private set; } = 0x0000;
        public void AddToLayer(int layerID)
        {
            if (layerID < 32 && layerID >= 0)
                Layer |= (1 << layerID);
            else
                Logging.LogWarning(this, $"{layerID} is an invalid Layer ID! \n\t Please choose a value between 0(inclusive) and 31(exclusive)!");
        }
        public void SetToLayer(int layerID)
        {
            Layer = 0x0000;
            AddToLayer(layerID);
        }
        public void RemoveFromLayer(int layerID)
        {
            if (layerID < 32 && layerID >= 0)
                Layer ^= (1 << layerID);
            else
                Logging.LogWarning(this, $"{layerID} is an invalid Layer ID! \n\t Please choose a value between 0(inclusive) and 31(exclusive)!");
        }
        public bool IsOnLayer(int layerID)
        {
            if (layerID < 32 && layerID >= 0)
                return (Layer & (1 << layerID)) == (1 << layerID);
            else
                Logging.LogWarning(this, $"{layerID} is an invalid Layer ID! \n\t Please choose a value between 0(inclusive) and 31(exclusive)!");
            return false;
        }
        #endregion

        public Transform transform;
        #region Components
        private List<Component> components;
        public T AddComponent<T>() where T : Component
        {
            components.Add(Activator.CreateInstance<T>());
            components.Last().transform = this.transform;
            return components.Last() as T;
        }
        public T GetComponent<T>() where T : Component                                                                                      
        {
            foreach (Component c in components)                                                                                                                                                                                     
                if (c is T)
                    return (c as T);                                                                                                                                                                                                                                                                                                                                
            return null;
        }
        public bool GetComponent<T>(out T component) where T : Component
        {
            foreach(Component c in components)
                if(c is T)
                {
                    component = c as T;
                    return true;
                }
            component = null;
            return false;
        }
        public List<T> GetComponents<T>() where T : Component
        {
            List<T> matches = new List<T>();
            foreach (Component c in components)
                if (c is T)
                    matches.Add(c as T);
            return matches;
        }
        public void RemoveComponent<T>() where T : Component
        {
            for (int i = 0; i < components.Count; i++)
                if (components[i] is T)
                {
                    components.RemoveAt(i);
                    return;
                }
        }
        public void RemoveComponents<T>() where T : Component
        {
            components.RemoveAll(x => x is T);
        }
        #endregion

        public void SetActive(bool active)
        {
            this.active = active;
        }

        public void Init()
        {
            foreach (Component c in components)
                c.OnInit();
        }

        public void Update()
        {
            if(active)
                foreach (Component c in components)
                    if (c.enabled)
                        c.OnUpdate();
        }
    }
}