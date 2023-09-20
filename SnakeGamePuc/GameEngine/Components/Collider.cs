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
        private List<Action<Collider>> m_onCollisionEnter;
        public Collider(GameObject _attachedGameObject, Rect _shape) : base(_attachedGameObject)
        {
            m_shape = _shape;
            m_onCollisionEnter = new List<Action<Collider>>();
        }
        public Collider(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
            m_shape = new Rect(new Vector2(-0.5f, 0.5f), new Vector2(0.5f, -0.5f));
            m_onCollisionEnter = new List<Action<Collider>>();
        }
        internal bool OverlapsOther(Collider _collider)
        {
            Transform localTr = AttachedGameObject.GetComponent<Transform>();
            Transform otherTr = _collider.AttachedGameObject.GetComponent<Transform>();

            return (localTr.Position - otherTr.Position).Magnitude() <= 0.001f;
            //Rect adjusted = new Rect(localTr.ModelMatClone * m_shape.TopLeft, )
        }
        internal void CallCollisionsCB(Collider _collider)
        {
            foreach (Action<Collider> cb in m_onCollisionEnter)
            {
                cb(_collider);
            }            
        }
        public void RegisterOnCollisionEnterCB(Action<Collider> _cb) => m_onCollisionEnter.Add(_cb);
    }
}
