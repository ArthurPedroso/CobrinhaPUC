using GameEngine.GEMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Components
{
    public class Collider : Component
    {
        private Rect m_shape;
        private List<Action> m_onCollisionEnter;
        public Collider(GameObject _attachedGameObject, Rect _shape) : base(_attachedGameObject)
        {
            m_shape = _shape;
            m_onCollisionEnter = new List<Action>();
        }
        public Collider(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
            m_shape = new Rect(new Vector2(-0.5f, 0.5f), new Vector2(0.5f, -0.5f));
            m_onCollisionEnter = new List<Action>();
        }
        internal bool OverlapsOther(Collider _collider)
        {
            Transform localTr = AttachedGameObject.GetComponent<Transform>();
            Transform otherTr = _collider.AttachedGameObject.GetComponent<Transform>();

            return localTr.Position == otherTr.Position;
            //Rect adjusted = new Rect(localTr.ModelMatClone * m_shape.TopLeft, )
        }
        internal void CallCollisionsCB()
        {
            foreach (Action cb in m_onCollisionEnter)
            {
                cb();
            }            
        }
        public void RegisterOnCollisionEnterCB(Action _cb) => m_onCollisionEnter.Add(_cb);
    }
}
