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
    }
}
