﻿using GameEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public sealed class GameObject
    {
        protected HashSet<Component> Components { get; private set; }

        public string Name { get; set; }

        public GameObject(string _name) 
        {
            Components = new HashSet<Component>();
            Name = _name;
        }
        public GameObject()
        {
            Components = new HashSet<Component>();
        }
        public void AttachComponent(Component _component)
        {
            Components.Add(_component);
        }

        /// <summary>
        /// Retorna a primeira ocorrencia de componente com o mesmo tipo que T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : Component 
        {
            foreach (Component component in Components)
            {
                if (component is T) return (T)component;
            }
            return null;
        }
        public T[] GetComponents<T>() where T : Component
        {
            List<T> components = new List<T>();
            foreach (Component component in Components)
            {
                if (component is T) components.Add((T)component);
            }
            return components.ToArray();
        }
    }
}
