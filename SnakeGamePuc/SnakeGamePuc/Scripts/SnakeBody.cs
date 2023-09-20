using GameEngine;
using GameEngine.Components;
using GameEngine.GEMath;
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
        public SnakeBody NextBodyPiece { private get; set; }
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
        }

        public void AddBodyPiece()
        {
            if (NextBodyPiece == null)
            {
                GameObject obj = ObjsBuilders.BuildSnakeBody();
                obj.GetComponent<Transform>().Position = AttachedGameObject.GetComponent<Transform>().Position;
                NextBodyPiece = obj.GetComponent<SnakeBody>();
                GameInstance.Instantiate(obj);
            }
            else
            {
                NextBodyPiece.AddBodyPiece();
            }
        }
        public void Move()
        {
            switch (Direction)
            {
                case SnakeDirection.Up:
                    m_transform.Position += Vector2.Up;
                    break;
                case SnakeDirection.Down:
                    m_transform.Position += Vector2.Down;
                    break;
                case SnakeDirection.Right:
                    m_transform.Position += Vector2.Right;
                    break;
                case SnakeDirection.Left:
                    m_transform.Position += Vector2.Left;
                    break;
            }
            if (NextBodyPiece != null)
            {
                NextBodyPiece.Move();
                NextBodyPiece.Direction = Direction;
            }            
        }
    }
}
