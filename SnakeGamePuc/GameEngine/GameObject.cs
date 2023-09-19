using GameEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class GameObject
    {
        protected List<Component> Components { get; private set; }


        public GameObject() 
        {
            Components = new List<Component>();
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
    }
}
