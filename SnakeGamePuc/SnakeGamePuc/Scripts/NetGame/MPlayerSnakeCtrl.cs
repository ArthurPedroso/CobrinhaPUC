﻿using GameEngine.Components;
using GameEngine.Input;
using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.GEMath;
using System.Windows.Media.Animation;

namespace SnakeGamePuc.Scripts.NetGame
{
    internal abstract class MPlayerSnakeCtrl : Script
    {
        private const float k_moveAfter = 0.3f;
        private const int k_startingSize = 4;

        private Transform m_transform;
        private SnakeDirection m_direction;
        private SnakeDirection m_lastDirection;
        private float m_elapsedTime;
        private bool m_ready;
        private SnakeBody m_snakeBody;

        private Vector2 m_startPos;

        public MPlayerSnakeCtrl(GameObject _attachedGameObject, Vector2 _startPos) : base(_attachedGameObject)
        {
            m_direction = SnakeDirection.Left;
            m_lastDirection = SnakeDirection.Left;
            m_startPos = _startPos;
            m_ready = false;
        }

        public override void Start()
        {
            m_transform = AttachedGameObject.GetComponent<Transform>();
            m_transform.Position = m_startPos;
            m_elapsedTime = 0;
            BuildBodyParts(m_startPos);
            m_ready = true;
        }

        public override void Update()
        {
            UpdateTimer();
            CheckInput();
        }
        public void EatApple()
        {
            m_snakeBody.AddBodyPiece();
        }
        public void Die()
        {
            GameInstance.SceneMan.LoadScene("LoseScene");
        }
        protected virtual void OnCollision(Collider _collider)
        {
        }
        private void CheckInput()
        {
            if (((GameInstance.Input.KeyPressed(InputKey.A) || GameInstance.Input.KeyPressed(InputKey.ArrowLeft))
                && m_direction != SnakeDirection.Right && m_lastDirection != SnakeDirection.Right))
                m_direction = SnakeDirection.Left;
            else if (((GameInstance.Input.KeyPressed(InputKey.W) || GameInstance.Input.KeyPressed(InputKey.ArrowUp))
                && m_direction != SnakeDirection.Down && m_lastDirection != SnakeDirection.Down))
                m_direction = SnakeDirection.Up;
            else if (((GameInstance.Input.KeyPressed(InputKey.D) || GameInstance.Input.KeyPressed(InputKey.ArrowRight))
                && m_direction != SnakeDirection.Left && m_lastDirection != SnakeDirection.Left))
                m_direction = SnakeDirection.Right;
            else if (((GameInstance.Input.KeyPressed(InputKey.S) || GameInstance.Input.KeyPressed(InputKey.ArrowDown))
                && m_direction != SnakeDirection.Up && m_lastDirection != SnakeDirection.Up))
                m_direction = SnakeDirection.Down;

        }
        private void UpdateTimer()
        {
            m_elapsedTime += GameInstance.Renderer.GetDeltaTime();
            if (m_elapsedTime >= k_moveAfter)
            {
                m_elapsedTime = 0;
                Move();
            }
        }
        protected virtual void BuildBodyParts(Vector2 _startPos)
        {
            Vector2 newPos = m_startPos;
            SnakeBody oldsnake = null;
            GameObject obj;
            SnakeBody snakeBody;
            for (int i = 0; i < k_startingSize; i++)
            {
                obj = ObjsBuilders.BuildSnakeBody();
                newPos += Vector2.Right;
                obj.GetComponent<Transform>().Position += newPos;
                snakeBody = obj.GetComponent<SnakeBody>();
                if (oldsnake != null) oldsnake.NextBodyPiece = snakeBody;
                GameInstance.Instantiate(obj);

                if (i == 0) m_snakeBody = snakeBody;
                oldsnake = snakeBody;
            }
        }

        private void Move()
        {
            switch (m_direction)
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
            m_lastDirection = m_direction;
            m_snakeBody.Move();
            m_snakeBody.Direction = m_direction;
        }

        public Vector2[] GetSnakePos()
        {
            List<Vector2> pos = new List<Vector2>();
            if (m_snakeBody != null)
            {
                pos.AddRange(m_snakeBody.BodyPositions());
            }
            pos.Add(m_transform.Position);

            return pos.ToArray();
        }

        private SnakeDirection InvertDirection(SnakeDirection _dir)
        {
            switch(_dir)
            {
                case SnakeDirection.Up:
                    return SnakeDirection.Down;
                case SnakeDirection.Down:
                    return SnakeDirection.Up;
                case SnakeDirection.Right:
                    return SnakeDirection.Left;
                case SnakeDirection.Left:
                    return SnakeDirection.Right;
                default:
                    return SnakeDirection.None;
            }
        }

        public byte[] SerializeSnake()
        {
            if (!m_ready) return null;
            SnakeBody body = null;

            List<byte> bytes= new List<byte>();
            byte dirCount = 0;
            int dirRef;

            body = m_snakeBody;
            while (body != null)
            {
                if (bytes.Count >= 2)
                {
                    if (bytes[bytes.Count - 2] == (byte)InvertDirection(body.Direction))
                    {
                        bytes[bytes.Count - 1]++;
                    }
                    else
                    {
                        bytes.Add((byte)InvertDirection(body.Direction));
                        bytes.Add(1);
                    }
                }
                else
                {
                    bytes.Add((byte)InvertDirection(body.Direction));
                    bytes.Add(1);
                }

                body = body.NextBodyPiece;
            }

            bytes.Add((byte)float.Round(m_transform.Position.X));
            bytes.Add((byte)float.Round(m_transform.Position.Y));

            return bytes.ToArray();
        }
    }
}
