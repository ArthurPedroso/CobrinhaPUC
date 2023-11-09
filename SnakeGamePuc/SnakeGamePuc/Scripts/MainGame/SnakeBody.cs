using GameEngine;
using GameEngine.Components;
using GameEngine.GEMath;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts
{
    internal enum SnakeDirection : byte
    {
        Left    = 1,
        Right   = 2,
        Up      = 3,
        Down    = 4,
        None    = 0
    }
    internal class SnakeBody : Script
    {
        private Transform m_transform;
        public SnakeBody NextBodyPiece { get; set; }
        public SnakeDirection Direction { get; set; }

        public SnakeBody(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
            NextBodyPiece = null;
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
                NextBodyPiece.Direction = SnakeDirection.None;
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
                default:
                    break;
            }
            if (NextBodyPiece != null)
            {
                NextBodyPiece.Move();
                NextBodyPiece.Direction = Direction;
            }            
        }
        public Vector2[] BodyPositions()
        {
            List<Vector2> pos = new List<Vector2>();
            if (NextBodyPiece != null)
            { 
                pos.AddRange(NextBodyPiece.BodyPositions());
            }
            pos.Add(m_transform.Position);

            return pos.ToArray();
        }
    }
}
