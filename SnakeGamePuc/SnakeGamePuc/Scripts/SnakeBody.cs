using GameEngine;
using GameEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts
{
    internal enum SnakeDirection
    {
        Left,
        Right,
        Up,
        Down,
    }
    internal class SnakeBody : Script
    {
        private Transform m_transform;
        private SnakeBody m_nextBodyPiece;
        public SnakeDirection Direction { get; set; }

        public SnakeBody(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
        }

        public override void Start()
        {
            m_transform = AttachedGameObject.GetComponent<Transform>();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public void AddBodyPiece()
        {
            if (m_nextBodyPiece == null)
            {
                GameObject obj = ObjsBuilders.BuildSnakeBody();
                obj.GetComponent<Transform>().Position = AttachedGameObject.GetComponent<Transform>().Position;
                m_nextBodyPiece = obj.GetComponent<SnakeBody>();
                GameInstance.Instantiate(obj);
            }
            else
            {
                m_nextBodyPiece.AddBodyPiece();
            }
        }
        public void Move()
        {
            
        }
    }
}
